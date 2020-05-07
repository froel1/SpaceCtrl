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
using SpaceCtrl.Data.Database.DbObjects;
using SpaceCtrl.Data.Helpers;
using SpaceCtrl.Data.Interfaces;
using SpaceCtrl.Data.Models.ClientSync;
using SpaceCtrl.Front.Extensions;
using SpaceCtrl.Front.Models.Client;
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

        public async Task AddAsync(NewClientModel client, IList<IFormFile> files)
        {
            var newPerson = new Person
            {
                Key = Guid.NewGuid(),
                FirstName = client.FirstName,
                LastName = client.LastName,
                CreateDate = DateTime.Now,
                IsActive = true
            };

            var (folderPath, imageNames) = await SaveImagesAsync(newPerson.Key, files);

            newPerson.SyncRequestedAt = DateTime.Now;
            newPerson.SyncDetails = CreatePersonSyncDetails(newPerson, folderPath, imageNames);

            _dbContext.Person.Add(newPerson);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagedList<ClientDetails>> GetAsync(PaginationWithFilter<ClientFilterModel> filter)
        {
            var count = await _dbContext.Person.CountAsync();
            var rawData = _dbContext.Person.Where(x => x.IsActive);

            if (filter.Filter is { })
            {
                if (filter.Filter.GroupId.HasValue)
                    rawData = rawData.Where(x => x.GroupId == filter.Filter.GroupId.Value);
                if (!string.IsNullOrEmpty(filter.Filter.Name))
                    rawData = rawData.Where(x => (x.FirstName + x.LastName).Contains(filter.Filter.Name));
            }

            var data = await rawData.OrderBy(x => x.Id).ToPagedList(filter)
            .Select(PersonToClientDetails()).ToListAsync();

            return new PagedList<ClientDetails>(count, data);
        }

        public async Task<ClientDetails> GetAsync(int clientId) => await _dbContext.Person.Where(x => x.Id == clientId)
            .Select(PersonToClientDetails()).FirstAsync();

        private static Expression<Func<Person, ClientDetails>> PersonToClientDetails()
        {
            return person => new ClientDetails(
                person.Id,
                person.FirstName,
                person.LastName,
                person.Group.Name,
                ClientType.Undefined,
                person.CreateDate,
                person.IsActive
            );
        }

        private static string CreatePersonSyncDetails(Person client, string folderPath, List<string> images)
        {
            var syncDetails = new PersonSyncDetails
            {
                SyncDetails = new SyncDetails
                {
                    Type = SyncOperationType.NewClient,
                    ImagePath = folderPath,
                    Images = images
                }
            };

            return syncDetails.ToJson();
        }

        private async Task<(string, List<string>)> SaveImagesAsync(Guid id, IEnumerable<IFormFile> files)
        {
            var folderPath = CreateFolder(id);
            var images = new List<string>();

            try
            {
                var index = 0;
                foreach (var (formFile, fileName) in files.Where(formFile => formFile.Length > 0)
                    .Select(ValidateImage()).ToList())
                {
                    var imageName = $"{++index}_{fileName}";
                    images.Add(imageName);
                    var imagePath = Path.Combine(folderPath, imageName);
                    await using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await formFile.CopyToAsync(fileStream);
                }
            }
            catch
            {
                Directory.Delete(folderPath, true);
                throw;
            }

            return (folderPath, images);
        }

        private string CreateFolder(Guid personKey)
        {
            var folderPath = Path.Combine(_settings.Image.BasePath, personKey.ToString());
            if (File.Exists(folderPath))
                throw new InvalidOperationException($"directory already exist: {folderPath}");

            Directory.CreateDirectory(folderPath);
            return folderPath;
        }

        private Func<IFormFile, (IFormFile, string)> ValidateImage() => file =>
        {
            if (file.Length > _settings.Image.MaxSize)
                throw new ValidationException($"File size is too big: {file.Name}");

            var ext = Path.GetExtension(file.FileName);
            if (!_settings.Image.AllowedExtensions.Contains(ext))
                throw new ValidationException($"Not Supported Extension: {file.Name}");

            var fileName = $"{DateTime.Today.Ticks}_image{ext}";

            return (file, fileName);
        };

        public async Task RemoveClientAsync(int personId)
        {
            var person = await _dbContext.Person.FirstAsync(x => x.Id == personId);

            person.IsActive = !person.IsActive;//TODO:cot add sync

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Dropdown>> GetGroupsAsync() => await _dbContext.PersonGroup.Select(x => new Dropdown(x.Id, x.Name)).ToListAsync();
    }
}