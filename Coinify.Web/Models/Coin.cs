using System.ComponentModel.DataAnnotations;

namespace Coinify.Web.Models
{
    public class Coin : IMoney
    {
        public int CoinId { get; set; }
        [Required]
        [Display(Name = "Coin Size")]
        public CoinSize Size { get; set; }
        [Required]
        [Display(Name = "Coin Value")]
        public int Value { get; set; }
    }
}
