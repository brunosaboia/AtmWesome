using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coinify.Web.Models
{
    public class Coin : Money
    {
        public int CoinId { get; set; }
        [Required]
        [Display(Name = "Coin Size")]
        public CoinSize Size { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Coin Value")]
        public override int Value { get; set; }

        [NotMapped]
        public override int MoneyId => CoinId;
    }
}
