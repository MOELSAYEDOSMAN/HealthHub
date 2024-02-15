﻿using HealthHup.API.Model.Models.Hospital;
using HealthHup.API.Service.AccountService;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HealthHup.API.Service.ModelService.HospitalService.Hostpital_doctor_Service
{
    public class DoctorService:BaseService<Doctor>, IDoctorService
    {
        private readonly IAuthService _AuthService;
        private readonly ISaveImage _SaveImg;
        private readonly IBaseService<Specialtie> _SpecialtieService;
        private readonly IAreaService _areaService;
        private readonly IBaseService<Governorate> _governorate;
        public DoctorService(ApplicatoinDataBaseContext db,IAuthService authService, ISaveImage saveImg, IBaseService<Specialtie> specialtieService, IAreaService areaService,IBaseService<Governorate> gove) : base(db)
        {
            _AuthService = authService;
            _SaveImg = saveImg;
            _SpecialtieService = specialtieService;
            _areaService = areaService;
            _governorate = gove;
        }

        //Doctors Not In Active
        public async Task<ListOutPutDoctors> GetDoctorsNotActiveAsync(int index)
        {
            var listDoctors = await findByAsync(
                condation: (d => !d.Accept),
                inculde:new string[] { "drSpecialtie", "doctor", "area", "Certificates" },
                OrderBy:x=>x.DateOfSendRequest
                );
            int count = listDoctors.Count;
            listDoctors= listDoctors.Skip(index * 10).Take(10).ToList();
            var listODoctors = new ListOutPutDoctors()
            {
                count = count % 10 == 0 ? count / 10 : ((count /10) + 1),
                Doctors = new List<ODoctor>()
            };
            listDoctors.ToList().ForEach(d => listODoctors.Doctors.Add(d));
            return listODoctors;
        }
       //Add Doctor From Normal User
        public async Task<InputDoctor> AddDoctorAsync(InputDoctor input,string Email ,List<IFormFile> Certificates)
        {
            //Cheack User
            var User = await _AuthService.GetUserAsync(Email);
            if (User == null)
                return new() { error = true, message = "Agin login" };
            //Cheack IF Have Old Request 
            var doc =await findAsync(x => x.doctor.Id == User.Id, new string[] { "doctor" });
            if (doc != null)
                return new() {error=true,message="You Have Old Request" };
            //Cheack If Certificates Upload
            if (Certificates?.Count == 0)
                return new(){ error = true, message = "Must Upload Certificates" };
            //Cheack If Select Area 
            var areaClic = await _areaService.GetAsync(input.areaClinicId,null);
            if (areaClic == null)
                return new() { error = true, message = "Must Select Area" };
            //Cheack If Select specialtie
            var specialtie = await _SpecialtieService.GetAsync(input.specialtieId,null);
            if(specialtie == null)
                return new() { error = true, message = "Must Select specialtie" };
            //Save Certificates
            List<string> ls = await _SaveImg.UploadImagesList("DoctorCertificates", Certificates);
            
            //Add Doctor
            Doctor NewDoctor = input;//Covert InputDoctor To Doctor
            //Add Certificates
            ls.ForEach(c =>
            {
                NewDoctor.Certificates.Add(
                    new()
                    {
                        src = c
                    });
            });
            NewDoctor.doctor = User;//Add User
            NewDoctor.area = areaClic;//Add Area
            NewDoctor.drSpecialtie = specialtie;//Add Specialtie
            await AddAsync(NewDoctor);
            return new() { error=false,message="You Application Will Be Reviewed"};
        }
        //Get Doctor 
        public async Task<ODoctor>? GetDoctorAsync(Guid Id)
        {
            var doctor = await findAsync( d=>d.Id==Id, new string[] { "drSpecialtie", "Dates", "doctor", "area", "Certificates" });
            if (doctor == null)
                return null;
            return doctor;
        }
        //Action Doctor To Active
        #region ActionWithDoctor
        public async Task<bool> ActionDoctorAsync(Guid Id,bool Accespt)
        =>Accespt?await AccseptDoctorAsync(Id):await RefusalDoctorAsync(Id);
        private async Task<bool> AccseptDoctorAsync(Guid Id)
        {
            var doctor = await findAsync(x=>x.Id==Id,new string[] {"doctor"});
            if (doctor == null)
                return false;
            doctor.Accept = true;
            doctor.DateOfJoin = DateTime.Now;
            //Add Role
            if(doctor.doctor!=null)
                await _AuthService.AddRoleAsync(doctor?.doctor?.Email, "Doctor");
            return await UpdateAsync(doctor);
        }
        private async Task<bool> RefusalDoctorAsync(Guid Id)
        {
            var doctor = await GetAsync(Id, new string[] { "Certificates" });
            if (doctor == null)
                return false;
            //Delete Certificates
            var Certifcates = doctor.Certificates.Select(x => $"DoctorCertificates/{x.src}").ToList();
            await _SaveImg.DeletsImages(Certifcates);//Delete 
            return await RemoveAsync(doctor);
        }
        #endregion
        //Get Doctors In Area
        public async Task<ListOutPutDoctors> GetDoctorsInArea(DoctorFilterInput input, string Email)
        {
            //Set Area
            input.area=input?.area??(await _AuthService.GetUserAsync(Email)).AreaId;
            //Get Doctors
            var Doctors =await findByAsync(d => d.areaId==input.area&&d.drSpecialtieId == input.Specialtie&&d.Accept==true,
                new string[] { "doctor"});
            Doctors = input?.joinDate??false ? Doctors.OrderBy(x => x.DateOfJoin).ToList() : Doctors;
            int count = Doctors?.Count??0;
           //Create Object
            var result=new ListOutPutDoctors()
            {
                count=count%10==0?count%10:(count/10)+1
            };
            //Give Value
            Doctors?.Skip(10*(int)input?.Index).Take(10).ToList()
                .ForEach(d=>result.Doctors.Add(d));
            return result;
        }
       //Get Doctors In Gove
        public async Task<ListOutPutDoctors> GetDoctorsInGove(DoctorFilterInput input, string Email)
        {
            //Chrack Area
            input.area = input?.goveId==null? input?.area:(await _AuthService.GetUserAsync(Email)).AreaId;
            //Get Gove
            var gove = input?.goveId != null ? 
                input?.goveId:
                (await _areaService.findAsync(a=>a.Id==input.area)).governorateId;
            //Get Doctors
            var Doctors=gove==null?
                new List<Doctor>():
                await _db.Governorates
                .Include(g => g.areas).ThenInclude(a => a.doctors).ThenInclude(d => d.doctor)
                 .Where(g=>g.Id==gove)
                 .SelectMany(g=>g.areas.
                 SelectMany(a=>a.doctors.Where(d=>d.drSpecialtieId==input.Specialtie&&d.Accept).ToList()))
                 .ToListAsync();
            Doctors = input?.joinDate ?? false ? Doctors.OrderBy(x => x.DateOfJoin).ToList() : Doctors;
            //Get Count
            int count = Doctors.Count;
            //Create Object
            var result = new ListOutPutDoctors()
            {
                count = count % 10 == 0 ? count % 10 : (count / 10) + 1
            };
            //Give 10 And Filter
            Doctors.Skip(10 * (int)input.Index).Take(10).ToList()
                .ForEach(d => result.Doctors.Add(d));

            return result;
        }
       
        
        # region AppointmentBook
        //Add Date To Doctors
        public async Task<string> AddAppointmentBookAsync(List<DoctorDate>Dates,string email)
        {
            var User=await _AuthService.GetUserAsync(email);
            var doc =await findAsync(x => x.doctor.Id == User.Id, new string[] { "doctor", "Dates" }); 
            foreach (var d1 in Dates)
            {
                if (Dates.Count(x => x.DayName == d1.DayName) != 1)
                    return $"{d1.DayName}  Repeater";
                if (!CompareTime(d1.From, d1.To))
                    return $"Cheack Time in Day Name={d1.DayName}";
                if (doc.Dates.Count(x=>x.DayName== d1.DayName)!=0)
                    return $"{d1.DayName} Perviously Registered";
            }
            doc.Dates.AddRange(Dates);
            await SaveChaneAsync();
            return "Save Dates";
        }
        //Edit
        public async Task<string> EditAppointmentBookAsync(DoctorDate Dates,string DayNameOld ,string email)
        {
            var UserId=await _AuthService.GetUserAsync(email);
            var doctor = await findAsync(x => x.doctorId == UserId.Id, new string[] { "Dates" });
            if (doctor.Dates.Count(x => x.DayName.ToUpper() == Dates.DayName.ToUpper()) != 0)
                return $"{Dates.DayName} Perviously Registered";
            var date = doctor.Dates.SingleOrDefault(x => x.DayName.ToUpper() == DayNameOld.ToUpper());
            if (date == null)
                return $"No Data This Name:{DayNameOld}";
            if (!CompareTime(Dates.From, Dates.To))
                return "Chrack Tiem";
            doctor.Dates.Remove(date);
            doctor.Dates.Add(Dates);
            await SaveChaneAsync();
            return "Save Change";
        }
        //Delte
        public async Task<string> ReoveAppointmentBookAsync(string DayNameDel, string email)
        {
            var UserId = await _AuthService.GetUserAsync(email);
            var doctor = await findAsync(x => x.doctorId == UserId.Id, new string[] { "Dates" });
            var date=doctor.Dates.FirstOrDefault(x => x.DayName.ToUpper() == DayNameDel.ToUpper());
            if (date == null)
                return $"No Date With This Name {DayNameDel}";
            doctor.Dates.Remove(date);
            await SaveChaneAsync();
            return "Save Change";
        }
        private bool CompareTime(string dateInputFrom,string dateInputTo)
        {
            int PlusFrom = 0; int PlusTo = 0;

            if (dateInputTo.EndsWith("AM") && dateInputFrom.EndsWith("BM"))
            {
                PlusFrom = 0;
                PlusTo = 12;
            }
            else
            {
                PlusFrom = dateInputFrom.EndsWith("AM") ? 0 : 12;
                PlusTo = dateInputTo.EndsWith("AM") ? 0 : 12;
            }
            //Get Hours
            int dateFromH=int.Parse(dateInputFrom.Substring(0,2))+PlusFrom;
            int dateToH = int.Parse(dateInputTo.Substring(0, 2)) + PlusTo;
            //Get M
            int dateFromM = int.Parse(dateInputFrom.Substring(3, 2));
            int dateToM = int.Parse(dateInputTo.Substring(3, 2));
            return dateToH > dateFromH?true:(dateToH == dateFromH)? (dateToM > dateFromM):false;
        }
        

        

        #endregion

    }
}