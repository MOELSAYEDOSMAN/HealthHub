using HealthHup.API.Service.AccountService;

namespace HealthHup.API.Service.ModelService.HospitalService.DoctorDates
{
    public class DoctorDateService:IDoctorDateService
    {
        private readonly IAuthService _AuthService;
        private readonly IDoctorService _doctorService; 
        private readonly IPatientDatesService _petientDatesService;
        public DoctorDateService(IAuthService AuthService,
            IDoctorService DoctorService, IPatientDatesService petientDatesService)
        {
            _AuthService = AuthService;
            _doctorService = DoctorService;
            _petientDatesService = petientDatesService;
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
            return await _petientDatesService.UpdateDateAsync(input, date.Id, Users._Patient.Email);            
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
            return await _petientDatesService.PushDateAsync(input, Users._Doctor.Id, Users._Patient.Email);
        }
        private async Task<(Doctor _Doctor,ApplicationUser _Patient)> GetDoctorAndPatient(string DoctorEmail, string PatientEmail)
        {
            var drUser =await _AuthService.GetUserAsync(DoctorEmail);
            var dr = await _doctorService.findAsNotTrakingync(d => d.doctorId == drUser.Id);
            var pt = await _AuthService.GetUserAsync(PatientEmail);
            return (dr, pt);
        }
    }
}
