namespace HealthHup.API.Service.MessageServiceFolder
{
    public interface ISendMessageService
    {
        Task ActionWithDoctor(string Email, string Name, string Messsage);
        Task ConfirmAccount(string Email, string Link);
        Task ForgetPassword(string Email, string UserName, string Message);
        Task AlertDate(string Email, string UserName, string DoctorName, DateAction dateAction, string Message);
        Task TakeBlock(string Email, string Name, string Message);
        Task ForgetDateMedicalSession(string Email, string UserName, string DoctorName, string Message);
        Task RateDoctorLow(string Email, string UserName);
        
    }
}
