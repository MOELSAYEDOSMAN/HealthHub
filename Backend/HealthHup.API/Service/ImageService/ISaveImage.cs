namespace HealthHup.API.Service.ImageService
{
    public interface ISaveImage
    {
        Task<string> UploadImage(string src, IFormFile img);
        IAsyncEnumerable<string> UploadImagesList(string src, List<IFormFile> imgs);
        Task<bool> DeleteImage(string src);
        Task<bool> DeletsImages(List<string> src);
        Task<bool> ChangeImg(string src, IFormFile img);
    }
}
