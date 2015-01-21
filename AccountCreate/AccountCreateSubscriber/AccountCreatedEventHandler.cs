using System;
using Messages;
using NServiceBus;

namespace AccountCreateSubscriber
{
    class AccountCreatedEventHandler:IHandleMessages<AccountCreated>
    {
        private IBus _myBus = null;

        public AccountCreatedEventHandler(IBus bus)
        {
            _myBus = bus;

        }
        public void Handle(AccountCreated message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Recieved account created: " + message.EmailAddress);
            Console.WriteLine("Sending account data to loyalty system");
            Console.ResetColor();
            _myBus.Send(new LoyaltyAccount {LoyaltyEmail = message.EmailAddress});
            
        }
    }
}
