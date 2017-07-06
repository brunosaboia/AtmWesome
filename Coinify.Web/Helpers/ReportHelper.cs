using Coinify.Web.Models;
using Coinify.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async static Task<IEnumerable<ReportLeastUsedMoneyViewModel>> GenerateMoneyReport(CoinifyWebContext context, int? id, bool shouldOrder = true)
        {
            var tmpDict = new Dictionary<Tuple<string, int>, Dictionary<string, KeyValuePair<Money, int>>>();
            List<Withdraw> withdrawList = new List<Withdraw>();

            if (id == null)
            {
                withdrawList = await context
                    .Withdraw
                    .Include(w => w.CurrencyDictionary)
                    .Include(w => w.AutomatedTellerMachine)
                    .ToListAsync();
            }
            else
            {
                withdrawList = await context
                    .Withdraw
                    .Include(w => w.CurrencyDictionary)
                    .Include(w => w.AutomatedTellerMachine)
                    .Where(w => w.AutomatedTellerMachine.AutomatedTellerMachineId == id)
                    .ToListAsync();
            }

            foreach(var withdraw in withdrawList)
            {
                var atm = withdraw.AutomatedTellerMachine;
                var key = new Tuple<string, int>
                    (atm.Alias, atm.AutomatedTellerMachineId);

                if (!tmpDict.ContainsKey(key))
                    tmpDict.Add(key, new Dictionary<string, KeyValuePair<Money, int>>());

                foreach (var money in withdraw.CurrencyDictionary.MoneyDictionary)
                {
                    var moneyKey = $"{money.Key.GetType().FullName}${money.Key.Value}${money.Key.MoneyId}";

                    if (!tmpDict[key].ContainsKey(moneyKey))
                    {
                        tmpDict[key].Add(moneyKey, new KeyValuePair<Money, int>(money.Key, money.Value));
                    }
                    else
                    {
                        var kvp = tmpDict[key][moneyKey];
                        tmpDict[key][moneyKey] = new KeyValuePair<Money, int>(kvp.Key, kvp.Value + money.Value);
                    }
                }
            }

            return tmpDict.Select(kvp => new ReportLeastUsedMoneyViewModel()
            {
                AtmId = kvp.Key.Item2,
                AtmAlias = kvp.Key.Item1,
                Report = shouldOrder ?
                    kvp.Value.Select(_ => _.Value).OrderBy(_ => _.Value).ToDictionary(k => k.Key, k => k.Value)
                    : kvp.Value.ToDictionary(k => k.Value.Key, k => k.Value.Value)
            });
        }
    }
}
