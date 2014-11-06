using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Saga;

namespace AccountCreateEndpoints.SagaData
{
    public class AccountCreate:ContainSagaData
    {
        [Unique]
        public virtual string EMailAddress { get; set; }
        public virtual string ConfirmationCode { get; set; }
    }
}
