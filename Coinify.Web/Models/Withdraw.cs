using System;
using System.ComponentModel.DataAnnotations;

namespace Coinify.Web.Models
{
    public class Withdraw
    {
        public int TransactionId { get; set; }
        [Required]
        public virtual User User { get; set; }
        [Required]
        public virtual AutomatedTellerMachine AutomatedTellerMachine { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
    }
}
