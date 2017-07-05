using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coinify.Web.Exceptions
{
    public class InsufficentFundsException : Exception
    {
        public InsufficentFundsException(string message) : base(message)
        {

        }
    }
}
