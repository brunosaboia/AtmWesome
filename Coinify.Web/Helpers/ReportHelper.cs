using Coinify.Web.Models;
using System.Linq;

namespace Coinify.Web.Helpers
{
    public static class ReportHelper
    {
        public static string[] GenerateWithdrawDetails(CurrencyDictionary dict)
        {
            var noteInfo = dict
                .NoteDictionary
                .Select(kvp => $"{kvp.Value} notes of ${kvp.Key.Value}")
                .ToArray();

            var coinInfo = dict
                .CoinDictionary
                .Select(kvp => $"{kvp.Value} coins of ${kvp.Key.Value} (size: {kvp.Key.Size.ToString()})")
                .ToArray();

            var detailedInfo = new string[]
            {
                $"The money was given in the following fashion: ",
                $"Notes: {string.Join(", ", (noteInfo.Length > 0 ? noteInfo : new[] { "No notes were given" }))}",
                $"Coins: {string.Join(", ", (coinInfo.Length > 0 ? coinInfo : new[] { "No coins were given" }))}"
            };

            return detailedInfo;
        }

        public static string PrintFormattedMoney(IMoney money)
        {
            if (money is Coin)
            {
                var coin = money as Coin;
                return $"Coin: ${coin.Value} (Size: {coin.Size.ToString()})";
            }
            else if (money is Note)
            {
                return $"Note: ${money.Value}";
            }
            else return string.Empty;
        }
    }
}
