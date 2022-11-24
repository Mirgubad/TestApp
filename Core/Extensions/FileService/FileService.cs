using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions.FileService
{
    public class FileService : IFileservice
    {
        public async Task<string> UploadAsync(IFormFile file, string webrootPath)
        {
            var filename = $"{Guid.NewGuid()}_{file.FileName}";
            string path = Path.Combine(webrootPath, "images", filename);
            using (FileStream fileStream = new(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(fileStream);
            }
            return filename;
        }

        public void Delete(string webrootPath, string filename)
        {
            string path = Path.Combine(webrootPath, "images", filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public bool CheckPhoto(IFormFile file)
        {
            if (file.ContentType.Contains("image/"))
            {
                return true;
            }
            return false;
        }

        public bool MaxSize(IFormFile file, int maxSize)
        {
            if (file.Length / 1024 > maxSize)
            {
                return false;
            }
            return true;
        }
    }
}
