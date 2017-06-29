using System.ComponentModel.DataAnnotations;

namespace Coinify.Web.Models.ViewModels
{
    public class CoinViewModel
    {
        [Required]
        public int CoinId { get; set; }
        [Required]
        public int Value { get; set; }
        [Required]
        public int Size { get; set; }

        public static CoinViewModel FromModel(Coin model)
        {
            return new CoinViewModel()
            {
                CoinId = model.CoinId,
                Value = model.Value,
                Size = model.Size.CoinSizeId
            };
        }        
    }
}
