using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coinify.Web.Exceptions
{
    public class InsufficientChangeException : Exception
    {
        public InsufficientChangeException(string message) : base(message)
        {

        }
    }
}
