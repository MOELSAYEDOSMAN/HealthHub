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
            Console.WriteLine("CreateClass");
            _messageService = messageService;
            _authService= authService;
        }
        public override Task OnConnectedAsync()
        {
            
           
            return base.OnConnectedAsync();
        }
        
        public async Task joinGroupAsync(string GroupName)
        {
            Console.WriteLine("____________JoinGroup__________________");

            await Console.Out.WriteLineAsync($"Join {GroupName}");
            Console.WriteLine("______________________________");

            await Groups.AddToGroupAsync(Context.ConnectionId,GroupName);
        }
      
        public async Task sendMessageAsync(string message,string groupName,string EmailSend)
        {
            Console.WriteLine("____________SendMessage__________________");

            Console.WriteLine(message);
            Console.WriteLine(groupName);
            Console.WriteLine(EmailSend);
            Console.WriteLine("______________________________");



            var Ur = await _authService.GetUserWithEmailAsync(EmailSend);
            MessageModelDTO messageModel = new MessageModelDTO()
            {
                date = DateOnly.FromDateTime(DateTime.Now),
                time= DateTime.UtcNow.ToLongTimeString(),
                email=Ur.email,
                imgSrc=$"/Image/User/{Ur.img}",
                message = message,
                name=Ur.name,
            };
            await Clients.OthersInGroup(groupName).SendAsync("reciveMessage", messageModel);
        }
    }
}
