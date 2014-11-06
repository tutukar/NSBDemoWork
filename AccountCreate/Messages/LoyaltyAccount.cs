using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages
{
    public class LoyaltyAccount:IMessage
    {
        public string LoyaltyEmail { get; set; }
    }
}
