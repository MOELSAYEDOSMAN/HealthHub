namespace HealthHup.API.Service.MessageServiceFolder
{
    public interface IMessageService
    {
        Task SendMessage(string Email, string Message, string? Color);
        string GetEmail();
    }
}
