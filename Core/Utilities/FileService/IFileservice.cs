using Microsoft.AspNetCore.Hosting;
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
        Task<string> UploadAsync(IFormFile file);
        void Delete(string filename);
        bool CheckPhoto(IFormFile file);
        bool MaxSize(IFormFile file, int maxSize);
    }
}
