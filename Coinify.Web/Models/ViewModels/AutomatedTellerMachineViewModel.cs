using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Coinify.Web.Models.ViewModels
{
    public class AutomatedTellerMachineViewModel
    {
        public int AutomatedTellerMachineId { get; set; }
        public string Alias { get; set; }
        [Display(Name = "Avaiable coins")]
        public Dictionary<int, int> CoinDictionary { get; set; }
        [Display(Name = "Avaiable notes")]
        public Dictionary<int, int> NoteDictionary { get; set; }
        [Display(Name = "Has a note dispenser?")]
        public bool HasNoteDispenser { get; set; }
        [Display(Name = "Coin Dipenser Sizes")]
        public Dictionary<int, bool> CoinDispensersDictionary { get; set; }        

        public static AutomatedTellerMachineViewModel FromModel(AutomatedTellerMachine model)
        {
            return new AutomatedTellerMachineViewModel()
            {
                AutomatedTellerMachineId = model.AutomatedTellerMachineId,
                Alias = model.Alias,
                CoinDictionary = model.CoinDictionary.ToDictionary(k => k.Key.CoinId, v => v.Value),
                NoteDictionary = model.NoteDictionary.ToDictionary(k => k.Key.NoteId, v => v.Value),
                HasNoteDispenser = model.HasNoteDispenser,
                CoinDispensersDictionary = model
                    .CoinDispensersDictionary
                    .ToDictionary(k => k.Key.CoinSizeId, v => v.Value)
            };
        }

        public AutomatedTellerMachine ToModel(CoinifyWebContext context)
        {
            return new AutomatedTellerMachine()
            {
                AutomatedTellerMachineId = AutomatedTellerMachineId,
                Alias = Alias,
                CoinDictionary = CoinDictionary.ToDictionary(k => context.Coin.Find(k.Key), v => v.Value),
                NoteDictionary = NoteDictionary.ToDictionary(k => context.Note.Find(k.Key), v => v.Value),
                HasNoteDispenser = HasNoteDispenser,
                CoinDispensersDictionary = CoinDispensersDictionary
                    .ToDictionary(k => context.CoinSize.Find(k.Key), v => v.Value)
            };
        }
    }
}
