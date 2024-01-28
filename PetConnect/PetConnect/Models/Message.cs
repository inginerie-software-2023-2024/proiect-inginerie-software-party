using System.ComponentModel.DataAnnotations;

namespace PetConnect.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public int? ChatId { get; set; }
        public string? UserId { get; set; }
        public string? Content { get; set; }
        public DateTime Timestamp { get; set; }
        virtual public ApplicationUser? User { get; set; }
        virtual public Chat? Chat { get; set; }
    }
}
