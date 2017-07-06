using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Coinify.Web.Models;

namespace Coinify.Web.Helpers
{
    public static class DatabaseHelper
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            var db = app.ApplicationServices.GetService<CoinifyWebContext>();

            db.EnsureSeedData();
        }

        public static void EnsureSeedData(this CoinifyWebContext context)
        {
            if (context.Database.GetPendingMigrations().Count() > 0)
            {
                context.Database.Migrate();
            }

            var coinSizes = new[]
            {
                new CoinSize()
                {
                    Size = 10
                },
                new CoinSize()
                {
                    Size = 20
                },
                new CoinSize()
                {
                    Size = 30
                },
                new CoinSize()
                {
                    Size = 40
                },
                new CoinSize()
                {
                    Size = 50
                },
            };

            var coins = new[]
            {
                new Coin()
                {
                    Value = 20,
                    Size = coinSizes[3]
                },
                new Coin()
                {
                    Value = 10,
                    Size = coinSizes[1]
                },
                new Coin()
                {
                    Value = 5,
                    Size = coinSizes[4]
                },
                new Coin()
                {
                    Value = 2,
                    Size = coinSizes[2]
                },
                new Coin()
                {
                    Value = 1,
                    Size = coinSizes[0]
                },
            };

            var notes = new[]
            {
                new Note()
                {
                    Value = 1000
                },
                new Note()
                {
                    Value = 500
                },
                new Note()
                {
                    Value = 200
                },
                new Note()
                {
                    Value = 100
                },
                new Note()
                {
                    Value = 50
                },
            };

            var currencyDictionary = new CurrencyDictionary()
            {
                CoinDictionary = new System.Collections.Generic.Dictionary<Coin, int>()
                {
                    { coins[0], 100 },
                    { coins[1], 200 },
                    { coins[2], 300 },
                    { coins[3], 400 },
                    { coins[4], 500 },
                },
                NoteDictionary = new System.Collections.Generic.Dictionary<Note, int>()
                {
                    { notes[0], 100 },
                    { notes[1], 200 },
                    { notes[2], 300 },
                    { notes[3], 400 },
                    { notes[4], 500 },
                }
            };

            var atm = new AutomatedTellerMachine()
            {
                Alias = "AmsTerdaM",
                CurrencyDictionary = currencyDictionary,
                HasNoteDispenser = true,
                CoinDispensersDictionary = new System.Collections.Generic.Dictionary<CoinSize, bool>()
                {
                    { coinSizes[4], true },
                    { coinSizes[2], true },
                }
            };

            var users = new[]
            {
                new User()
                {
                    Name = "Bill Gates",
                    Balance = 1000000000
                },
                new User()
                {
                    Name = "Bruno",
                    Balance = 1000
                }
            };

            if (!context.CoinSize.Any())
            {
                context.CoinSize.AddRange(coinSizes);
            }

            if (!context.Coin.Any())
            {
                context.Coin.AddRange(coins);
            }

            if (!context.Note.Any())
            {
                context.Note.AddRange(notes);
            }

            if (!context.AutomatedTellerMachine.Any())
            {
                context.CurrencyDictionary.Add(currencyDictionary);
                context.AutomatedTellerMachine.Add(atm);
            }

            if (!context.User.Any())
            {
                context.User.AddRange(users);
            }

            context.SaveChanges();

        }
    }
}
