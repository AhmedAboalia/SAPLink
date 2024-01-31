using System.Text;
using SAPbobsCOM;
using SAPLink.Application.SAP.Application;
using SAPLink.Domain;
using SAPLink.Domain.Common;
using SAPLink.Domain.Models.SAP.Sales;
using PrismInvoice = SAPLink.Domain.Models.Prism.Sales.Invoice;

namespace SAPLink.Application.SAP.Handlers;

public static class IncomingPayment 
{
    private static RequestResult<Payment> SyncSinglePayment(PrismInvoice invoice, string account, double amount, string docEntry, string customerCode, BoRcptInvTypes invoiceType)
    {
        var result = new RequestResult<Payment>();

        var oPayment = (Payments)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oIncomingPayments); 

        oPayment.CardCode = customerCode;

        if (invoiceType == BoRcptInvTypes.it_Invoice)
        {
            oPayment.Invoices.DocEntry = int.Parse(docEntry);
            oPayment.Invoices.InvoiceType = invoiceType;
            oPayment.Invoices.SumApplied = amount;
            oPayment.CashAccount = account;
            oPayment.CashSum = amount;
        }
        else
        {

            oPayment.DocTypte = SAPbobsCOM.BoRcptTypes.rCustomer;
            oPayment.CashAccount = account;
            oPayment.CashSum = amount;

            //oPayment.AccountPayments.SumPaid = amount;
            //oPayment.CashSum = amount;
            //oPayment.AccountPayments.AccountCode = ClientHandler.GetFieldValueByQuery($"SELECT DPmClear FROM OCRD WHERE CardCode = '{customerCode}'");
        }
        
        oPayment.TaxDate = invoice.CreatedDatetime;
        oPayment.DocDate = invoice.CreatedDatetime;
        oPayment.DocDate = invoice.CreatedDatetime;
        oPayment.Invoices.Add();

        var response = oPayment.Add();

        if (response != 0)
        {
            result.Message = ClientHandler.Company.GetLastErrorDescription();
            result.StatusBarMessage = ClientHandler.Company.GetLastErrorDescription();
            result.Status = Enums.StatusType.Failed;
        }
        else
        {
            result.Message = "Incoming payment added successfully";
            result.StatusBarMessage = "Incoming payment added successfully";
            result.Status = Enums.StatusType.Success;
        }

        return result;
    }
    public static RequestResult<Payment>AddMultiplePaymentsInvoice(PrismInvoice invoice, string docEntry, string CustomerID, BoRcptInvTypes invoiceType)
    {
        var results = new List<RequestResult<Payment>>();
        var combinedResult = new RequestResult<Payment>();

        var paymentList = new Dictionary<string, double>();

        var tenderData = "";
        foreach (var tender in invoice.Tenders)
        {
            if (tender.TenderName == null ||
                tender.TenderName.ToLower() == Enums.PaymentTypes.Visa.ToLower() ||
                tender.TenderName.ToLower() == Enums.PaymentTypes.Mada.ToLower() ||
                tender.TenderName.ToLower() == Enums.PaymentTypes.MasterCard.ToLower() ||
                tender.TenderName.ToLower() == Enums.PaymentTypes.Return.ToLower() ||

                //tender.TenderType == (int)Enums.PaymentTypesEnum.Visa ||
                //tender.TenderType == (int)Enums.PaymentTypesEnum.Mada ||
                //tender.TenderType == (int)Enums.PaymentTypesEnum.MasterCard ||
                //tender.TenderType == (int)Enums.PaymentTypesEnum.Return ||

                tender.TenderType == (int)Enums.PaymentTypesEnum.Cash ||
                tender.TenderType == (int)Enums.PaymentTypesEnum.Deposit ||
                tender.TenderType == (int)Enums.PaymentTypesEnum.BankTransfer
            )
            {
                //var accountCode = "160000"; //Local
                var accountCode = ClientHandler.GetAccountCode($"1101{invoice.StoreCode}0100");

                if (paymentList.ContainsKey(accountCode))
                    paymentList[accountCode] += tender.Amount;
                else
                    paymentList.Add(accountCode, tender.Amount);
            }

            else if (tender.TenderName.ToLower() == Enums.PaymentTypes.Tamara.ToLower() ||
                     tender.TenderName.ToLower() == Enums.PaymentTypes.Emkan.ToLower())
            {
                //var accountCode = "160000"; //Local
                var accountCode = ClientHandler.GetAccountCode("11180050100");

                if (paymentList.ContainsKey(accountCode))
                    paymentList[accountCode] += tender.Amount;
                else
                    paymentList.Add(accountCode, tender.Amount);
            }

            //tenderData += $"Tender Name: {tender.TenderName} - Amount: ({tender.Amount})";
        }

        foreach (KeyValuePair<string, double> entry in paymentList)
        {
            string account = entry.Key;
            double amount = entry.Value;

            results.Add(SyncSinglePayment(invoice,  account, amount , docEntry, CustomerID, invoiceType));
        }

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
            combinedResult.Message = "All incoming payments added successfully.\r\n\r\n" + tenderData;
            combinedResult.StatusBarMessage = "All incoming payments added successfully.";
        }
        else
        {
            combinedResult.Status = Enums.StatusType.Failed;
            combinedResult.Message = $"{combinedMessage}\r\n\r\n {tenderData}";
            combinedResult.StatusBarMessage = "One or more payments failed. Check the message for details.";
        }

        return combinedResult;
    }

    public static RequestResult<Payment> AddPayment(PrismInvoice invoice)
    {
        var results = new List<RequestResult<Payment>>();
        var combinedResult = new RequestResult<Payment>();

        var amounts = 0.00;
        var account = ClientHandler.GetAccountCode($"2112{invoice.StoreCode}0100");


        foreach (var tender in invoice.Tenders)
        {
            //var accountCode = "160000"; //Local
            amounts += tender.Amount;
        }

        results.Add(SyncSinglePayment(invoice, account, amounts));

        var allSuccess = true;
        var combinedMessage = new StringBuilder();

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
            combinedResult.Message = "Incoming payment added successfully.";
            combinedResult.StatusBarMessage = "Incoming payment added successfully.";
        }
        else
        {
            combinedResult.Status = Enums.StatusType.Failed;
            combinedResult.Message = combinedMessage.ToString();
            combinedResult.StatusBarMessage = "Payment failed. Check the message for details.";
        }

        return combinedResult;
    }

    private static RequestResult<Payment> SyncSinglePayment(PrismInvoice invoice, string account, double amount)
    {
        var result = new RequestResult<Payment>();

        var oPayment = (Payments)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oIncomingPayments);

        oPayment.CardCode = invoice.CustomerID;
        oPayment.CashAccount = account;
        oPayment.CashSum = amount;
        oPayment.Remarks = $"PRISM Deposit ({invoice.DocumentNumber}).";
        var response = oPayment.Add();

        if (response != 0)
        {
            result.Message = ClientHandler.Company.GetLastErrorDescription();
            result.StatusBarMessage = ClientHandler.Company.GetLastErrorDescription();
            result.Status = Enums.StatusType.Failed;
        }
        else
        {
            result.Message = "Incoming payment added successfully";
            result.StatusBarMessage = "Incoming payment added successfully";
            result.Status = Enums.StatusType.Success;
        }

        return result;
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

    //    incomingPayment.CardCode = invoice.customerCode;
    //    var cashFlowAssignments = new List<CashFlowAssignment>();
    //    //var paymentCreditCards = new List<PaymentCreditCard>();
    //    //var paymentInvoices = new List<PaymentInvoice>();

    //    var cashSum = 0.00;
    //    foreach (var tender in invoice.Tenders)
    //    {
    //        cashSum += tender.Amount;

    //        if  (tender.TenderName == "كاش" || 
    //            tender.TenderName.ToLower() == "cash" || 
    //            tender.TenderName == "MC"||
    //            tender.TenderName == "Visa" ||
    //            tender.TenderName == "UDF2" 
    //            )
    //        {
    //           // var accountCode  = "160000"; //Local

    //            var accountCode = $"1101{invoice.StoreCode}0100";

    //            incomingPayment.CashAccount = accountCode;
    //            incomingPayment.CashAccount = ClientHandler.GetAccountCode(accountCode);

    //            var cashFlow = new CashFlowAssignment
    //            {
    //                AmountLc = tender.Amount,
    //                PaymentMeans = "pmtCash"
    //            };
    //            cashFlowAssignments.Add(cashFlow);
    //        }
    //        else if (tender.TenderName == "UDF1" || tender.TenderName == "UDF3")
    //        {
    //            //incomingPayment.CashAccount = "160000"; //Local

    //            incomingPayment.CashAccount = "11180050100";

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