using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages
{
    public class ConfirmAccount:IMessage
    {
        public string EmailAddress { get; set; }

        public string ConfirmationCode { get; set; }
        public string Nonce { get; set; }
    }
}
