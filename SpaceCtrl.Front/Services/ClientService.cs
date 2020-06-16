using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using SpaceCtrl.Data.Database.DbObjects;
using SpaceCtrl.Data.Extensions;
using SpaceCtrl.Data.Helpers;
using SpaceCtrl.Data.Models.ClientSync;
using SpaceCtrl.Front.Extensions;
using SpaceCtrl.Front.Models.Client;
using SpaceCtrl.Front.Models.Client.Groups;
using SpaceCtrl.Front.Models.Common;
using SpaceCtrl.Front.Models.Settings;

namespace SpaceCtrl.Front.Services
{
    public class ClientService
    {
        private readonly SpaceCtrlContext _dbContext;
        private readonly AppSettings _settings;

        public ClientService(SpaceCtrlContext dbContext, IOptions<AppSettings> options)
        {
            _dbContext = dbContext;
            _settings = options.Value;
        }

        public async Task<int> AddAsync(ClientModel client, IList<IFormFile> files)
        {
            var person = await UpdateOrCreatePersonAsync(client);

            var savedImages = await SaveImagesAsync(person, files);

            var syncType = SyncOperationType.UpdateClient;

            if (!client.Id.HasValue)
            {
                _dbContext.Person.Add(person);
                syncType = SyncOperationType.NewClient;
            }

            UpdatePersonSyncDetails(person, syncType);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error("Can't save person, ex: {e}", e);
                savedImages.ForEach(File.Delete);
                throw;
            }

            return person.Id;
        }

        private async Task<Person> UpdateOrCreatePersonAsync(ClientModel client)
        {
            Person person;

            if (!client.Id.HasValue)
            {
                person = new Person
                {
                    Key = Guid.NewGuid(),
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    CreateDate = DateTime.Now,
                    GroupId = client.GroupId,
                    IsActive = true
                };
            }
            else
            {
                person = await _dbContext.Person
                    .Include(x => x.PersonImages).ThenInclude(x => x.Frame)
                    .FirstAsync(x => x.Id == client.Id && x.IsActive);

                person.FirstName = client.FirstName;
                person.LastName = client.LastName;
                person.GroupId = client.GroupId;

                RemoveOldImages(person, client);
            }

            return person;
        }

        public async Task<PagedList<ClientDetails>> GetAsync(PaginationWithFilter<ClientFilterModel> filter)
        {
            var count = await _dbContext.Person.CountAsync(x => x.IsActive);
            var rawData = _dbContext.Person.Where(x => x.IsActive);

            if (filter.Filter is { })
            {
                if (filter.Filter.GroupId.HasValue)
                    rawData = rawData.Where(x => x.GroupId == filter.Filter.GroupId.Value);
                if (!string.IsNullOrEmpty(filter.Filter.Name))
                    rawData = rawData.Where(x => (x.FirstName + x.LastName).Contains(filter.Filter.Name));
            }

            var data = await rawData.OrderBy(x => x.Id).ToPagedList(filter)
                .Select(x => new ClientDetails(
                    x.Id,
                    x.FirstName,
                    x.LastName,
                    x.Group.Name,
                    x.CreateDate,
                    x.IsActive
                )).ToListAsync();

            return new PagedList<ClientDetails>(count, data);
        }

        public async Task<ClientModel> GetAsync(int clientId) => await _dbContext.Person.Where(x => x.Id == clientId)
            .Include(x => x.PersonImages).ThenInclude(x => x.Frame)
            .Select(x => new ClientModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                GroupId = x.GroupId,
                Images = x.PersonImages.Select(images => new ImageListModel
                {
                    Id = images.Frame.Id,
                    Path = images.Frame.GetWebPath(_settings.Image, ImageType.User)
                }).ToList()
            }).FirstAsync();

        private static void UpdatePersonSyncDetails(Person client, SyncOperationType operationType)
        {
            var syncDetails = client.SyncDetails.DeserializeToObject<PersonSyncDetails>(false) ??
                              new PersonSyncDetails();

            syncDetails.SyncDetails = new SyncDetails
            {
                Type = operationType
            };

            client.SyncRequestedAt = DateTime.Now;
            client.SyncDetails = syncDetails.ToJson();
        }

        private Func<IFormFile, (IFormFile, string)> ValidateImage() => file =>
        {
            if (file.Length > _settings.Image.MaxSize)
                throw new ValidationException($"File size is too big: {file.Name}");

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!_settings.Image.AllowedExtensions.Contains(ext))
                throw new ValidationException($"Not Supported Extension: {file.Name}");

            return (file, ext);
        };

        public async Task RemoveClientAsync(int personId)
        {
            var person = await _dbContext.Person
                .Include(x => x.PersonImages).ThenInclude(x => x.Frame)
                .FirstAsync(x => x.Id == personId);

            person.IsActive = !person.IsActive;

            DeleteImagesFromStorage(person.PersonImages);

            UpdatePersonSyncDetails(person, SyncOperationType.DeleteClient);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteClientAsync(int id)
        {
            var person = await _dbContext.Person.FirstOrDefaultAsync(x => x.Id == id);

            if (person is null)
                throw new InvalidOperationException("Client not found");
            if (!person.IsActive)
                throw new InvalidOperationException("Client is not active");

            person.IsActive = false;
            await _dbContext.SaveChangesAsync();
        }

        #region groups

        public async Task<List<Dropdown>> GetGroupsAsync() =>
            await _dbContext.PersonGroup.Select(x => new Dropdown(x.Id, x.Name)).ToListAsync();

        public async Task<List<Dropdown>> GetGroupMembersAsync(int clientId)
        {
            var data = await (from per in _dbContext.Person
                              join pp in _dbContext.Person on per.GroupId equals pp.GroupId into grp
                              from perGrp in grp.DefaultIfEmpty()
                              where per.Id == clientId && perGrp.IsActive && perGrp.Id != clientId
                              select new Dropdown(
                                  perGrp.Id,
                                  $"{perGrp.FirstName} {perGrp.LastName}"
                              )).ToListAsync();
            return data;
        }

        public async Task<int> CreateGroupAsync(GroupModel model)
        {
            var group = new PersonGroup
            {
                Name = model.Name,
                Description = model.Description ?? string.Empty
            };
            _dbContext.PersonGroup.Add(group);
            await _dbContext.SaveChangesAsync();
            return group.Id;
        }

        public async Task DeleteGroup(int id)
        {
            var group = await _dbContext.PersonGroup.Include(x => x.Person).FirstAsync(x => x.Id == id);

            if (group.Person.Any(x => x.IsActive))
                throw new ValidationException("Group with active members can't be deleted");

            _dbContext.PersonGroup.Remove(group); //todo: in active persons ??

            await _dbContext.SaveChangesAsync();
        }

        #endregion groups

        #region Images

        public async Task<List<string>> SaveImagesAsync(Person person, IEnumerable<IFormFile> files)
        {
            var savedImages = new List<string>();

            foreach (var (formFile, extension) in files.Where(formFile => formFile.Length > 0)
                .Select(ValidateImage()).ToList())
            {
                var imageId = Guid.NewGuid();
                var imagePath = Path.Combine(_settings.Image.BasePath, _settings.Image.UserImagesPath, $"{imageId}{extension}");

                try
                {
                    await using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await formFile.CopyToAsync(fileStream);
                }
                catch (Exception e)
                {
                    Log.Error("Can't save images, ex: {e}", e);
                    savedImages.ForEach(File.Delete);
                    throw;
                }

                savedImages.Add(imagePath);

                person.PersonImages.Add(new PersonImages
                {
                    Frame = new Frame
                    {
                        CreateDate = DateTime.Now,
                        Id = imageId,
                        Type = extension
                    }
                });
            }

            return savedImages;
        }

        private void DeleteImagesFromStorage(IEnumerable<PersonImages> imagesToDelete)
        {
            if (imagesToDelete is null)
                return;

            foreach (var personImage in imagesToDelete)
            {
                var path = personImage.Frame.GetPath(_settings.Image, ImageType.User);
                File.Delete(path);
            }
        }

        private void RemoveOldImages(Person person, ClientModel client)
        {
            var imageIds = client?.Images.Select(x => x.Id).ToList() ?? new List<Guid>();
            var imagesToDelete = person.PersonImages.Where(x => !imageIds.Contains(x.FrameId)).ToList();
            if (!imagesToDelete.Any())
                return;

            _dbContext.PersonImages.RemoveRange(imagesToDelete);
            _dbContext.Frame.RemoveRange(imagesToDelete.Select(x => x.Frame));

            DeleteImagesFromStorage(imagesToDelete);
        }

        #endregion Images
    }
}