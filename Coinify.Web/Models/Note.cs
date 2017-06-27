namespace Coinify.Web.Models
{
    public class Note : IMoney
    {
        public int NoteId { get; set; }
        public int Value { get; set; }
    }
}
