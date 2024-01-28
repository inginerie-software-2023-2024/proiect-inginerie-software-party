using System.ComponentModel.DataAnnotations;

namespace PetConnect.Models
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }
        public string? CurrentUserId { get; set; }
        public string? OtherUserId { get; set; }
        public string? CurrentUserName { get; set; }
        public string? OtherUserName { get; set; }
        public ICollection<Message>? Messages { get; set; }
        virtual public ApplicationUser? OtherUser { get; set; }
        virtual public ApplicationUser? CurrentUser { get; set; }
    }
}
