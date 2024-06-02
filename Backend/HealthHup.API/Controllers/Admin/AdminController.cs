using HealthHup.API.Model.Extion.AdminLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.Sig;
using System.Security.Claims;
using System.Text.RegularExpressions;


namespace HealthHup.API.Controllers.Admin
{
    [Route("[controller]"),Authorize(Roles ="Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBaseService<ApplicationUser> _normalUser;
        private readonly IDoctorService _doctorService;
        private readonly IAdminLogs _adminLogs;
        public AdminController(IBaseService<ApplicationUser> normalUser, IDoctorService doctorService
            ,IAdminLogs adminLogs)
        {
            _normalUser = normalUser;
            _doctorService= doctorService;
            _adminLogs = adminLogs;
        }
        
        [HttpGet("PatientCount")]
        public async Task<IActionResult> PatientCount()
            => Ok(await _normalUser.CountAsync());
        [HttpGet("DoctorCount")]
        public async Task<IActionResult> DoctorCount()
            => Ok(await _doctorService.CountAsync());
        [HttpGet("AdminLogs")]
        public async Task<IActionResult> AdminLogs(string?AdminEmail)
            =>Ok(AdminLogsDTO.ConvertFromLogAdminAction(await _adminLogs.GetAdminActions(AdminEmail ?? User.FindFirstValue(ClaimTypes.Email))));
    }
}
