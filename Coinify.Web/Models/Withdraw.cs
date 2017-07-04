using System;
using System.ComponentModel.DataAnnotations;

namespace Coinify.Web.Models
{
    public class Withdraw
    {
        public int WithdrawId { get; set; }
        [Required]
        public virtual User User { get; set; }
        [Required]
        public virtual AutomatedTellerMachine AutomatedTellerMachine { get; set; }
        [Required]
        public DateTime WithdrawDate { get; set; }
        [Required]
        public virtual CurrencyDictionary CurrencyDictionary { get; set; }
    }
}
