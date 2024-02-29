using HealthHup.API.Service.AccountService;
using Microsoft.AspNetCore.SignalR;

namespace HealthHup.API.Hubs.ChatHubFolder
{
    public class ChatHub:Hub
    {
        private readonly IBaseService<Message> _messageService;
        private readonly IAuthService _authService;
        public ChatHub(IBaseService<Message> messageService,IAuthService authService)
        {
            _messageService= messageService;
            _authService= authService;
        }

        //[Authorize]
        public async Task JoinGroupAsync(string GroupName)
        {
            await Console.Out.WriteLineAsync(Context.UserIdentifier);
            await Groups.AddToGroupAsync(Context.ConnectionId,GroupName);
        }
    }
}
