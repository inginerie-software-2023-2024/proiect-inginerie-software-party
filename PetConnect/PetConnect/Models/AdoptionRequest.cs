using System.ComponentModel.DataAnnotations;

namespace PetConnect.Models
{
    public class AdoptionRequest
    {
        [Key]
        public int RequestId { get; set; }
        public int Status { get; set; }
        public DateTime RequestDate { get; set; }
        public string? UserId { get; set; }
        public int? PetId { get; set; }

        virtual public ApplicationUser? User { get; set; }
        virtual public Pet? Pet { get; set; }   
    }
}
