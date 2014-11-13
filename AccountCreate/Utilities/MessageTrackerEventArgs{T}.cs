using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class MessageTrackerEventArgs<T> : EventArgs
    {
        public Guid MessageId { get; set; }
        public T Data { get; set; }
    }
}
