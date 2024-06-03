namespace HealthHup.API.Model.Extion.Hospital.PatientDTO
{
    public class RepentanceDTO
    {
        public string drugName { get; set; }
        public int repeatCount { get; set; }
        public string Note { get; set; }
        public string repeat { get; set; }
        public DateOnly startdate { get; set; }
        public DateOnly enddate { get; set; }


        public static implicit operator RepentanceDTO(Repentance input)
            => new RepentanceDTO()
            {
                drugName = input.drug.name,
                enddate = DateOnly.FromDateTime(input?.EndDate ?? DateTime.MaxValue),
                startdate = DateOnly.FromDateTime(input?.StartDate ?? DateTime.MaxValue),
                Note = input?.Note,
                repeat = input.Repeat,
                repeatCount = input.RepeatCount
            };

        public static IEnumerable<RepentanceDTO>? ConvertFromListOfRepentance(IList<Repentance> input)
        {
            foreach (var i in input)
                yield return i;
        }
    }
}
