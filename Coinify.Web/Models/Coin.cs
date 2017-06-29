using System.ComponentModel.DataAnnotations;

namespace Coinify.Web.Models
{
    public class Coin : IMoney
    {
        public int CoinId { get; set; }
        [Required]
        public CoinSize Size { get; set; }
        [Required]
        public int Value { get; set; }
    }
}
