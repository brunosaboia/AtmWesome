using System;

namespace Coinify.Web.Models.ViewModels
{
    public class WithdrawViewModel
    {
        public int WithdrawId { get; set; }
        public int Amount { get; set; }
        public DateTime WithdrawDate  { get; set; }
        public int AutomatedTellerMachineId { get; set; }
        public int UserId { get; set; }
    }
}
