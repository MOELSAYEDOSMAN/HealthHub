using HealthHup.API.Service.AccountService;
using HealthHup.API.Service.Notification;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HealthHup.API.Service.ModelService.HospitalService.DoctorDates
{
    public class DoctorDateService:IDoctorDateService
    {
        private readonly IAuthService _AuthService;
        private readonly IDoctorService _doctorService; 
        private readonly IPatientDatesService _petientDatesService;
        private readonly ISendMessageService _SendMessage;
        private readonly INotifiyService _notifiyService;
        public DoctorDateService(IAuthService AuthService,
            IDoctorService DoctorService, IPatientDatesService petientDatesService
            , ISendMessageService SendMessage, INotifiyService notifiyService)
        {
            _AuthService = AuthService;
            _doctorService = DoctorService;
            _petientDatesService = petientDatesService;
            _SendMessage = SendMessage;
            _notifiyService = notifiyService;
        }

        public async Task<string> CancelDate(string DoctorEmail, string PatientEmail)
        {
            var Users = await GetDoctorAndPatient(DoctorEmail, PatientEmail);
            if (Users._Doctor == null)
                return "Not Role";
            
            if (Users._Patient == null)
                return "Must Select petient";

            
            var date=await _petientDatesService.findAsync(d=>d.doctorId==Users._Doctor.Id&&d.patientId==Users._Patient.Id);
            if (date == null)
                return "Appointment Not Found";
            
            if (date.date.Date < DateTime.UtcNow.Date)
                return "Appointment has Passed";
            await _petientDatesService.RemoveAsync(date);
            await _SendMessage.AlertDate(Users._Patient.Email,Users._Patient.Name,Users._Doctor.doctor.Name,DateAction.Cancel,$"Dr\\{Users._Doctor.doctor.Name} Cancel Date: {DateOnly.FromDateTime(date.date)}");
            await _notifiyService.DoctorCancelYourTime(Users._Patient, $"Dr\\{Users._Doctor.doctor.Name} Cancel Date: {DateOnly.FromDateTime(date.date)}");
            //Send Message Or Notfiy
            return "Done";
        }

        public async Task<string> ChangeDate(string DoctorEmail, string PatientEmail, PatientDateInput input)
        {
            //Cheack Date
            if (input.day.Date < DateTime.UtcNow.Date)
                return "Choose a Future Date";
            //Get Users
            var Users = await GetDoctorAndPatient(DoctorEmail, PatientEmail);
            if (Users._Doctor == null)
                return "Not Role";

            if (Users._Patient == null)
                return "Must Select Paient";
            var date = await _petientDatesService.findAsNotTrakingync(d => d.doctorId == Users._Doctor.Id && d.patientId == Users._Patient.Id&&d.date.Date>=DateTime.UtcNow.Date);
            var OldDate = new PatientDates()
            {
                date=date.date,FromTime=date.FromTime
            };
            var result= await _petientDatesService.UpdateDateAsync(input, date.Id, Users._Patient.Email);
            if (result == "Save Change")
            {
                await _SendMessage.AlertDate(Users._Patient.Email, Users._Patient.Name, Users._Doctor.doctor.Name, DateAction.Change, $"Dr\\{Users._Doctor.doctor.Name} Change Date\n From=>Date:{DateOnly.FromDateTime(OldDate.date)} Time:{OldDate.FromTime} \n  To=>Date:{DateOnly.FromDateTime(input.day.Date)} Time:{input.From}");
                await _notifiyService.DoctorChangeYourTime(Users._Patient, $"Dr\\{Users._Doctor.doctor.Name} Change Date\n From=>Date:{DateOnly.FromDateTime(OldDate.date)} Time:{OldDate.FromTime} \n  To=>Date:{DateOnly.FromDateTime(input.day.Date)} Time:{input.From}");
            }
            return result;
        }

        public async Task<string> PushDate(string DoctorEmail, string PatientEmail, PatientDateInput input)
        {
            //Cheack Date
            if (input.day.Date < DateTime.UtcNow.Date)
                return "Choose a Future Date";
            //Get Users
            var Users = await GetDoctorAndPatient(DoctorEmail, PatientEmail);
            if (Users._Doctor == null)
                return "Not Role";

            if (Users._Patient == null)
                return "Must Select Paient";
            var result= await _petientDatesService.PushDateAsync(input, Users._Doctor.Id, Users._Patient.Email);
            if (result == "Save.")
            {
                await _SendMessage.AlertDate(Users._Patient.Email, Users._Patient.Name, Users._Doctor.doctor.Name, DateAction.Add, $"Dr\\{Users._Doctor.doctor.Name} select Date\n  To=>Date:{DateOnly.FromDateTime(input.day.Date)} Time:{input.From}");
                await _notifiyService.DoctorSelectDate(Users._Patient, $"Dr\\{Users._Doctor.doctor.Name} select Date\n  To=>Date:{DateOnly.FromDateTime(input.day.Date)} Time:{input.From}");
            }
            return result;
        }
        private async Task<(Doctor _Doctor,ApplicationUser _Patient)> GetDoctorAndPatient(string DoctorEmail, string PatientEmail)
        {
            var drUser =await _AuthService.GetUserAsync(DoctorEmail);
            var dr = await _doctorService.findAsNotTrakingync(d => d.doctorId == drUser.Id,new string[] { "doctor" });
            var pt = await _AuthService.GetUserAsync(PatientEmail);
            return (dr, pt);
        }
    }
}
