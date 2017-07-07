using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coinify.Web.Models
{
    public class Note : Money
    {
        public int NoteId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Note Value")]
        public override int Value { get; set; }
        [NotMapped]
        public override int MoneyId => NoteId;
    }
}
