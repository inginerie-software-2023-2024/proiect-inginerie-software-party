using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace PetConnect.Models
{
    public class Pet
    {
        [Key]
        public int PetId { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu")]
        [StringLength(50, ErrorMessage = "Numele nu poate avea mai mult de 50 de caractere")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Specia este obligatorie")]
        [StringLength(100, ErrorMessage = "Specia nu poate avea mai mult de 100 de caractere")]
        public string Species { get; set; }

        [Required(ErrorMessage = "Rasa este obligatorie")]
        [StringLength(100, ErrorMessage = "Rasa nu poate avea mai mult de 100 de caractere")]
        public string Breed { get; set; }

        [Required(ErrorMessage = "Varsta este obligatorie")]
        [Range(0,30, ErrorMessage = "Varsta trebuie sa fie intre 0 si 30 de ani")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Marimea este obligatorie")]
        [Range(3, 200, ErrorMessage = "Marimea trebuie sa fie intre 3 si 250 de cm")]
        public int Size { get; set; }

        [Required(ErrorMessage = "Genul este obligatoriu")]
        public bool Sex { get; set; }

        [Required(ErrorMessage = "Culoarea este obligatorie")]
        [StringLength(100, ErrorMessage = "Culoarea nu poate avea mai mult de 100 de caractere")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Starea de vaccin este obligatorie")]
        public bool Vaccined { get; set; }

        [Required(ErrorMessage = "Starea de sterilizare este obligatorie")]
        public bool Sterilized { get; set; }

        [Required(ErrorMessage = "Locatia este obligatorie")]
        [StringLength(200, ErrorMessage = "Locatia nu poate avea mai mult de 200 de caractere")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Descrierea este obligatorie")]
        [StringLength(700, ErrorMessage = "Descrierea nu poate avea mai mult de 700 de caractere")]
        [MinLength(15, ErrorMessage = "Descrierea trebuie sa aiba mai mult de 15 caractere")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Poza este obligatorie")]
        public string Image { get; set; }
        public string? UserId { get; set; }
        virtual public IdentityUser? User { get; set; }
        public bool Approved { get; set; }
        virtual public ICollection<AdoptionRequest>? AdoptionRequests { get; set; }
        virtual public ICollection<Comment>? Comments { get; set; }
    }
}
