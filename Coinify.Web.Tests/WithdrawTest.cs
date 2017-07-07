using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coinify.Web.Models;
using Coinify.Web.Exceptions;
using System.Collections.Generic;
using Coinify.Web.Helpers;
using System.Linq;

namespace Coinify.Web.Tests
{
    [TestClass]
    public class WithdrawTest
    {
        CoinSize[] coinSizes { get; set; }
        Coin[] coins { get; set; }
        Note[] notes { get; set; }
        User[] users { get; set; }
        AutomatedTellerMachine atm { get; set; }
        CurrencyDictionary currencyDictionary { get; set; }
        CurrencyDictionary emptyCurrenctyDictionary { get; set; }
        AutomatedTellerMachine emptyAtm { get; set; }

        [TestInitialize()]
        public void Initialize()
        {
            (coinSizes, coins, notes, users, atm, currencyDictionary) = Helpers
                .DatabaseHelper
                .CreateEntitiesArrays();

            var allDispensers = new Dictionary<CoinSize, bool>();

            foreach(var size in coinSizes)
            {
                allDispensers.Add(size, true);
            }

            emptyCurrenctyDictionary = new CurrencyDictionary()
            {
                CoinDictionary = new Dictionary<Coin, int>(),
                NoteDictionary = new Dictionary<Note, int>()
            };

            emptyAtm = new AutomatedTellerMachine()
            {
                CurrencyDictionary = emptyCurrenctyDictionary,
                Alias = "Empty ATM",
                CoinDispensersDictionary = allDispensers,
                HasNoteDispenser = true
            };
        }

        [TestMethod]
        public void WithdrawShouldRemoveBalanceFromUser()
        {
            var testUser = users[0];

            var balance = testUser.Balance;

            WithdrawHelper.WithdrawFromAtm(testUser, atm, 1000);

            Assert.AreEqual(balance - 1000, testUser.Balance);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficentFundsException))]
        public void UserWithdrawWithInsufficientFundsShouldThrow()
        {
            var testUser = users[1];

            WithdrawHelper.WithdrawFromAtm(testUser, atm, 3000);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientChangeException))]
        public void UserWithdrawShouldThrowIfAtmHasNotEnoughChange()
        {
            var testUser = users[0];

            WithdrawHelper.WithdrawFromAtm(testUser, atm, 99999999);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientChangeException))]
        public void EmptyAtmShouldNotWithdraw()
        {
            var testUser = users[0];

            WithdrawHelper.WithdrawFromAtm(testUser, emptyAtm, 1);
        }

        //TODO: Maybe it is better to throw an exception here? Invalid Op?
        [TestMethod]
        public void NegativeWithdrawShouldYieldNothing()
        {
            var testUser = users[0];

            var result = WithdrawHelper.WithdrawFromAtm(testUser, atm, -1);

            var coinCount = result.CoinDictionary.Count;
            var noteCount = result.NoteDictionary.Count;

            Assert.AreEqual((0, 0), (coinCount, noteCount));
        }

        [TestMethod]
        public void WithdrawShouldPreferNotesOverCoins()
        {
            var testUser = users[0];

            var result = WithdrawHelper.WithdrawFromAtm(testUser, atm, 1000);

            Assert.AreEqual(0, result.CoinDictionary.Count);
        }

        [TestMethod]
        public void WithdrawShouldYieldRightChange()
        {
            var testUser = users[0];

            var result = WithdrawHelper.WithdrawFromAtm(testUser, atm, 578);

            var noteFiveHundredCount = result
                .NoteDictionary
                .Count(c => c.Key.Value == 500);

            var noteFiftyCount = result
                .NoteDictionary
                .Count(c => c.Key.Value == 50);

            var coinTwentyCount = result
                .CoinDictionary
                .Count(c => c.Key.Value == 20);

            var coinFiveCount = result
                .CoinDictionary
                .Count(c => c.Key.Value == 5);

            var coinTwoCount = result
                .CoinDictionary
                .Count(c => c.Key.Value == 2);

            var coinOneCount = result
                .CoinDictionary
                .Count(c => c.Key.Value == 1);

            var actual = (
                noteFiveHundredCount,
                noteFiftyCount,
                coinTwentyCount,
                coinFiveCount,
                coinTwoCount,
                coinOneCount);

            var expected = (1, 1, 1, 1, 1, 1);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WithdrawBalanceShouldBeEqualToAmount()
        {
            var testUser = users[0];

            var result = WithdrawHelper.WithdrawFromAtm(testUser, atm, 1000);

            Assert.AreEqual(1000, result.Balance);
        }
    }
}
