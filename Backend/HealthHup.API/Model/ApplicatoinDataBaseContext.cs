using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthHup.API.Model
{
    public class ApplicatoinDataBaseContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicatoinDataBaseContext(DbContextOptions<ApplicatoinDataBaseContext> options) : base(options)
        {
        }
        //Adress
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Area> Areas { get; set; }

        //Drugs
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<ActivePharmaceutical> ActivePharmaceuticals { get; set; }

        //Pharmacy
        public DbSet<Pharmacy> pharmacies { get; set; }
        public DbSet<PharmacyDrug> pharmacyDrugs { get; set; }

        //Hospital
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialtie> Specialties { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}