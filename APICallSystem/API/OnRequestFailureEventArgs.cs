using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICallSystem.API
{
    public class OnRequestFailureEventArgs<T> : EventArgs
    {
        public required string errorData;
        public required HttpResponseMessage response;
    }
}
