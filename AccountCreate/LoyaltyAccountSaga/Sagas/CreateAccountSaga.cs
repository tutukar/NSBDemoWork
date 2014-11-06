using System;
using AccountCreateEndpoints.SagaData;
using Messages;
using Microsoft.Win32;
using NServiceBus;
using NServiceBus.Saga;
using Remotion.Linq.Parsing;

namespace LoyaltyAccountSaga.Sagas
{
    public class CreateAccountSaga:Saga<AccountCreate>,
        IAmStartedByMessages<LoyaltyAccount>,
        IHandleMessages<ConfirmAccount>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<AccountCreate> mapper)
        {
            mapper.ConfigureMapping<ConfirmAccount>(s => s.EmailAddress).ToSaga(m => m.EMailAddress);
            
        }

        public void Handle(LoyaltyAccount message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Recieved new loyalty account.");
            Console.WriteLine("Sending confirmation email.");
            Console.WriteLine("Waiting for confirmation.......");
            Console.ResetColor();
            Data.EMailAddress = message.LoyaltyEmail;
            //Encrypt the password using 3DES
            Data.ConfirmationCode = Guid.NewGuid().ToString();
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
