using Microsoft.AspNetCore.Identity;

namespace PetConnect.Models
{
    public class ApplicationUser: IdentityUser
    {
        virtual public ICollection<AdoptionRequest>? AdoptionRequests { get; set; }
        virtual public ICollection<Pet>? Pets { get; set; }
        virtual public ICollection<Comment>? Comments { get; set; }
    }
}
