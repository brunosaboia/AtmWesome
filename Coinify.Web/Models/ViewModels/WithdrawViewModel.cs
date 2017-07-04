using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coinify.Web.Models.ViewModels
{
    public class WithdrawViewModel
    {
        public int WithdrawId { get; set; }
        public int Amount { get; set; }
        public DateTime WithdrawDate  { get; set; }
    }
}
