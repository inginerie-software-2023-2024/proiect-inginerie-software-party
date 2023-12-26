using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetConnect.Models
{
    public class Pet
    {
        [Key]
        public int PetId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public int Size { get; set; }
        public bool Sex { get; set; }
        public string Color { get; set; }
        public bool Vaccined { get; set; }
        public bool Sterilized { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string? UserId { get; set; }
        virtual public IdentityUser? User { get; set; }
        virtual public ICollection<AdoptionRequest>? AdoptionRequests { get; set; }
        virtual public ICollection<Comment>? Comments { get; set; }
    }
}
