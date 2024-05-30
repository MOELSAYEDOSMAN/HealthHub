using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.Sig;
using System.Text.RegularExpressions;


namespace HealthHup.API.Controllers.Admin
{
    [Route("[controller]"),Authorize(Roles ="Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBaseService<ApplicationUser> _normalUser;
        private readonly IDoctorService _doctorService;

        public AdminController(IBaseService<ApplicationUser> normalUser, IDoctorService doctorService)
        {
            _normalUser = normalUser;
            _doctorService= doctorService;
        }
        
        [HttpGet("PatientCount")]
        public async Task<IActionResult> PatientCount()
            => Ok(await _normalUser.CountAsync());
        [HttpGet("DoctorCount")]
        public async Task<IActionResult> DoctorCount()
            => Ok(await _doctorService.CountAsync());
    }
}
