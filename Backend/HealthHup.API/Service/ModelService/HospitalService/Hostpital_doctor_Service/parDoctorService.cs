using Microsoft.EntityFrameworkCore;

namespace HealthHup.API.Service.ModelService.HospitalService.Hostpital_doctor_Service
{
    public partial class DoctorService
    {
        public async Task<ListOutPutDoctors> SerchDoctorWithName(string Name, DoctorFilterInput input,string email)
        {
            var govId = input?.goveId ?? await GetGovermetPaient(email);
            var Doctors = await _db.Governorates.Where(g => g.Id == govId)
                .Include(g => g.areas).ThenInclude(a => a.doctors).ThenInclude(d => d.doctor)
                .SelectMany(a => a.areas.SelectMany(d => d.doctors))
                .Where(d => d.drSpecialtieId == input.Specialtie && d.doctor.Name.Contains(Name))
                .ToListAsync();
            return new()
            {
                count = 0,
                Doctors = ODoctor.Doctors(Doctors).ToList()                
            };
        }
        
        private async Task<Guid> GetAreaIdPaientAsync(string email)
        {
            ApplicationUser paient;
            paient=await _AuthService.GetUserAsync(email);
            return paient.AreaId;
        }

        private async Task<Guid> GetGovermetPaient(string email)
        {
            Guid areaid=await GetAreaIdPaientAsync(email);
            var gove = (await _areaService.findAsNotTrakingync(a=>a.Id==areaid)).governorateId;
            return gove;
        }
    }
}
