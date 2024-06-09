using HealthHup.API.Model.Extion.Ml;
using System.Text;
using System.Text.Json;

namespace HealthHup.API.Service.MlService
{
    public class MLApiService : IMLApiService
    {
        private readonly HttpClient _httpClient;

        public MLApiService(HttpClient httpClient)
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
        public async Task<string> BoneXrayPrediction(IFormFile File)
        {

            var body = new MultipartFormDataContent();
            body.Add(new StringContent(JsonSerializer.Serialize(File)), "file",File.FileName);
            var data = await _httpClient.PostAsync("/predict", body);
            return await JsonSerializer.DeserializeAsync<string>(data.Content.ReadAsStream(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        }
    }
}
