using Coinify.Web.Exceptions;
using Coinify.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coinify.Web.Helpers
{
    public static  class WithdrawHelper
    {
        private static Dictionary<IMoney, int> GenerateValidMoneyDictionary(CurrencyDictionary dict)
        {
            var validMoney = dict
                .MoneyDictionary
                .Where(kvp => kvp.Value > 0)
                .OrderByDescending(kvp => kvp.Key is Note)
                .ToDictionary(kvp => kvp.Key as IMoney, kvp => kvp.Value);

            return validMoney;
        }

        public static CurrencyDictionary WithdrawFromAtm(User user, AutomatedTellerMachine atm, int amount, CoinifyWebContext context = null)
        {
            var curDict = atm.CurrencyDictionary;

            var ret = new CurrencyDictionary()
            {
                CoinDictionary = new Dictionary<Coin, int>(),
                NoteDictionary = new Dictionary<Note, int>()
            };

            if (user.Balance - amount < 0)
            {
                throw new InsufficentFundsException($"User {user.UserId} has insuficient funds to withdraw ${amount}");
            }

            user.Balance -= amount;

            var validMoney = GenerateValidMoneyDictionary(curDict);

            foreach (var kvp in validMoney.ToList())
            {
                while (validMoney[kvp.Key] != 0)
                {
                    if ((amount - kvp.Key.Value) < 0)
                        break;

                    amount -= kvp.Key.Value;
                    validMoney[kvp.Key]--;

                    if (kvp.Key is Note)
                    {
                        var note = kvp.Key as Note;

                        if (ret.NoteDictionary.ContainsKey(note))
                            ret.NoteDictionary[note]++;
                        else ret.NoteDictionary[note] = 1;

                        curDict.NoteDictionary[note]--;
                    }
                    else if (kvp.Key is Coin)
                    {
                        var coin = kvp.Key as Coin;

                        if (ret.CoinDictionary.ContainsKey(coin))
                            ret.CoinDictionary[coin]++;
                        else ret.CoinDictionary[coin] = 1;

                        curDict.CoinDictionary[coin]--;

                    }
                    else break;
                }
            }
            if (amount > 0)
            {
                throw new
                    InsufficientChangeException($"There is no change in ATM {atm.AutomatedTellerMachineId} to withdraw ${amount}");
            }

            if (context != null)
            {
                context.Update(user);
                context.Update(atm);

                context.SaveChanges();
            }

            return ret;
        }
    }
}
