using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Models
{
    public class OptionModel
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public required string Text { get; set; }

        public int PollId { get; set; }

        [ForeignKey(nameof(PollId))]
        public PollModel Poll { get; set; } = null!;
    }

}
