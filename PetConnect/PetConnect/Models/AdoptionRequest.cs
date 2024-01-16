using System.ComponentModel.DataAnnotations;

namespace PetConnect.Models
{
    public class AdoptionRequest
    {
        [Key]
        public int RequestId { get; set; }
        public int Status { get; set; }
        public DateTime RequestDate { get; set; }
        public string? AdopterId { get; set; }
        public int? PetId { get; set; }

        [Required(ErrorMessage = "Mesajul este obligatoriu")]
        public string Message { get; set; }
        virtual public ApplicationUser? Adopter { get; set; }
        virtual public Pet? Pet { get; set; }   
    }
}
