using System.ComponentModel.DataAnnotations;

namespace HealthHup.API.Validation
{
    public class DoctorDateValidation : ValidationAttribute
    {
        
        public override bool IsValid(object? value)
        {
            //"10:30 AM"
            if (value is string date)
            {
                return DateHaveAMorBM(date) && DateSq(date) && DateStartFrom1To12(date) && DateStartFrom0To59(date);
            }
            return false;
        }
        private static bool DateHaveAMorBM(string date)
            => (date.ToUpper().Contains("AM") || date.ToUpper().Contains("PM"));
        private static bool DateStartFrom1To12(string date)
        {
            int Number = -1;
            return int.TryParse(date.Substring(0, 2), out Number) || (Number <= 12 && Number >= 1);
        }
        private static bool DateStartFrom0To59(string date)
        {
            int Number = -1;
            return int.TryParse(date.Substring(3, 2), out Number) || (Number <= 59 && Number >= 0);
        }
        private static bool DateSq(string date)
        => date[2] == ':';
    }
}
