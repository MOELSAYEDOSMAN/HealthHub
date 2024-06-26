﻿using HealthHup.API.Model.Extion.Ml;

namespace HealthHup.API.Service.MlService
{
    public interface IMLApiService
    {
        Task<interactionModelDto> GetTypeInteraction(string smile1, string smiles2);
        Task<string> BoneXrayPrediction(IFormFile File);
    }
}
