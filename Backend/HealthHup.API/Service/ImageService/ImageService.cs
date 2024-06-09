
namespace HealthHup.API.Service.ImageService
{
    public class ImageService : ISaveImage
    {
        private readonly FileBaseService _fileBaseService;
        public ImageService(IWebHostEnvironment env)
        {
            _fileBaseService=new FileBaseService(env);
        }
        public async Task<FileStream> GetImage(string uri)
        {
            return _fileBaseService.ReadFile($"wwwroot/Image/{uri}");
        }
        public async Task<string> UploadImage(string src, IFormFile img)
        {
            var FileName=Guid.NewGuid().ToString();
            var FileExtion = "webp";
            var Src = $"Image/{src}";
            await _fileBaseService.UploadFile(img, FileName, FileExtion, Src);
            return $"{FileName}.{FileExtion}";
        }
        public async Task<bool> ChangeImg(string src, IFormFile img)
        {
            var path = src.Substring(0, src.IndexOf('/'));
            var fileName = src.Substring(src.IndexOf('/') + 1, src.Count()-path.Count()-1);
            await _fileBaseService.ChangeFile(img, $"Image/{path}", fileName);
            return true ;
        }

        public async Task<bool> DeleteImage(string src)
        {
            var path = src.Substring(0, src.IndexOf('/'));
            var fileName = src.Substring(src.IndexOf('/') + 1, src.Count() - path.Count() - 1);
            return await _fileBaseService.RemoveFile($"Image/{path}", fileName);
        }

        public async Task<bool> DeletsImages(List<string> src)
        {
            foreach (var i in src)
                await DeleteImage(i);
            return true;
        }

        

        public async IAsyncEnumerable<string> UploadImagesList(string src, List<IFormFile> imgs)
        {
            foreach(var i in imgs)
                yield return await UploadImage(src, i);
        }
    }
}
