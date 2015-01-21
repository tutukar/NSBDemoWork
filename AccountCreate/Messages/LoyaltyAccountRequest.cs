using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages
{
    public class LoyaltyAccountRequest : IMessage
    {
        public Guid MessageId { get; set; }
        public string LoyaltyEmail { get; set; }
    }
}
