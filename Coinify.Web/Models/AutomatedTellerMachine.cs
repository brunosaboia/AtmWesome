using System.Collections.Generic;

namespace Coinify.Web.Models
{
    public class AutomatedTellerMachine
    {
        public int AutomatedTellerMachineId { get; set; }

        public bool HasNoteDispenser { get; set; }

        public virtual ICollection<CoinSize> CoinDispensers { get; set; }
    }
}
