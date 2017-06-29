using System.ComponentModel.DataAnnotations;

namespace Coinify.Web.Models
{
    public class Note : IMoney
    {
        public int NoteId { get; set; }
        [Required]
        [Display(Name = "Note Value")]
        public int Value { get; set; }
    }
}
