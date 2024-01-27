using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PetConnect.Data;
using PetConnect.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;




namespace PetConnect.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ChatHub(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       
        public async Task SendMessage(string userId, string messageContent)
        {

            var currentUserId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

         

            var existingChat = _context.Chats.FirstOrDefault(c =>
                (c.CurrentUserId == currentUserId && c.OtherUserId == userId) ||
                (c.OtherUserId == currentUserId && c.CurrentUserId == userId));
           

            if (existingChat == null)
            {
                
                existingChat = new Chat
                {   CurrentUserId = currentUserId,
                    OtherUserId = userId,
                    CurrentUserName = GetUserNameById(currentUserId),
                    OtherUserName = GetUserNameById(userId),
                    Messages = new List<Message>()  
                };

                _context.Chats.Add(existingChat);
                await _context.SaveChangesAsync();

               
            }
            

           
            var message = new Message
            {
                ChatId = existingChat.ChatId,
                UserId = currentUserId, 
                Content = messageContent,
                Timestamp = DateTime.Now,
                
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            if (existingChat.OtherUserId == currentUserId) { await Clients.User(existingChat.CurrentUserId).SendAsync("ReceiveMessage", existingChat.OtherUserName, messageContent);  }
            if (existingChat.CurrentUserId == currentUserId) { await Clients.User(existingChat.OtherUserId).SendAsync("ReceiveMessage", existingChat.CurrentUserName, messageContent); }

            
        }

       
        private string GetUserNameById(string userId)
        {
           
            var user = _userManager.FindByIdAsync(userId).Result;
            return user != null ? user.UserName : "Utilizator necunoscut";
        }
    }
}
