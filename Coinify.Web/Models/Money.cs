using System;
using System.Linq;

namespace Coinify.Web.Models
{
    public abstract class Money : IMoney
    {
        public virtual int MoneyId { get; }
        public abstract int Value { get; set; }
    }
}
