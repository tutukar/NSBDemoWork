using System;
using AccountCreateEndpoints.SagaData;
using Messages;
using Microsoft.Win32;
using NServiceBus;
using NServiceBus.Saga;
using Remotion.Linq.Parsing;

namespace LoyaltyAccountSaga.Sagas
{
    public class CreateAccountSaga : Saga<AccountCreate>,
        IAmStartedByMessages<LoyaltyAccountRequest>,
        IHandleMessages<ConfirmAccount>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<AccountCreate> mapper)
        {
            mapper.ConfigureMapping<ConfirmAccount>(s => s.EmailAddress).ToSaga(m => m.EMailAddress);
        }
        
        public void Handle(LoyaltyAccountRequest message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Recieved loyalty account request {0}", message.MessageId);
            Data.EMailAddress = message.LoyaltyEmail;
            //Encrypt the password using 3DES
            Data.ConfirmationCode = Guid.NewGuid().ToString();
            Console.WriteLine("Sending confirmation email with verification code {0}", Data.ConfirmationCode);
            Console.WriteLine("Waiting for confirmation.......");
            
            Console.WriteLine("Sending loyalty account response {0}", message.MessageId);
            Console.ResetColor();
            Bus.Reply(new LoyaltyAccountResponse { MessageId = message.MessageId });
        }

        public void Handle(ConfirmAccount message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Email confirmation recieved.");
            Console.WriteLine("Checking...");
            if (message.ConfirmationCode == Data.ConfirmationCode)
            {
                Console.WriteLine("Confirmation code verified.");
                Console.WriteLine("Loyalty customer verified.");
                Console.ResetColor();
                MarkAsComplete();
            }
            //Use the account 

            Console.WriteLine();
        }
    }
}
