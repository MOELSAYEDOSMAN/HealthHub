using HealthHup.API.Model.Extion.Account;
using HealthHup.API.Model.Extion.Message;
using HealthHup.API.Service.AccountService;
using System.Runtime.Intrinsics.X86;

namespace HealthHup.API.Service.ModelService.HospitalService.Chat
{
    public class ChatService:BaseService<Model.Models.Chat.Message>,IChatService
    {
        
        private readonly IAuthService _authService;
        public ChatService(ApplicatoinDataBaseContext db,IAuthService authService)
            :base(db)
        {
            _authService= authService;
        }

        public async Task<IEnumerable<MessageModelDTO>> GetMessages(string User1,string User2)
        {
            var US1=await _authService.GetUserAsync(User1);
            var US2 = await _authService.GetUserAsync(User2);
            
            return MessageModelDTO
                .ToListMessageModelDTOFromListMessage(await findByAsync(m=>(m.UserSendId == US1.Id && m.UserReciveId == US2.Id)||(m.UserSendId == US2.Id && m.UserReciveId == US1.Id),new string[] { "UserSend" },o=>o.dateTiemSendMessage));
        }
        public async Task<IEnumerable<DTOUserInformation>> GetUserChat(string Email)
        {
            var US1 = await _authService.GetUserAsync(Email);
            var SendUser = (await findByAsync(m => m.UserReciveId == US1.Id)).Select(d=>d.UserSend);
            return DTOUserInformation.ConvertFromListOFUser(SendUser.DistinctBy(u=>u.Id).ToList());
        }
    }
}
