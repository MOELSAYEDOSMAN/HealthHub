﻿namespace HealthHup.API.Model.Models.Hospital.Patient
{
    public class MedicalSession
    {
        public Guid Id { get; set; }
        public DateTime date { get; set; }
        public string? Notes { get; set; }
        public string DiseaseName { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
        public List<Repentance> repentances { get; set; }
    }
}