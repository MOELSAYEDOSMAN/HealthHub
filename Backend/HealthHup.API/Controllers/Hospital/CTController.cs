using HealthHup.API.Service.MlService.MLCT;
using HealthHup.API.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthHup.API.Controllers.Hospital
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Doctor")]
    public class CTController : ControllerBase
    {
        private readonly ICTService _CTService;
        public CTController(ICTService _CTService)
        {
            this._CTService= _CTService;
        }
        [HttpPost]
        public async Task<IActionResult> CheackCT([FileValidation] IFormFile img)
        {
            return Ok(await _CTService.CT_File_Result(img));
        }
    }
}
