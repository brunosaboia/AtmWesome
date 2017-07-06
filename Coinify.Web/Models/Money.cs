using System;

namespace Coinify.Web.Models
{
    public abstract class Money : IMoney, IComparable<Money>
    {
        public virtual int MoneyId { get; }
        public abstract int Value { get; set; }

        public virtual int CompareTo(Money that)
        {
            int result = this.Value.CompareTo(that.Value);
            if (result != 0)
            {
                return result;
            }
            result = this.GetType().GetHashCode().CompareTo(that.GetType().GetHashCode());
            if (result != 0)
            {
                return result;
            }
            return this.MoneyId.CompareTo(that.MoneyId);
        }
    }
}
