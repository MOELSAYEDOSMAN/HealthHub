namespace HealthHup.API.Service.ImageService
{
    public class SaveImage : ISaveImage
    {
        private readonly IWebHostEnvironment env;
        public SaveImage(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public Task<bool> DeleteImage(string src)
        {
            var filepath = Path.Combine(env.ContentRootPath + System.IO.Path.DirectorySeparatorChar, $@"wwwroot/Image/{src}");
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
                return Task.FromResult<bool>(true);
            }
            else
            {
                return Task.FromResult<bool>(false);
            }
        }

        public Task<bool> DeletsImages(List<string> src)
        {
            bool result = true;

            foreach (string s in src)
            {
                DeleteImage(s);
            }
            return Task.FromResult<bool>(result);
        }

        public async Task<string> UploadImage(string src, IFormFile img)
        {
            string NameImg = Guid.NewGuid().ToString() + ".webp";
            var filepath = Path.Combine(env.ContentRootPath + System.IO.Path.DirectorySeparatorChar, $@"wwwroot/Image/{src}", NameImg);
            using (var steam = System.IO.File.Create(filepath))
            {
                await img.CopyToAsync(steam);
            }
            return NameImg;
        }

        public async Task<bool> ChangeImg(string src, IFormFile img)
        {
            await DeleteImage(src);
            src = Path.Combine(env.ContentRootPath + System.IO.Path.DirectorySeparatorChar, $@"wwwroot/Image/{src}");
            using (var steam = System.IO.File.Create(src))
            {
                await img.CopyToAsync(steam);
            }
            return true;
        }

        public async Task<List<string>> UploadImagesList(string src, List<IFormFile> imgs)
        {
            List<string> images = new List<string>();
            foreach (IFormFile file in imgs)
            {
                string NameImg = Guid.NewGuid().ToString() + ".webp";
                var filepath = Path.Combine(env.ContentRootPath + System.IO.Path.DirectorySeparatorChar, $@"wwwroot/Image/{src}", NameImg);
                using (var steam = System.IO.File.Create(filepath))
                {
                    await file.CopyToAsync(steam);
                }
                images.Add(NameImg);
            }
            return images;
        }
    }
}
