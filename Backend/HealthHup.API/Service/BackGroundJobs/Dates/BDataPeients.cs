﻿
using HealthHup.API.Service.AccountService;
using HealthHup.API.Service.ModelService.Admin.Alerts;
using HealthHup.API.Service.Notification;

namespace HealthHup.API.Service.BackGroundJobs.Dates
{
    public class BDataPeients : IBDataPeients
    {
        private readonly IBaseService<Disease> _Disease;
        private readonly IMedicalSessionService _MedicalSessionService;
        private readonly IPatientDatesService _PatientDatesService;
        private readonly IAlertService _alertService;
        private readonly IDoctorService _doctorService;
        private readonly INotifiyService _notifiyService;
        public BDataPeients(IBaseService<Disease> DiseaseService,IMedicalSessionService MedicalSessionService,
            IPatientDatesService PatientDatesService,
            IAlertService alertService, IDoctorService doctorService
            , INotifiyService notifiyService
            )
        {
            _Disease = DiseaseService;
            _MedicalSessionService = MedicalSessionService;
            _PatientDatesService= PatientDatesService;
            _alertService = alertService;
            _doctorService = doctorService;
            _notifiyService=notifiyService;
        }
        public async Task DeleteOldDates()
        {
            var Dates = await _PatientDatesService.findByAsync(d => d.date < DateTime.UtcNow.Date, inculde: new string[] { "patient", "doctor" });
            foreach(var i in Dates)
            {
                var Doctor = await _doctorService.GetDoctorMainModel(i.doctorId);
                await _alertService.AddAlertPatientCancelDates(i?.patient,i.date,Doctor?.Name);
            }
            if(Dates.Count>0)
                await _PatientDatesService.RemoveRangeAsync(Dates.ToList());

        }

        public async Task DeleteOldDisease()
        {
            var Diseases = await _Disease.findByAsync(d => d.Cured == true);
            
            
            if (Diseases.Count > 0)
               await _Disease.RemoveRangeAsync(Diseases.ToList());

            foreach(var i in Diseases)
            {
                var medicals = await _MedicalSessionService.GetMedicalSessionsWithDiseaseCuredAsync(i.PatientId,i.Name);
                await _MedicalSessionService.RemoveRangeAsync(medicals.ToList());
            }
        }

        public async Task AlertDate()
        {
            var dates = await _PatientDatesService.findByAsync(d => d.date.Date == DateTime.Now.Date, new string[] { "patient" });
            foreach (var i in dates)
                await _notifiyService
                    .AlertDate(i.patient,$"Don't Forget Your appintment Today at {i.FromTime}");
        }

        
    }
}
