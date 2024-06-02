namespace HealthHup.API.Model.Extion.AdminLog
{
    public class AdminLogsDTO
    {
        public string? userid { get; set; }
        public string adminAction { get; set; }
        public DateTime createAt { get; set; }
        public string? Message { get; set; }

        public static implicit operator AdminLogsDTO(LogAdminAction input)
            => new AdminLogsDTO
            {
                adminAction = input.adminAction.ToString(),
                createAt=input.dateCreated,
                userid=input.UserId,
                Message=input?.Message,
            };

        public static IEnumerable<AdminLogsDTO> ConvertFromLogAdminAction(IList<LogAdminAction> Logs)
        {
            foreach (var i in Logs)
                yield return i;
        }
    }
}
