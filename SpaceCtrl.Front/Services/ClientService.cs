using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SpaceCtrl.Data.Interfaces;
using SpaceCtrl.Data.Models.Database;
using SpaceCtrl.Front.Models.Client;
using SpaceCtrl.Front.Models.Settings;

namespace SpaceCtrl.Front.Services
{
    public class ClientService
    {
        private readonly SpaceCtrlContext _dbContext;
        private readonly ISpaceCtrlCamera _spaceCtrl;
        private readonly AppSettings _settings;

        public ClientService(SpaceCtrlContext dbContext, ISpaceCtrlCamera spaceCtrl, IOptions<AppSettings> options)
        {
            _dbContext = dbContext;
            _spaceCtrl = spaceCtrl;
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
                IsActive = true
            };
            await SaveImagesAsync(newClient.Guid, files);
            _dbContext.Client.Add(newClient);

            await _dbContext.SaveChangesAsync();
        }

        private async Task SaveImagesAsync(Guid id, IEnumerable<IFormFile> files)
        {
            var folderPath = Path.Combine(_settings.Image.BasePath, id.ToString());
            if (File.Exists(folderPath))
                throw new InvalidOperationException($"directory already exist: {folderPath}");

            Directory.CreateDirectory(folderPath);

            var index = 0;
            foreach (var (formFile, fileName) in files.Where(formFile => formFile.Length > 0).Select(ValidateImage()).ToList())
            {
                var imagePath = Path.Combine(folderPath, $"{++index}_{fileName}");
                await using var fileStream = new FileStream(imagePath, FileMode.Create);
                await formFile.CopyToAsync(fileStream);
            }
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
            await _spaceCtrl.RemoveObjectAsync(client.Guid);
            client.IsActive = !client.IsActive;
            await _dbContext.SaveChangesAsync();
        }
    }

}
