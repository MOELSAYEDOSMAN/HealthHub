namespace HealthHup.API.Model.Extion.Account
{
    public class DTOUserInformation
    {
        public string name { get; set; }=string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string gove { get; set; } = string.Empty;
        public string area { get; set; } = string.Empty;
        public string gender { get; set; } = string.Empty;
        public string img { get; set; } = string.Empty;
        public int age { get; set; } = -1;
        public DateOnly birDay { get; set; }


        public static implicit operator DTOUserInformation(ApplicationUser input)
            => new DTOUserInformation()
            {
                name = input?.Name,
                email=input?.Email,
                phone=input?.PhoneNumber,
                img=input?.src,
                gender=(input?.Gender??false)?"Male":"Female",
                age=input?.Age??-1,
                birDay=DateOnly.FromDateTime(input?.Brdate??DateTime.MinValue),
            };

        public static IEnumerable<DTOUserInformation> ConvertFromListOFUser(List<ApplicationUser> input)
        {
            foreach (var i in input)
                yield return i;
        }
    }
}
