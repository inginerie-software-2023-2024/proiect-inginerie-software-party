using System.ComponentModel.DataAnnotations;

namespace PetConnect.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int? PetId { get ;set; }
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Continutul este obligatoriu")]
        [StringLength(500, ErrorMessage = "Continutul nu poate avea mai mult de 500 de caractere")]
        public string Content { get; set; }
        public DateTime Date { get; set; }

        virtual public ApplicationUser? User { get; set; }   
        virtual public Pet? Pet { get; set; }   

    }
}
