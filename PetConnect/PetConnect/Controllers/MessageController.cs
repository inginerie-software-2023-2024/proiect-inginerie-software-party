using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PetConnect.Data;
using PetConnect.Hubs;
using PetConnect.Models;

namespace PetConnect.Controllers
{
    public class MessageController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MessageController(ApplicationDbContext context, IHubContext<ChatHub> hubContext, UserManager<ApplicationUser> userManager)
        {
            _hubContext = hubContext;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var currentUserId = _userManager.GetUserId(User);

            ViewBag.UserId = currentUserId;
            ViewBag.CurrentUserName = GetUserNameById(currentUserId);

            var chats = _context.Chats
                .Include(c => c.Messages)
                .Where(c => (c.CurrentUserId == currentUserId || c.OtherUserId == currentUserId) &&
                (c.Messages.Any(m => m.ChatId == c.ChatId)))
                .ToList();
           
          
           
            ViewBag.Chats = chats;
            
            return View();
        }

        [HttpGet]
        public IActionResult Conversation(string otherUserId)
        
        {
            var currentUserId = _userManager.GetUserId(User);
            ViewBag.UserId = currentUserId;
            ViewBag.CurrentUserName = GetUserNameById(currentUserId);
            var chat = _context.Chats.FirstOrDefault(c =>
                (c.CurrentUserId == currentUserId && c.OtherUserId == otherUserId) ||
                (c.OtherUserId == currentUserId && c.CurrentUserId == otherUserId));

            if (chat == null)
            {
                
                chat = new Chat
                {
                    CurrentUserId = currentUserId,
                    OtherUserId = otherUserId,
                    CurrentUserName = GetUserNameById(currentUserId),
                    OtherUserName = GetUserNameById(otherUserId),
                    Messages = new List<Message>()
                };

                _context.Chats.Add(chat);
                _context.SaveChanges();
            }

            var conversation = _context.Messages
                .Include(msg => msg.Chat)
                .Where(msg => (msg.ChatId == chat.ChatId))
                .OrderBy(msg => msg.Timestamp)
                .ToList();

            chat.Messages = conversation;


            return View(chat);
        }

        [HttpPost]
        public IActionResult StartConversation(string otherUserId, string content)
        {
            var currentUserId = _userManager.GetUserId(User);

            var existingChat = _context.Chats.FirstOrDefault(c =>
                (c.CurrentUserId == currentUserId && c.OtherUserId == otherUserId) ||
                (c.OtherUserId == currentUserId && c.CurrentUserId == otherUserId));

            if (existingChat == null)
            {
                
                existingChat = new Chat
                {
                    CurrentUserId = currentUserId,
                    OtherUserId = otherUserId,
                    CurrentUserName = GetUserNameById(currentUserId),
                    OtherUserName = GetUserNameById(otherUserId),
                    Messages = new List<Message>()  
                };

                _context.Chats.Add(existingChat);
                _context.SaveChanges();
            }

            
            var message = new Message
            {
                ChatId = existingChat.ChatId,  
                UserId = existingChat.CurrentUserId,
                Content = content,
                Timestamp = DateTime.Now,
                
            };

            _context.Messages.Add(message);
            _context.SaveChanges();

            _hubContext.Clients.User(otherUserId).SendAsync("ReceiveMessage", currentUserId, content);

            return RedirectToAction("Conversation", new { otherUserId });
        }



        private string GetUserNameById(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            return user != null ? 
            user.UserName : "Utilizator necunoscut";
        }
    }
}

