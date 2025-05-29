using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Models
{
    public class VoteModel
    {
        [Key]
        public int Id { get; set; }

        public int OptionId { get; set; }
        
        public int PollId { get; set; }
        
        public int UserId { get; set; }

        [ForeignKey(nameof(OptionId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public OptionModel Option { get; set; } = null!;

        [ForeignKey(nameof(PollId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public PollModel Poll { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public UserModel User { get; set; } = null!;
    }
}
