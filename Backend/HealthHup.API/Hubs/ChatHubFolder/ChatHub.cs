using HealthHup.API.Model.Extion.Message;
using HealthHup.API.Service.AccountService;
using Humanizer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace HealthHup.API.Hubs.ChatHubFolder
{
    
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
            await Groups.AddToGroupAsync(Context.ConnectionId,GroupName);
        }
      
        public async Task sendMessageAsync(string message,string groupName,string EmailSend,string EmailTo)
        {
            
            var Ur = await _authService.GetUserAsync(EmailSend);
            var Ur2 = await _authService.GetUserAsync(EmailTo);

            MessageModelDTO messageModel = new MessageModelDTO()
            {
                date = DateOnly.FromDateTime(DateTime.Now),
                time= DateTime.UtcNow.ToLongTimeString(),
                email=Ur.Email,
                imgSrc=$"/Image/User/{Ur.src}",
                message = message,
                name=Ur.Name,
            };
            await _messageService.AddAsync(new Message()
            {
                dateTiemSendMessage = DateTime.Now,
                Id=Guid.NewGuid(),
                See=true,
                text=message,
                UserRecive= Ur2??new ApplicationUser(),
                UserSend=Ur??new ApplicationUser()
                
            });
            await Clients.OthersInGroup(groupName).SendAsync("reciveMessage", messageModel);
        }
    }
}
