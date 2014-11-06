using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages
{
    public class CreateAccount:IMessage
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
