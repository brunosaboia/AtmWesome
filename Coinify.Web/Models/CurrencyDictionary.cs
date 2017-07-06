using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Coinify.Web.Models
{
    public class CurrencyDictionary
    {
        public int CurrencyDictionaryId { get; set; }

        [ScaffoldColumn(false)]
        public string JsonCoinDictionary
        {
            // Do not use  Expression-Bodied Members to simplify the code below
            // There is an issue with EF and EBM.
            // See https://github.com/aspnet/Scaffolding/issues/410
            get
            {
                return JsonConvert
                    .SerializeObject(CoinDictionary.ToList(), Formatting.None);
            }
            set
            {
                CoinDictionary = JsonConvert
                    .DeserializeObject<List<KeyValuePair<Coin, int>>>(value)
                    .ToDictionary(k => k.Key, v => v.Value);
            }
        }

        [ScaffoldColumn(false)]
        public string JsonNoteDictionary
        {
            // Do not use  Expression-Bodied Members to simplify the code below
            // There is an issue with EF and EBM.
            // See https://github.com/aspnet/Scaffolding/issues/410

            get
            {
                return JsonConvert.SerializeObject(NoteDictionary.ToList(), Formatting.None);
            }
            set
            {
                NoteDictionary = JsonConvert
                    .DeserializeObject<List<KeyValuePair<Note, int>>>(value)
                    .ToDictionary(k => k.Key, v => v.Value);
            }
        }


        [NotMapped]
        public Dictionary<Coin, int> CoinDictionary { get; set; }

        [NotMapped]
        public Dictionary<Note, int> NoteDictionary { get; set; }

        [NotMapped]
        public Dictionary<Money, int> MoneyDictionary
        {
            get
            {
                var coinCast = CoinDictionary
                    .Select(kvp => new KeyValuePair<Money, int>(kvp.Key, kvp.Value));

                var noteCast = NoteDictionary
                    .Select(kvp => new KeyValuePair<Money, int>(kvp.Key, kvp.Value));

                return coinCast.Union(noteCast)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
        }

        [NotMapped]
        public int Balance => CoinBalance + NoteBalance;

        [NotMapped]
        public int CoinBalance => CoinDictionary.Sum(c => (c.Key.Value * c.Value));

        [NotMapped]
        public int NoteBalance => NoteDictionary.Sum(c => (c.Key.Value * c.Value));
    }
}
