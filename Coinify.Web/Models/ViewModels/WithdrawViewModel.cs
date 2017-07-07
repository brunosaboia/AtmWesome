using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Coinify.Web.Models.ViewModels
{
    public class WithdrawViewModel
    {
        public int WithdrawId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
        public DateTime WithdrawDate  { get; set; }
        [Required]
        [Display(Name = "Select ATM")]
        public int AtmId { get; set; }
        [Required]
        [Display(Name = "Select User")]
        public int UserId { get; set; }

        public Withdraw ToModel(CoinifyWebContext context)
        {
            return new Withdraw()
            {
                WithdrawId = WithdrawId,
                User = context.User.Find(UserId),
                AutomatedTellerMachine = context
                    .AutomatedTellerMachine
                    .Include(atm => atm.CurrencyDictionary)
                    .SingleOrDefault(atm => atm.AutomatedTellerMachineId == AtmId)
            };
        }
    }
}
