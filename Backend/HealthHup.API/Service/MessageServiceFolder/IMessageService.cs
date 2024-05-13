namespace HealthHup.API.Service.MessageServiceFolder
{
    public interface IMessageService
    {
        Task SendMessage(string Email, string Message, string? Color);
        Task<bool> ConfirmAccount(string Email, string link);
        
    }
}
