using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class CountryModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "Country";

        [MaxLength(3)]
        public string Code { get; set; } = "XXX";

        public override string ToString() => $"{Name} ({Code})";
    }
}
