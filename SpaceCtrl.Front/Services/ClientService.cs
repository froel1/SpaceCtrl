using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SpaceCtrl.Data.Helpers;
using SpaceCtrl.Data.Interfaces;
using SpaceCtrl.Data.Models.ClientSync;
using SpaceCtrl.Data.Models.Database;
using SpaceCtrl.Front.Models.Client;
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
            var newClient = new Client
            {
                Guid = Guid.NewGuid(),
                FirstName = client.FirstName,
                LastName = client.LastName,
                CreateDate = DateTime.Now,
                TargetId = 1,
                IsActive = true,
                Type = (int)client.Type
            };

            var (folderPath, imageNames) = await SaveImagesAsync(newClient.Guid, files);

            newClient.SyncRequestedAt = DateTime.Now;
            newClient.SyncDetails = CreateClientSyncDetails(newClient, folderPath, imageNames);

            _dbContext.Client.Add(newClient);

            await _dbContext.SaveChangesAsync();
        }

        private static string CreateClientSyncDetails(Client client, string folderPath, List<string> images)
        {
            var syncDetails = new ClientSyncDetails
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
                foreach (var (formFile, fileName) in files.Where(formFile => formFile.Length > 0).Select(ValidateImage()).ToList())
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

        private string CreateFolder(Guid clientId)
        {
            var folderPath = Path.Combine(_settings.Image.BasePath, clientId.ToString());
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

        public async Task RemoveClientAsync(int clientId)
        {
            var client = await _dbContext.Client.FirstAsync(x => x.Id == clientId);

            client.IsActive = !client.IsActive;

            await _dbContext.SaveChangesAsync();
        }
    }
}