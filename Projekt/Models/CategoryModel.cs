using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class CategoryModel
    {
        [Key] 
        public int Id { get; set; }

        [Required, MinLength(3), MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        public ICollection<PollModel> Polls { get; set; } = [];

    }
}
