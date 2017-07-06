using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coinify.Web.Models
{
    public class Withdraw
    {
        public int WithdrawId { get; set; }
        [Required]
        public virtual User User { get; set; }
        [Required]
        [Display(Name = "ATM")]
        public virtual AutomatedTellerMachine AutomatedTellerMachine { get; set; }
        [Required]
        [Display(Name = "Withdraw Date")]
        public DateTime WithdrawDate { get; set; }
        [Required]
        public virtual CurrencyDictionary CurrencyDictionary { get; set; }

        [NotMapped]
        public int Amount
        {
            get
            {
                return CurrencyDictionary?.Balance ?? 0;
            }
        }
    }
}
