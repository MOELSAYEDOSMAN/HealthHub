using HealthHup.API.Model.Extion.Account;
using HealthHup.API.Model.Extion.Message;
using System.Threading.Tasks;

namespace HealthHup.API.Service.ModelService.HospitalService.Chat
{
    public interface IChatService
    {
        Task<IEnumerable<MessageModelDTO>> GetMessages(string User1, string User2);
        Task<IEnumerable<DTOUserInformation>> GetUserChat(string Email);
    }
}
