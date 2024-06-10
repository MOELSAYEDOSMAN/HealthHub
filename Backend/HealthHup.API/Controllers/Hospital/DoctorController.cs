using HealthHup.API.Model.Extion.Account;
using HealthHup.API.Service.AccountService;
using HealthHup.API.Service.ModelService.HospitalService.DoctorDates;
using HealthHup.API.Service.Notification;
using HealthHup.API.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HealthHup.API.Controllers.Hospital
{
    [Route("Hospital/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IDoctorService _doctorService;
        private readonly IDoctorDateService _doctorDateService;
        private readonly IPatientDatesService _patientDatesService;
        private readonly ISendMessageService _MessageService;
        private readonly IAdminLogs _adminLgos;
        private readonly INotifiyService _notifyService;
        public DoctorController(IDoctorService doctorService, IPatientDatesService patientDatesService,
            IAuthService authService, IDoctorDateService doctorDateService,IAdminLogs adminLog, ISendMessageService _MessageService1
            ,INotifiyService notifyService)
            
        {
            _doctorService = doctorService;
            _patientDatesService = patientDatesService;
            _authService = authService;
            _doctorDateService= doctorDateService;
            _adminLgos=adminLog;
            _MessageService= _MessageService1;
            _notifyService = notifyService;
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
            return Ok(ChangeImageDoctors(await _doctorService.GetDoctorsInArea(input, Email)));
        }


        [HttpGet("GetDoctorsInGovernorate"), Authorize]
        public async Task<IActionResult> GetDoctorsInGovernorate([FromQuery] DoctorFilterInput input)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ListOutPutDoctors());
            var Email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(ChangeImageDoctors(await _doctorService.GetDoctorsInGove(input, Email)));
        }
       
        [HttpGet("SerchDoctorsWithName"), Authorize]
        public async Task<IActionResult> SerchDoctorsWithName([SerchValidation,MinLength(3)] string DoctorName, [FromQuery] DoctorFilterInput input)
            => Ok(ChangeImageDoctors(await _doctorService.SerchDoctorWithName(DoctorName,input, User.FindFirstValue(ClaimTypes.Email))));

        //Get Doctors Not Active
        [HttpGet("GetDoctorsNotActive"), Authorize(Roles = "Admin,CustomerService")]
        public async Task<IActionResult> GetDoctorsNotActive(uint index=0)
            =>Ok(ChangeImageDoctors(await _doctorService.GetDoctorsNotActiveAsync((int)index)));

        //Get InfoDoctor
        [HttpGet("GetDoctorSpecialtie"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorSpecialtie()
            => Ok(await _doctorService.GetDoctorSpecialtie(User.FindFirstValue(ClaimTypes.Email)));
        
        //BookedAppointments
        [HttpGet("BookedAppointments"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctoeBookedAppointments()
        => Ok(await _patientDatesService.GetDoctorDatesAsync(null,User.FindFirstValue(ClaimTypes.Email)));
        
        
        //Find Doctor
        [HttpGet("GetDoctor")]
        public async Task<IActionResult> GetDoctor(Guid Id)
            => Ok(ChangeImageDoctor(await _doctorService.GetDoctorAsync(Id)));
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
            var valFile = new FileValidation();

            if (Certificates.Count>0)
                foreach(var i in Certificates)
                    if(!valFile.IsValid(i))
                        return Ok(
                    new InputDoctor() { error = true, message = valFile.ErrorMessage }
                    );
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
        {
            
            var doctore = await _doctorService.GetDoctorAsync(Id);
            if (doctore != null)
            {
                await _MessageService.ActionWithDoctor(doctore.Email, doctore.Name, action ? "Your Request has been Accepted" : "Your Request has been rejected");
                await _notifyService.DoctorAction(await _authService.GetUserAsync(doctore.Email), action ? "Your Request has been Accepted" : "Your Request has been rejected");
            }
            return Ok(await _doctorService.ActionDoctorAsync(Id, action, User.FindFirstValue(ClaimTypes.Email)));
        }

        

        [HttpPut("ChangePrice"), Authorize(Roles ="Doctor")]
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

        [HttpDelete("CancelPatientDate"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CancelPatientDate([Required, SerchValidation] string PatientEmail)
            => Ok(await _doctorDateService.CancelDate(User.FindFirstValue(ClaimTypes.Email),PatientEmail));

        [HttpPut("ChangePatientDate"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> ChangePatientDate([Required, SerchValidation] string PatientEmail,PatientDateInput input)
            => Ok(await _doctorDateService.ChangeDate(User.FindFirstValue(ClaimTypes.Email), PatientEmail,input));
        [HttpPost("PushPatientDate"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> PushPatientDate([Required, SerchValidation] string PatientEmail, PatientDateInput input)
            => Ok(await _doctorDateService.PushDate(User.FindFirstValue(ClaimTypes.Email), PatientEmail, input));



        private ODoctor? ChangeImageDoctor(ODoctor? input)
        {
            if (input?.Id == null)
                return null;

            input.DrImg = $"{this.Request.Scheme}://{this.Request.Host}/Image/User/{input.DrImg}";

            foreach(var i in input.Certificates)
                i.src= $"{this.Request.Scheme}://{this.Request.Host}/Image/DoctorCertificates/{i.src}";

            return input;
        }

        private IEnumerable<ODoctor> ChangeImageDoctors(List<ODoctor>? input)
        {
            foreach (var i in input)
               yield return ChangeImageDoctor(i);
        }
        private ListOutPutDoctors ChangeImageDoctors(ListOutPutDoctors input)
        {
            if (input.count > 0)
                input.Doctors = ChangeImageDoctors(input.Doctors).ToList();
            return input;
        }

    }
}
