using HealthHup.API.Model.Models.Notifaction;

namespace HealthHup.API.Model.Extion
{
    public class notfiyDTO
    {
        public string Message { get; set; }
        public DateOnly Date { get; set; }
        public string Time { get; set; }
        public string notifyHeader { get; set; }
        public string? link { get; set; }

        //Conver From notfiy to notfiyDTO
        public static implicit operator notfiyDTO(Notify input)
            => new()
            {
                Date=DateOnly.FromDateTime(input.DateTime),
                link=input?.link,
                Message=input?.Message,
                notifyHeader=input.notifyHeader.ToString(),
                Time = input.DateTime.ToShortTimeString(),
            };
        //Convert From IList<Notify> to IEnumerable<notfiyDTO>
        public static IEnumerable<notfiyDTO> notfiyDTOs(IList<Notify> input)
        {
            foreach (var i in input)
                yield return i;
        }
    }
}
