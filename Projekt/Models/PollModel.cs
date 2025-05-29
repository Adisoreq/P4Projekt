using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Models
{
    public class PollModel
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(3), MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(50)]
        public string Description { get; set; } = "";

        public bool Public { get; set; } = true;
        public bool Closed { get; set; } = false;
        public bool MultipleChoice { get; set; } = false;

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset End { get; set; }

        public int AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public UserModel Author { get; set; } = null!;

        public ICollection<OptionModel> Options { get; set; } = new List<OptionModel>();
        public ICollection<CategoryModel> Categories { get; set; } = [];
    }

}
