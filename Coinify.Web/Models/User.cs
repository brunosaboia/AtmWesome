using System.ComponentModel.DataAnnotations;

namespace Coinify.Web.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
