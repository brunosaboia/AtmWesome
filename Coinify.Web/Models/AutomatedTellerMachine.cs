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
        public virtual ICollection<CoinSize> CoinDispensers { get; set; }

        [ScaffoldColumn(false)]
        public string JsonCoinDictionary
        {
            get => JsonConvert.SerializeObject(CoinDictionary, Formatting.None);
            set => CoinDictionary = JsonConvert.DeserializeObject<Dictionary<Coin, int>>(value);
            
        }

        [ScaffoldColumn(false)]
        public string JsonNoteDictionary
        {
            get => JsonConvert.SerializeObject(NoteDictionary, Formatting.None);
            set => NoteDictionary = JsonConvert.DeserializeObject<Dictionary<Note, int>>(value);
        }

        [NotMapped]
        public Dictionary<Coin, int> CoinDictionary { get; set; }

        [NotMapped]
        public Dictionary<Note, int> NoteDictionary { get; set; }

        [NotMapped]
        public int AvaiableBalance => NoteDictionary.Sum(n => n.Value) + CoinDictionary.Sum(c => c.Value);        
    }
}
