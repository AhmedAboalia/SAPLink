using System.Text;
using SAPbobsCOM;
using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.SAP.Sales;
using SAPLink.Core.Utilities;
using SAPLink.Handler.SAP.Application;
using SAPLink.Handler.SAP.Connection;
using HttpClientFactory = SAPLink.Handler.Connection.HttpClientFactory;
using PrismInvoice = SAPLink.Core.Models.Prism.Sales.Invoice;

namespace SAPLink.Handler.SAP.Handlers;

public static class OutgoingPayment
{
    private static RequestResult<Payment> SyncSinglePayment(PrismInvoice invoice, string account, double amount, string entry, string customerCode)
    {
        var result = new RequestResult<Payment>();

        var oPayment = (Payments)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oVendorPayments);
        oPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_OutgoingPayments;
        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
        oPayment.TaxDate = invoice.CreatedDatetime;
        oPayment.DocDate = invoice.CreatedDatetime;
        oPayment.DocDate = invoice.CreatedDatetime;
       

        oPayment.CardCode = customerCode;

        oPayment.Invoices.DocEntry = int.Parse(entry);
        oPayment.Invoices.InvoiceType = BoRcptInvTypes.it_CredItnote;
        oPayment.Invoices.SumApplied = amount;
        oPayment.CashAccount = account;
        oPayment.CashSum = amount;
        oPayment.Invoices.Add();

        var response = oPayment.Add();

        if (response != 0)
        {
            result.Message += ClientHandler.Company.GetLastErrorDescription();
            result.StatusBarMessage = ClientHandler.Company.GetLastErrorDescription();
            result.Status = Enums.StatusType.Failed;
        }
        else
        {
            result.Message += "Outgoing payment added successfully";
            result.StatusBarMessage = "Outgoing payment added successfully";
            result.Status = Enums.StatusType.Success;
        }

        return result;
    }
    public static RequestResult<Payment> AddMultiplePaymentsInvoice(PrismInvoice invoice, string docEntry, string customerCode)
    {
        var results = new List<RequestResult<Payment>>();
        var combinedResult = new RequestResult<Payment>();

        var paymentList = new Dictionary<string, double>();
        var tenderData = "";
        foreach (var tender in invoice.Tenders)
        {
            if (tender.TenderType == (int)Enums.PaymentTypesEnum.Cash ||
                tender.TenderName.ToLower() == Enums.PaymentTypes.Return.ToLower() ||
                tender.TenderType == (int)Enums.PaymentTypesEnum.Deposit ||
                tender.TenderType == (int)Enums.PaymentTypesEnum.BankTransfer
            )
            {
                //var accountCode = "160000"; //Local
                var accountCode = ClientHandler.GetAccountCode($"1101{invoice.StoreCode}0100");

                if (paymentList.ContainsKey(accountCode))
                    paymentList[accountCode] -= tender.Amount;
                else
                    paymentList.Add(accountCode, -tender.Amount);
            }
            tenderData += $" TenderName: {tender.TenderName} - Amount:{tender.Amount}\r\n ";
        }

        foreach (KeyValuePair<string, double> entry in paymentList)
        {
            string account = entry.Key;
            double amount = entry.Value;

            results.Add(SyncSinglePayment(invoice, account, amount, docEntry, customerCode));
        }

        // Combine results
        bool allSuccess = true;
        StringBuilder combinedMessage = new StringBuilder();

        foreach (var result in results)
        {
            if (result.Status == Enums.StatusType.Failed)
            {
                allSuccess = false;
            }
            combinedMessage.AppendLine(result.Message);
        }

        if (allSuccess)
        {
            combinedResult.Status = Enums.StatusType.Success;
            combinedResult.Message = "All Outgoing payments added successfully.";
            combinedResult.StatusBarMessage = "All Outgoing payments added successfully.";
        }
        else
        {
            combinedResult.Status = Enums.StatusType.Failed;
            combinedResult.Message = combinedMessage.ToString();
            combinedResult.StatusBarMessage = "One or more payments failed. Check the message for details.";
        }

        return combinedResult;
    }


    //public RequestResult<Payment> SyncOld(PrismInvoice invoice, string SAPInvoiceNo)
    //{
    //    var result = new RequestResult<Payment>();
    //    var body = "";
    //    try
    //    {
    //        if (invoice.Tenders.Count > 0)
    //        {
    //            body = CreateBody(invoice, SAPInvoiceNo);


    //            result.Response = HttpClientFactory.Initialize("IncomingPayments", Method.POST, LoginModel.LoginTypes.Basic, null, body);

    //            if (result.Response.StatusCode == HttpStatusCode.OK || result.Response.StatusCode == HttpStatusCode.Created)
    //            {
    //                var odata = JsonConvert.DeserializeObject<Payment>(result.Response.Content);

    //                if (odata != null && odata != null) 
    //                {
    //                    result.EntityList.Add(odata);
    //                    result.Message = $"Incoming Payment {odata.DocDate} is Added. \r\n Body: \r\n {body} \r\n Response \r\n {result.Response.Content}";
    //                    result.StatusBarMessage = $"Incoming Payment {odata.DocDate} is Added.";
    //                    result.Status = Enums.StatusType.Success;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            result.Message = $"There is no available Incoming Payment for Doc. No. {invoice.DocumentNumber}.";
    //            result.StatusBarMessage = $"No available Incoming Payment for Doc. No. {invoice.DocumentNumber}. \r\n Body: \r\n {body} \r\n Response \r\n {result.Response.Content}"; ;
    //            result.Status = Enums.StatusType.NotFound;
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        result.Message = $"Cant add Incoming Payment  \r\n Body: \r\n {body} \r\n Exception: {e.Message}\r\n Response \r\n {result.Response.Content}";
    //        result.StatusBarMessage = "Cant add  Incoming Payment.";
    //        result.Status = Enums.StatusType.Failed;
    //    }

    //    if (result.Message.IsNullOrEmpty())
    //    {
    //        result.Message = $"Cant add Incoming Payment  \r\n Body: \r\n {body} \r\n \r\n Response \r\n {result.Response.Content}";
    //        result.StatusBarMessage = "Cant add  Incoming Payment.";
    //        result.Status = Enums.StatusType.Failed;
    //    }
    //    return result;
    //}

    //private string CreateBody(PrismInvoice invoice, string sapInvoiceNo)
    //{
    //    var incomingPayment = new Payment();

    //    incomingPayment.CardCode = invoice.CustomerID;
    //    var cashFlowAssignments = new List<CashFlowAssignment>();
    //    //var paymentCreditCards = new List<PaymentCreditCard>();
    //    //var paymentInvoices = new List<PaymentInvoice>();

    //    var cashSum = 0.00;
    //    foreach (var tender in invoice.Tenders)
    //    {
    //        cashSum += tender.Amount;

    //        if (tender.TenderType
    //             is (int)Enums.PaymentTypes.Cash
    //             or (int)Enums.PaymentTypes.Visa
    //             or (int)Enums.PaymentTypes.Mada
    //             or (int)Enums.PaymentTypes.MasterCard
    //             or (int)Enums.PaymentTypes.Check
    //             or (int)Enums.PaymentTypes.BankTransfer
    //             )
    //        {
    //            //var accountCode = "160000"; //Local

    //            var accountCode = ClientHandler.GetAccountCode($"1101{invoice.StoreCode}0100");;
    //            incomingPayment.CashAccount = accountCode;

    //            var cashFlow = new CashFlowAssignment
    //            {
    //                AmountLc = tender.Amount,
    //                PaymentMeans = "pmtCash"
    //            };
    //            cashFlowAssignments.Add(cashFlow);
    //        }
    //        else if (tender.TenderName == "UDF1" || tender.TenderName == "UDF3")
    //        {
    //            //var accountCode = "160000"; //Local

    //            var accountCode = ClientHandler.GetAccountCode("11180050100");

    //            incomingPayment.CashAccount = accountCode;

    //            var cashFlow = new CashFlowAssignment
    //            {
    //                AmountLc = tender.Amount,
    //                PaymentMeans = "pmtCash"
    //            };
    //            cashFlowAssignments.Add(cashFlow);
    //        }
    //        //if (tender.TenderType is (int)PaymentTypes.DownPayment)
    //        //{
    //        //    //incomingPayment.CashAccount = "160000";
    //        //    incomingPayment.CashAccount = $"2112{invoice.StoreCode}0100";
    //        //    var cashFlow = new CashFlowAssignment
    //        //    {
    //        //        AmountLc = tender.Amount,
    //        //        PaymentMeans = "pmtCash"
    //        //    };
    //        //    CashFlowAssignments.Add(cashFlow);
    //        //    CashSum += tender.Amount;
    //        //}
    //        #region Visa

    //        //if (tender.TenderType == (int)PaymentTypes.Visa)
    //        //{
    //        //    var paymentCreditCard = new PaymentCreditCard
    //        //    {
    //        //CreditAcct = "140090",

    //        //"PaymentCreditCards": [
    //        //{
    //        //    "CreditCard": 2,
    //        //    "CreditAcct": "140090",
    //        //    "CreditCardNumber": "3333",
    //        //    "CardValidUntil": "2024-10-31",
    //        //    "VoucherNum": "123",
    //        //    "OwnerIdNum": "102",
    //        //    "OwnerPhone": null,
    //        //    "PaymentMethodCode": 1,
    //        //    "NumOfPayments": 1,
    //        //    "FirstPaymentDue": "2023-08-26",
    //        //    "FirstPaymentSum": 100,
    //        //    "AdditionalPaymentSum": 0,
    //        //    "CreditSum": 100,
    //        //    "CreditCur": "GBP",
    //        //    "CreditRate": 0,
    //        //    "ConfirmationNum": null,
    //        //    "NumOfCreditPayments": 1,
    //        //    "CreditType": "cr_Regular",
    //        //    "SplitPayments": "tNO"
    //        //}
    //        //],
    //        //    };
    //        //    PaymentCreditCards.Add(paymentCreditCard);
    //        //}


    //        #endregion
    //    }

    //    incomingPayment.CashSum = cashSum;
    //    incomingPayment.CashFlowAssignments = cashFlowAssignments;
    //    incomingPayment.PaymentInvoices = new List<PaymentInvoice>
    //    {
    //      new ()
    //      {
    //        DocEntry = sapInvoiceNo,

    //      }
    //    };

    //    return incomingPayment.ToJson();
    //}
}