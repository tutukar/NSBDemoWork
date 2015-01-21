using System;
using System.Collections.Generic;
using Messages;
using NServiceBus;
using Utilities;

namespace AccountCreateSubscriber
{
    #region RequestResponse
    class LoyaltyAccountResponseHandler : IHandleMessages<LoyaltyAccountResponse>
    {
        private IBus _myBus = null;

        public LoyaltyAccountResponseHandler(IBus bus)
        {
            _myBus = bus;
        }
        public void Handle(LoyaltyAccountResponse message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Recieved loyalty account response for message {0}", message.MessageId);

            if (MessageTracker<string>.Instance.Exists(message.MessageId))
            {
                Console.WriteLine("Loyalty account create job submitted for email {0}", MessageTracker<string>.Instance.GetData(message.MessageId));
                MessageTracker<string>.Instance.StopTracking(message.MessageId);
            }
            else
            {
                Console.WriteLine("Recieved delayed response for loyalty account request message {0}", message.MessageId);
            } 

            Console.ResetColor();
        }
    } 
    #endregion
}
