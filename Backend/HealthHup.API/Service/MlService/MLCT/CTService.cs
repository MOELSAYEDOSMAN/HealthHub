
namespace HealthHup.API.Service.MlService.MLCT
{
    public class CTService : ICTService
    {
        readonly ISaveImage _SaveImage; 
        readonly IMLApiService _MLApiService;
        public CTService(ISaveImage SaveImage, IMLApiService mLApiService
            ) 
        {
          
            _SaveImage = SaveImage;
            _MLApiService = mLApiService;
        }
        public async Task<string> CT_File_Result(IFormFile CTFile)
        {
            return await _MLApiService.BoneXrayPrediction(CTFile);
        }

        public async Task<string> CT_URI_Result(string CTUri)
        {
            var fileStream = await _SaveImage.GetImage(CTUri);

            // Reset the fileStream position to ensure the stream is read from the beginning
            fileStream.Position = 0;

            // Create a FormFile from the FileStream
            var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", "image.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            // Call the prediction method with the FormFile
            var result = await _MLApiService.BoneXrayPrediction(formFile);

            return result;
        }
    }
}
