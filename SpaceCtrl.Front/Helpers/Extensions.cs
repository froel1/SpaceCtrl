using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SpaceCtrl.Front.Helpers
{
    public static class Extensions
    {
        public static async Task<List<string>> ToBase64StringAsync(this IList<IFormFile> files)
        {
            var fileBytes = new List<string>();

            foreach (var formFile in files.Where(formFile => formFile.Length > 0))
            {
                await using var stream = new MemoryStream();
                await formFile.CopyToAsync(stream);
                fileBytes.Add(Convert.ToBase64String(stream.ToArray()));
            }

            return fileBytes;
        }
    }
}
