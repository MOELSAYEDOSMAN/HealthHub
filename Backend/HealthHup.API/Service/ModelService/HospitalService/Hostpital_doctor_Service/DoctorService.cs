using HealthHup.API.Model.Models.Hospital;
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
        public DoctorService(ApplicatoinDataBaseContext db,IAuthService authService, ISaveImage saveImg, IBaseService<Specialtie> specialtieService, IAreaService areaService) : base(db)
        {
            _AuthService = authService;
            _SaveImg = saveImg;
            _SpecialtieService = specialtieService;
            _areaService = areaService;
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
            var doctor = await findAsync( d=>d.Id==Id, new string[] { "drSpecialtie", "doctor", "area", "Certificates" });
            if (doctor == null)
                return null;
            return doctor;
        }
        //Action Doctor To Active
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
        //Get Doctors In Area
        public async Task<ListOutPutDoctors> GetDoctorsInArea(DoctorFilterInput input, string Email)
        {
            input.area=(await _AuthService.GetUserAsync(Email)).AreaId;
            var Doctors =await findByAsync(d => d.areaId==input.area&&d.drSpecialtieId == input.Specialtie&&d.Accept==true,
                null);
            int count = Doctors?.Count??0;
            var result=new ListOutPutDoctors()
            {
                count=count%10==0?count%10:(count/10)+1,
                Doctors=new List<ODoctor>()
            };
            Doctors.Skip(10*(int)input?.Index).Take(10).ToList()
                .ForEach(d=>result.Doctors.Add(d));
            return result;
        }
       //Get Doctors In Gove
        public async Task<ListOutPutDoctors> GetDoctorsInGove(DoctorFilterInput input, string Email)
        {
            input.area = (await _AuthService.GetUserAsync(Email)).AreaId;
            var gove= (await _db.Areas.Include(a => a.governorate).SingleOrDefaultAsync(x => x.Id == input.area))
                .governorate;
           var Doctors= await _db.Governorates
                 .Include(g => g.areas).ThenInclude(a => a.doctors).ThenInclude(d => d.doctor)
                 .Include(g => g.areas).ThenInclude(a => a.doctors).ThenInclude(d => d.drSpecialtie)
                 .Where(g=>g.Id==gove.Id)
                 .SelectMany(g=>g.areas.SelectMany(a=>a.doctors)).ToListAsync();
            int count = Doctors.Count;
            var result = new ListOutPutDoctors()
            {
                count = count % 10 == 0 ? count % 10 : (count / 10) + 1,
                Doctors = new List<ODoctor>()
            };
            Doctors.Where(d => d.drSpecialtie.Id == input.Specialtie&&d.Accept)
                .Skip(10 * (int)input.Index).Take(10).ToList()
                .ForEach(d => result.Doctors.Add(d));
            return result;
        }
    }
}
