namespace Coinify.Web.Models
{
    public class Coin : IMoney
    {
        public int CoinId { get; set; }
        public CoinSize Size { get; set; }
        public int Value { get; set; }
    }
}
