using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Coinify.Web.Models.ViewModels
{
    public class AutomatedTellerMachineViewModel
    {
        public string Alias { get; set; }

        [Display(Name = "Avaiable coins")]
        public Dictionary<Coin, int> CoinDictionary { get; set; }
        [Display(Name = "Avaiable notes")]
        public Dictionary<Note, int> NoteDictionary { get; set; }

        [Display(Name = "Has a note dispenser?")]
        public bool HasNoteDispenser { get; set; }

        [Display(Name = "Coin Dipenser Sizes")]
        public List<CoinSize> CoinDispensers { get; set; }

        [Display(Name = "Avaiable Balance")]
        public int AvaiableBalance => NoteDictionary.Sum(n => n.Value) + CoinDictionary.Sum(c => c.Value);
    }
}
