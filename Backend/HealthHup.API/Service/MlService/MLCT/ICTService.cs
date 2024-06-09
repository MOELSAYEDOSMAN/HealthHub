namespace HealthHup.API.Service.MlService.MLCT
{
    public interface ICTService
    {
        Task<string> CT_File_Result(IFormFile CTFile);
        Task<string> CT_URI_Result(string CTUri);

    }
}
