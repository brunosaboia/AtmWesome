using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Coinify.Web.Models
{
    public class AutomatedTellerMachine
    {
        public int AutomatedTellerMachineId { get; set; }
        [Required]
        public string Alias { get; set; }
        [Required]
        [Display(Name = "Has a note dispenser?")]
        public bool HasNoteDispenser { get; set; }

        [ScaffoldColumn(false)]
        public string JsonCoinDispensersDictionary
        {
            // Do not use  Expression-Bodied Members to simplify the code below
            // There is an issue with EF and EBM.
            // See https://github.com/aspnet/Scaffolding/issues/410

            get
            {
                return JsonConvert
                    .SerializeObject(CoinDispensersDictionary.ToList(), Formatting.None);
            }
            set
            {
                CoinDispensersDictionary = JsonConvert
                    .DeserializeObject<List<KeyValuePair<CoinSize, bool>>>(value)
                    .ToDictionary(k => k.Key, v => v.Value);
            }
        }

        [NotMapped]
        public Dictionary<CoinSize, bool> CoinDispensersDictionary { get; set; }

        [Required]
        public virtual CurrencyDictionary CurrencyDictionary { get; set; }
    }
}
