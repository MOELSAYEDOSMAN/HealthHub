﻿using Microsoft.EntityFrameworkCore;

namespace HealthHup.API.Model.Models.Hospital.Patient
{
    [Owned]
    public class Repentance
    {
        public Drug drug { get; set; }
        public string? Note { get; set; }
        public string? Repeat { get; set; }
        public int RepeatCount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
