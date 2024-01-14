using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetConnect.Models
{
    public class ApplicationUser: IdentityUser
    {
        virtual public ICollection<AdoptionRequest>? AdoptionRequests { get; set; }
        virtual public ICollection<Pet>? Pets { get; set; }
        virtual public ICollection<Comment>? Comments { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }
    }
}
