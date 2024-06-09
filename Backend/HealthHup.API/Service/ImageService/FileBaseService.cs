using Microsoft.AspNetCore.Hosting;
using System.IO;
using static System.Net.WebRequestMethods;

namespace HealthHup.API.Service.ImageService
{
    public class FileBaseService
    {
        private readonly IWebHostEnvironment env;
        public FileBaseService(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public async Task UploadFile(IFormFile FIle,string FileName,string FileExtion,string path)
        {
            var SaveFile = Path.Combine(env.ContentRootPath + System.IO.Path.DirectorySeparatorChar, $@"wwwroot/{path}", $"{FileName}.{FileExtion}");
            using (var steam = System.IO.File.Create(SaveFile))
            {
                await FIle.CopyToAsync(steam);
            }
        }
        public FileStream ReadFile(string path)
        {
            var image = System.IO.File.OpenRead(path);
            return image;
        }
        public async Task ChangeFile(IFormFile file,string src, string FileName)
        {
            if (!await RemoveFile(src, FileName))
                return;

            src = Path.Combine(env.ContentRootPath + System.IO.Path.DirectorySeparatorChar, $@"wwwroot/{src}/{FileName}");
            using (var steam = System.IO.File.Create(src))
            {
                await file.CopyToAsync(steam);
            }

        }
        
        public async Task<bool> RemoveFile(string src,string FileName)
        {
            var filepath = Path.Combine(env.ContentRootPath + System.IO.Path.DirectorySeparatorChar, $@"wwwroot/{src}/{FileName}");
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
                return true;
            }
            return false;
        }
    }
}
