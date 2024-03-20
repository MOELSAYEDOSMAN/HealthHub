using HealthHup.API.Model.Extion.Ml;
using System.Text;
using System.Text.Json;

namespace HealthHup.API.Service.MlService
{
    public class MLDrugApiService : IMLDrugApiService
    {
        private readonly HttpClient _httpClient;

        public MLDrugApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<interactionModelDto> GetTypeInteraction( string smile1,string smiles2)
        {
            var body = new
            {
                drug_1 = smile1,
                drug_2 = smiles2,
            };
            var input = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var data = await _httpClient.PostAsync("/CheckInteraction", input);
            return await JsonSerializer.DeserializeAsync<interactionModelDto>(data.Content.ReadAsStream(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
