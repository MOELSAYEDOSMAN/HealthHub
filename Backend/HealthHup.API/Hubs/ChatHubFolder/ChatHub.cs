using HealthHup.API.Model.Extion.Message;
using HealthHup.API.Service.AccountService;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace HealthHup.API.Hubs.ChatHubFolder
{
    [Authorize]
    public class ChatHub:Hub
    {
        private readonly IBaseService<Message> _messageService;
        private readonly IAuthService _authService;
        public ChatHub(IBaseService<Message> messageService,IAuthService authService)
        {
            _messageService = messageService;
            _authService= authService;
        }
        public async Task joinGroupAsync(string GroupName)
        {
            
            await Console.Out.WriteLineAsync(Context.User.FindFirstValue(ClaimTypes.Email));
            await Groups.AddToGroupAsync(Context.ConnectionId,GroupName);
        }
        public async Task sendMessageAsync(string message,string groupName)
        {
            var Ur = await _authService.GetUserWithEmailAsync(Context.User.FindFirstValue(ClaimTypes.Email));
            MessageModelDTO messageModel = new MessageModelDTO()
            {
                date = DateOnly.FromDateTime(DateTime.Now),
                time= DateTime.UtcNow.ToLongTimeString(),
                email=Ur.email,
                imgSrc=$"/Image/User/{Ur.img}",
                message = message,
                name=Ur.name,
            };
            await Clients.OthersInGroup(groupName).SendAsync("SendAsync", messageModel);
        }
    }
}
