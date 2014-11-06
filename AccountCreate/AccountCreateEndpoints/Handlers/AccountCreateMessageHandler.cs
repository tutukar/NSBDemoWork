using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace AccountCreateEndpoints.Handlers
{
    class AccountCreateMessageHandler:IHandleMessages<CreateAccount>
    {
        private IBus _myBus = null;
        public AccountCreateMessageHandler(IBus bus)
        {
            _myBus = bus;
        }
        public void Handle(CreateAccount message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Create account message recieved for: " + message.Email);
            //Go create an account
            //Let other systems know about the new account
            Console.WriteLine("Account created. Publishing event.");
            Console.ResetColor();
            _myBus.Publish<AccountCreated>(m => m.EmailAddress = message.Email);
           
        }
    }
}
