using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions.FileService
{
    public interface IFileservice
    {
        Task<string> UploadAsync(IFormFile file, string webrootPath);
        void Delete(string webrootPath, string filename);
        bool CheckPhoto(IFormFile file);
        bool MaxSize(IFormFile file, int maxSize);
    }
}
