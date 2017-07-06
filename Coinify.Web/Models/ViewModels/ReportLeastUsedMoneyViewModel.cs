using System;
using System.Collections.Generic;

namespace Coinify.Web.Models.ViewModels
{
    public class ReportLeastUsedMoneyViewModel
    {
        public int AtmId { get; set; }
        public string AtmAlias { get; set; }
        public Dictionary<Money,int> Report { get; set; }
    }
}
