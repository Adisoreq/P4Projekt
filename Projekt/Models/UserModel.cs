using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }

        [Required, EmailAddress, MaxLength(255)]
        public required string Email { get; set; }

        [Required, MinLength(8), MaxLength(255)]
        public required string Password { get; set; }

        [Required]
        public required SexModel Sex { get; set; }

        public int NationalityId { get; set; }

        [ForeignKey(nameof(NationalityId))]
        public CountryModel Nationality { get; set; } = null!;

        public DateTimeOffset BirthDate { get; set; }
        public DateTimeOffset Created { get; set; }
    }

}
