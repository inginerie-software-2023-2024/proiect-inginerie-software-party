using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PetConnect.Models
{
    public class Pet
    {
        [Key]
        public int PetId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Species { get; set; }

        [Required]
        public string Breed { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public int Size { get; set; }

        [Required]
        public bool Sex { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public bool Vaccined { get; set; }

        [Required]
        public bool Sterilized { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Image { get; set; }
        public string? UserId { get; set; }
        virtual public IdentityUser? User { get; set; }
        public bool Approved { get; set; }
        virtual public ICollection<AdoptionRequest>? AdoptionRequests { get; set; }
        virtual public ICollection<Comment>? Comments { get; set; }
    }
}
