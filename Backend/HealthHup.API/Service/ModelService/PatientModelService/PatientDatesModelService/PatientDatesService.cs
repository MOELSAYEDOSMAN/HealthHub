using HealthHup.API.Model.Models.Hospital;
using HealthHup.API.Service.AccountService;
using Microsoft.EntityFrameworkCore;

namespace HealthHup.API.Service.ModelService.PatientModelService.PatientDatesModelService
{
    public class PatientDatesService:BaseService<PatientDates>, IPatientDatesService
    {
        readonly IAuthService _authService;
        readonly IDoctorService _doctorService;
        readonly IBaseService<DoctorDate> _doctordate;
        public PatientDatesService(ApplicatoinDataBaseContext db,IAuthService authService ,IDoctorService doctorService, IBaseService<DoctorDate> doctordate) : base(db)
        {
            _authService = authService;
            _doctorService = doctorService;
            _doctordate = doctordate;
        }

        //Get
        public async Task<List<PatientDateDTO>> GetDoctorDatesAsync(Guid? Id ,string? email)
        {
            IList<PatientDates> dates;
            if (Id != null)
                dates = await findByAsync(d => d.doctorId == Id, new string[] { "patient" });
            else if (email != string.Empty)
            {
                var drId = await _authService.GetUserAsync(email);
                dates = (await _doctorService.findAsync(d => d.doctorId == drId.Id, new string[] { "patientDates" }))
                    .patientDates;
            }
            else
                return new List<PatientDateDTO>();
            var DatesResult=new List<PatientDateDTO>();
            foreach(var d in dates)
            {
                PatientDateDTO v = d;
                v.dayName=d.date.DayOfWeek.ToString();
                v.to = RepireTime(GetTimeTo(d.FromTime));
                DatesResult.Add(v);
            }
            return DatesResult;
        }
        //Post
        public async Task<string> PushDateAsync(PatientDateInput input,Guid DrId,string Email)
        {
            //Get User
            var usr = await _authService.GetUserAsync(Email);
            if (usr == null)
                return "Must Login";
            string resultCheacks = await CheackDoctorTimes(input, DrId, usr.Id,true);
            if (resultCheacks != string.Empty)
                return resultCheacks;
            //Save In db
            await AddAsync(new PatientDates()
            {
                date = input.day,
                doctorId = DrId,
                patient = usr,
                Id = Guid.NewGuid(),
                FromTime = input.From,
            });
            return "Save.";
        }
        //Delete
        public async Task<bool> CancleDateAsync(Guid PaintDateid,string Email)
        {
            //Get User
            var usr=await _authService.GetUserAsync(Email);
            if(usr == null) return false;

            //Get Date
            var date = await findAsync(d => d.Id == PaintDateid && d.patientId == usr.Id);
            if (date == null) return false;
            await RemoveAsync(date);
            return true;
        }
                //**With HangeFire**
        public async Task RemoveOldDateAsync()
        {
            var OldDates = await findByAsync(d=>!CompareDate(d.date));
            OldDates.ToList().ForEach(async d => await RemoveAsync(d));
        }
        //put
        public async Task<string> UpdateDateAsync(PatientDateInput input, Guid DateId, string Email)
        {
            //Cheack User
            var user=await _authService.GetUserAsync(Email);
            if (user == null)
                return "Must Login";
            //Cheack If Date In Db
            var oldDateUser = await findAsync(d => d.patientId == user.Id && d.Id == DateId);
            if (oldDateUser == null)
                return "Must Select Date";
            //Cheack Date Avaible
            string CheackDoctorDates = await CheackDoctorTimes(input, oldDateUser.doctorId, user.Id, false);
            if(CheackDoctorDates != string.Empty)
                return CheackDoctorDates;

            //Update Db
            oldDateUser.FromTime = input.From;
            oldDateUser.date = input.day;
            await UpdateAsync(oldDateUser);
            return "Save Change";

        }









        //Private Functions
        private async Task<string> CheackDoctorTimes(PatientDateInput input,Guid DrId, string UserId,bool newPush)
        {
            //cheack Doctor times 
            //get Dr Dates
            var drDates = await _doctorService.findAsync(d => d.Id == DrId, new string[] { "Dates" });
            if (drDates == null)
                return "Must Select Doctor";
            //Cheack date
            var dayUserSelect = drDates.Dates.FirstOrDefault(d => d.DayName == input.day.DayOfWeek.ToString());
            if (dayUserSelect == null)
                return "Choose The Appropriate Day";
            //Cheack Time
            if (!CompareTime(input.From, dayUserSelect.From, dayUserSelect.To) || !CompareDate(input.day))
                return "Choose The Appropriate Time";

            //If User has date prev
            if(newPush)
            {
                var UserOldDates = await findByAsync(d => d.patientId == UserId && d.doctorId == DrId);
                if (UserOldDates?.Count >0)
                    return "You aleady Have An Appointment comping up";
            }


            //Check other Patient Select Date
            var oldDates = await findByAsync(d => d.date == input.day && d.FromTime.Substring(0,2) == input.From.Substring(0, 2));
            if (oldDates?.Count > 0)
                return "this date was Previously Selected";
            return string.Empty;
        }

        //Date|Time
        //Covert Time From AM To BM & ++
        string GetTimeTo(string time)
        {
            int timeValue = int.Parse(time.Substring(0, 2));
            if (time.ToUpper().Contains("AM"))
                return timeValue == 12 ? $"01:{int.Parse(time.Substring(3, 2))} BM"
                    : $"{++timeValue}:{int.Parse(time.Substring(3, 2))} AM";
            else
                return timeValue == 12 ? $"01:{int.Parse(time.Substring(3, 2))} AM"
                    : $"{++timeValue}:{int.Parse(time.Substring(3, 2))} BM";
        }

        //Compare Time
        bool CompareTime(string timeFrom,string DrTimeFrom, string DrTimeTo)
        {
            double UserTimeSelectFrom = ReturnTimeValue(timeFrom);
            double UserTimeSelectTo = ReturnTimeValue(RepireTime(GetTimeTo(timeFrom)));
            double DrTimeFr = ReturnTimeValue(DrTimeFrom);
            double DrTimeto = ReturnTimeValue(DrTimeTo);
            if (UserTimeSelectFrom >= DrTimeFr && UserTimeSelectTo <= DrTimeto)
                return true;
            else
                return false;
        }
        string RepireTime(string time)
        { 
            if (time.Length == 8)
                return time;
            int output;
            if (!int.TryParse(time.Substring(0, 2), out output))
                time = $"0{time}";
            output = -1;
            if (!int.TryParse(time.Substring(3, 2), out output)||output==0)
                time = $"{time.Substring(0, 3)}0{time.Substring(3)}";
            return time;
        }
        double ReturnTimeValue(string time)
        {
            double UserTime = double.Parse($"{time.Substring(0, 2)}.{time.Substring(3, 2)}");
            UserTime = time.ToUpper().Contains("AM") ? UserTime : UserTime + 12;
            return UserTime;
        }
        //Compare Date
        bool CompareDate(DateTime date1)
            => DateOnly.FromDateTime(date1) >= DateOnly.FromDateTime(DateTime.UtcNow);
        bool CompareDate(DateTime date1,DateTime date2)
            => DateOnly.FromDateTime(date1) > DateOnly.FromDateTime(date2);
    }
}
