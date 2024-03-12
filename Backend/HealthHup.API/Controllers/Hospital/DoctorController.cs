using HealthHup.API.Model.Extion.Account;
using HealthHup.API.Service.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HealthHup.API.Controllers.Hospital
{
    [Route("Hospital/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientDatesService _patientDatesService;
        public DoctorController(IDoctorService doctorService, IPatientDatesService patientDatesService,IAuthService authService)
        {
            _doctorService = doctorService;
            _patientDatesService = patientDatesService;
            _authService = authService;

        }
        //Get Doctors Active
        [HttpGet("CheackRoleDoctor"),Authorize]
        public async Task<IActionResult> CheackRoleDoctor()
        {
            if(User.IsInRole("Doctor"))
                return BadRequest(new OUser()
                {
                    Error = true,
                    Message = "You did it Before"
                });

            return Ok(await _authService.CheackDoctorRoleAsync(User.FindFirstValue(ClaimTypes.Email)));
        }
        [HttpGet("GetDoctorsInArea"), Authorize]
        public async Task<IActionResult> GetDoctorsInArea([FromQuery]DoctorFilterInput input)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ListOutPutDoctors());
            var Email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _doctorService.GetDoctorsInArea(input, Email));
        }

        
        [HttpGet("GetDoctorsInGovernorate"), Authorize]
        public async Task<IActionResult> GetDoctorsInGovernorate([FromQuery] DoctorFilterInput input)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ListOutPutDoctors());
            var Email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _doctorService.GetDoctorsInGove(input, Email));
        }
        
        
        //Get Doctors Not Active
        [HttpGet("GetDoctorsNotActive"), Authorize(Roles = "Admin,CustomerService")]
        public async Task<IActionResult> GetDoctorsNotActive(uint index=0)
            =>Ok(await _doctorService.GetDoctorsNotActiveAsync((int)index));
        
        
        //BookedAppointments
        [HttpGet("BookedAppointments"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctoeBookedAppointments()
        => Ok(await _patientDatesService.GetDoctorDatesAsync(null,User.FindFirstValue(ClaimTypes.Email)));
        
        
        //Find Doctor
        [HttpGet("GetDoctor")]
        public async Task<IActionResult> GetDoctor(Guid Id)
            => Ok(await _doctorService.GetDoctorAsync(Id));
        //Get CountOfPatientsWithDoctor
        [HttpGet("CountOfPatientsWithDoctor"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetCountOfPatientsWithDoctor()
            => Ok(await _doctorService.PatientCountAsync(User.FindFirstValue(ClaimTypes.Email)));
        //Get CountOfPatientsWithDoctor
        [HttpGet("PatientPercentage"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetPatientPercentage()
            => Ok(await _doctorService.PatientPercentageAsync(User.FindFirstValue(ClaimTypes.Email)));
        [HttpGet("DoctorDates"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorDates()
        => Ok(await _doctorService.GetDoctorDatesAsync(User.FindFirstValue(ClaimTypes.Email)));
        //Add Doctor
        [HttpPost("AddDoctor"),Authorize]
        public async Task<IActionResult> AddDoctor(List<IFormFile> Certificates, IFormCollection input)
        {
            try
            {
                InputDoctor inputDoctor = JsonConvert.DeserializeObject<InputDoctor>(input["input"]);
                var Email = User.FindFirstValue(ClaimTypes.Email);
                return await AddDoctorAsync(Certificates, inputDoctor, Email);
            }
            catch
            {
                return BadRequest(new InputDoctor() { error = true, message = "No Data Send" });
            }
        }
        private async Task<IActionResult> AddDoctorAsync(List<IFormFile> Certificates,InputDoctor input,string Email)
        {
            if(!ModelState.IsValid)
            {
                string Error = string.Empty;
                foreach (var error in ModelState.Values.SelectMany(x => x.Errors))
                    Error = $"{Error} \n{error.ErrorMessage}";
                return Ok(
                    new InputDoctor() { error = true, message = Error }
                    );
            }
            return Ok(await _doctorService.AddDoctorAsync(input,Email,Certificates));
        }
        
        
        //AppointmentBook
        [HttpPost("AddAppointmentBook"),Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AddAppointmentBook(List<DoctorDate> dates)
        {
            if (!ModelState.IsValid)
                return Ok("Chaeck Dates");
            var email = User.FindFirstValue(claimType: ClaimTypes.Email);
            return Ok(await _doctorService.AddAppointmentBookAsync(dates, email));
        }


        
        
        [HttpPut("EditAppointmentBook"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> EditAppointmentBook(DoctorDate date,string OldDayName)
        {
            if(!ModelState.IsValid)
            {
                string Error = string.Empty;
                foreach (var e in ModelState.Values.SelectMany(e => e.Errors))
                    Error = $"{Error}{e.ErrorMessage}\n";
                return Ok(Error);
            }
            return Ok(await _doctorService.EditAppointmentBookAsync(date,OldDayName,User.FindFirstValue(ClaimTypes.Email)));
        }
        
        
        //Active Doctor
        [HttpPut("ActionDoctor"), Authorize(Roles = "Admin,CustomerService")]
        public async Task<IActionResult> ActionDoctor(Guid Id, bool action)
            => Ok(await _doctorService.ActionDoctorAsync(Id,action));

        [HttpPut("ChangePrice"), Authorize("Doctor")]
        public async Task<IActionResult> ChangePrice([Required, FromQuery] decimal NewPrice)
            => Ok(await _doctorService.ChangePriceSession(User.FindFirstValue(ClaimTypes.Email), NewPrice));
        [HttpDelete("DelteAppointmentBook"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DelteAppointmentBook(string OldDayName)
        {
            if (OldDayName == string.Empty)
            {
                return Ok("Select Day");
            }
            return Ok(await _doctorService.ReoveAppointmentBookAsync(OldDayName, User.FindFirstValue(ClaimTypes.Email)));
        }

    }
}
