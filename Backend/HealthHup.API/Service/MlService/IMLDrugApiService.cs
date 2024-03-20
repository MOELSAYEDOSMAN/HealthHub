using HealthHup.API.Model.Extion.Ml;

namespace HealthHup.API.Service.MlService
{
    public interface IMLDrugApiService
    {
        Task<interactionModelDto> GetTypeInteraction(string smile1, string smiles2);
    }
}
