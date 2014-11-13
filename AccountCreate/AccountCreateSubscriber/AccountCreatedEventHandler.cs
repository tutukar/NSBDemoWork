using System;
using System.Collections.Generic;
using Messages;
using NServiceBus;
using Utilities;

namespace AccountCreateSubscriber
{
    class AccountCreatedEventHandler : IHandleMessages<AccountCreated>
    {
        private IBus _myBus = null;

        public AccountCreatedEventHandler(IBus bus)
        {
            _myBus = bus;
        }
        public void Handle(AccountCreated message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Recieved account created: {0}", message.EmailAddress);

            var messageId = Guid.NewGuid();

            MessageTracker<string>.Instance.TimeoutInterval = new TimeSpan(0, 0, 10);
            MessageTracker<string>.Instance.MessageTimeout += MessageTracker_MessageTimeout;
            MessageTracker<string>.Instance.Track(messageId, message.EmailAddress);
            
            Console.WriteLine("Sending loyalty account request {0}", messageId);
            Console.ResetColor();
            _myBus.Send(new LoyaltyAccountRequest()
            {
                MessageId = messageId,
                LoyaltyEmail = message.EmailAddress,
            }); 
        }

        private void MessageTracker_MessageTimeout(object sender, MessageTrackerEventArgs<string> e)
        {
            Console.WriteLine("Loyalty account request timed out {0} : {1}", e.MessageId, e.Data);
        }
    }
}
