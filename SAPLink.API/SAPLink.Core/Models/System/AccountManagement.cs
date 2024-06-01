using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPLink.Core.Models.System
{
    public class AccountManagement
    {
        public int Id { get; set; }
        public string PaymentTypeCode { get; set; }
        public string PaymentTypeName { get; set; }

        public string Account { get; set; }
        public AccountTypes AccountType { get; set; }
    }

    public enum AccountTypes
    {
        IncomingPayments,
        OutgoingPayments,
    }
}
