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
        public int PaymentType { get; set; }
        public string PaymentTypeName { get; set; }

        public bool UsePerStoreAccount { get; set; }
        public string? Account1 { get; set; }
        public string? StoreCode { get; set; }
        public string? Account2 { get; set; }
        public string? Account { get; set; }

       //public AccountTypes AccountType { get; set; }
    }

    public enum AccountTypes
    {
        IncomingPayments,
        OutgoingPayments,
    }

    public enum PaymentTypesEnum
    {
        Cash = 0,
        Deposit = 7,

        //CreditCards
        //Visa = 2,
        //MasterCard = 2,//  MC

        //Tamara = 2,//UDF1
        //Mada = 2,//UDF2

        //Emkan = 2,//UDF3
        //Check = 2,
        BankTransfer = 3,
        //Return = 2,
        //DownPayment = 4,
        //CentralGiftCard = 15,
        //CentralCredit = 17,
    }

    public class PaymentTypes
    {
        //public static string Cash = "cash";
        //public static string CashArabic = "كاش";
        public static string Visa = "Visa";
        public static string MasterCard = "MC";
        public static string Tamara = "UDF1";
        public static string Mada = "UDF2";
        public static string Emkan = "UDF3";
        public static string Return = "UDF4";
        public static string Tabby = "UDF6";
        public static string AmericanExpress = "AmEx";

        public static string Cash = "0";
        public static string Deposit = "7";
        public static string BankTransfer = "3";
    }
}
