using SAPLink.Core.Utilities;
using PrismInvoice = SAPLink.Core.Models.Prism.Sales.Invoice;
using SAPInvoice = SAPLink.Core.Models.SAP.Sales.Invoice;

namespace SAPLink.Core.Models.SAP.Sales
{

    public partial class Payment
    {
        [JsonProperty("odata.metadata", NullValueHandling = NullValueHandling.Ignore)]
        public Uri OdataMetadata { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public List<Payment> Value { get; set; }

        [JsonProperty("odata.nextLink", NullValueHandling = NullValueHandling.Ignore)]
        public string OdataNextLink { get; set; }
    }
   
    public partial class Payment
    {
        [JsonProperty("DocNum", NullValueHandling = NullValueHandling.Ignore)]
        public long? DocNum { get; set; }

        [JsonProperty("DocType", NullValueHandling = NullValueHandling.Ignore)]
        public string DocType { get; set; }

        [JsonProperty("HandWritten", NullValueHandling = NullValueHandling.Ignore)]
        public string HandWritten { get; set; }

        [JsonProperty("Printed", NullValueHandling = NullValueHandling.Ignore)]
        public string Printed { get; set; }

        [JsonProperty("DocDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DocDate { get; set; }

        [JsonProperty("CardCode", NullValueHandling = NullValueHandling.Ignore)]
        public string CardCode { get; set; }

        [JsonProperty("CardName", NullValueHandling = NullValueHandling.Ignore)]
        public string CardName { get; set; }

        [JsonProperty("Address", NullValueHandling = NullValueHandling.Ignore)]
        public object Address { get; set; }

        [JsonProperty("CashAccount", NullValueHandling = NullValueHandling.Ignore)]
       // [JsonConverter(typeof(ParseStringConverter))]
        public string? CashAccount { get; set; }

        [JsonProperty("DocCurrency", NullValueHandling = NullValueHandling.Ignore)]
        public string DocCurrency { get; set; }

        [JsonProperty("CashSum", NullValueHandling = NullValueHandling.Ignore)]
        public double? CashSum { get; set; }

        [JsonProperty("CheckAccount", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string? CheckAccount { get; set; }

        [JsonProperty("TransferAccount", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long? TransferAccount { get; set; }

        [JsonProperty("TransferSum", NullValueHandling = NullValueHandling.Ignore)]
        public long? TransferSum { get; set; }

        [JsonProperty("TransferDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? TransferDate { get; set; }

        [JsonProperty("TransferReference", NullValueHandling = NullValueHandling.Ignore)]
        public object TransferReference { get; set; }

        [JsonProperty("LocalCurrency", NullValueHandling = NullValueHandling.Ignore)]
        public string LocalCurrency { get; set; }

        [JsonProperty("DocRate", NullValueHandling = NullValueHandling.Ignore)]
        public long? DocRate { get; set; }

        [JsonProperty("Reference1", NullValueHandling = NullValueHandling.Ignore)]
        ///[JsonConverter(typeof(ParseStringConverter))]
        public long? Reference1 { get; set; }

        [JsonProperty("Reference2", NullValueHandling = NullValueHandling.Ignore)]
        public object Reference2 { get; set; }

        [JsonProperty("CounterReference", NullValueHandling = NullValueHandling.Ignore)]
        public object CounterReference { get; set; }

        [JsonProperty("Remarks", NullValueHandling = NullValueHandling.Ignore)]
        public object Remarks { get; set; }

        [JsonProperty("JournalRemarks", NullValueHandling = NullValueHandling.Ignore)]
        public string JournalRemarks { get; set; }

        [JsonProperty("SplitTransaction", NullValueHandling = NullValueHandling.Ignore)]
        public string SplitTransaction { get; set; }

        [JsonProperty("ContactPersonCode", NullValueHandling = NullValueHandling.Ignore)]
        public object ContactPersonCode { get; set; }

        [JsonProperty("ApplyVAT", NullValueHandling = NullValueHandling.Ignore)]
        public string ApplyVat { get; set; }

        [JsonProperty("TaxDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? TaxDate { get; set; }

        [JsonProperty("Series", NullValueHandling = NullValueHandling.Ignore)]
        public long? Series { get; set; }

        [JsonProperty("BankCode", NullValueHandling = NullValueHandling.Ignore)]
        public object BankCode { get; set; }

        [JsonProperty("BankAccount", NullValueHandling = NullValueHandling.Ignore)]
        public object BankAccount { get; set; }

        [JsonProperty("DiscountPercent", NullValueHandling = NullValueHandling.Ignore)]
        public long? DiscountPercent { get; set; }

        [JsonProperty("ProjectCode", NullValueHandling = NullValueHandling.Ignore)]
        public object ProjectCode { get; set; }

        [JsonProperty("CurrencyIsLocal", NullValueHandling = NullValueHandling.Ignore)]
        public string CurrencyIsLocal { get; set; }

        [JsonProperty("DeductionPercent", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeductionPercent { get; set; }

        [JsonProperty("DeductionSum", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeductionSum { get; set; }

        [JsonProperty("CashSumFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? CashSumFc { get; set; }

        [JsonProperty("CashSumSys", NullValueHandling = NullValueHandling.Ignore)]
        public long? CashSumSys { get; set; }

        [JsonProperty("BoeAccount", NullValueHandling = NullValueHandling.Ignore)]
        public object BoeAccount { get; set; }

        [JsonProperty("BillOfExchangeAmount", NullValueHandling = NullValueHandling.Ignore)]
        public long? BillOfExchangeAmount { get; set; }

        [JsonProperty("BillofExchangeStatus", NullValueHandling = NullValueHandling.Ignore)]
        public object BillofExchangeStatus { get; set; }

        [JsonProperty("BillOfExchangeAmountFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? BillOfExchangeAmountFc { get; set; }

        [JsonProperty("BillOfExchangeAmountSC", NullValueHandling = NullValueHandling.Ignore)]
        public long? BillOfExchangeAmountSc { get; set; }

        [JsonProperty("BillOfExchangeAgent", NullValueHandling = NullValueHandling.Ignore)]
        public object BillOfExchangeAgent { get; set; }

        [JsonProperty("WTCode", NullValueHandling = NullValueHandling.Ignore)]
        public object WtCode { get; set; }

        [JsonProperty("WTAmount", NullValueHandling = NullValueHandling.Ignore)]
        public long? WtAmount { get; set; }

        [JsonProperty("WTAmountFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? WtAmountFc { get; set; }

        [JsonProperty("WTAmountSC", NullValueHandling = NullValueHandling.Ignore)]
        public long? WtAmountSc { get; set; }

        [JsonProperty("WTAccount", NullValueHandling = NullValueHandling.Ignore)]
        public object WtAccount { get; set; }

        [JsonProperty("WTTaxableAmount", NullValueHandling = NullValueHandling.Ignore)]
        public long? WtTaxableAmount { get; set; }

        [JsonProperty("Proforma", NullValueHandling = NullValueHandling.Ignore)]
        public string Proforma { get; set; }

        [JsonProperty("PayToBankCode", NullValueHandling = NullValueHandling.Ignore)]
        public object PayToBankCode { get; set; }

        [JsonProperty("PayToBankBranch", NullValueHandling = NullValueHandling.Ignore)]
        public object PayToBankBranch { get; set; }

        [JsonProperty("PayToBankAccountNo", NullValueHandling = NullValueHandling.Ignore)]
        public object PayToBankAccountNo { get; set; }

        [JsonProperty("PayToCode", NullValueHandling = NullValueHandling.Ignore)]
        public object PayToCode { get; set; }

        [JsonProperty("PayToBankCountry", NullValueHandling = NullValueHandling.Ignore)]
        public object PayToBankCountry { get; set; }

        [JsonProperty("IsPayToBank", NullValueHandling = NullValueHandling.Ignore)]
        public string IsPayToBank { get; set; }

        [JsonProperty("DocEntry", NullValueHandling = NullValueHandling.Ignore)]
        public long? DocEntry { get; set; }

        [JsonProperty("PaymentPriority", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentPriority { get; set; }

        [JsonProperty("TaxGroup", NullValueHandling = NullValueHandling.Ignore)]
        public object TaxGroup { get; set; }

        [JsonProperty("BankChargeAmount", NullValueHandling = NullValueHandling.Ignore)]
        public long? BankChargeAmount { get; set; }

        [JsonProperty("BankChargeAmountInFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? BankChargeAmountInFc { get; set; }

        [JsonProperty("BankChargeAmountInSC", NullValueHandling = NullValueHandling.Ignore)]
        public long? BankChargeAmountInSc { get; set; }

        [JsonProperty("UnderOverpaymentdifference", NullValueHandling = NullValueHandling.Ignore)]
        public long? UnderOverpaymentdifference { get; set; }

        [JsonProperty("UnderOverpaymentdiffSC", NullValueHandling = NullValueHandling.Ignore)]
        public long? UnderOverpaymentdiffSc { get; set; }

        [JsonProperty("WtBaseSum", NullValueHandling = NullValueHandling.Ignore)]
        public long? WtBaseSum { get; set; }

        [JsonProperty("WtBaseSumFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? WtBaseSumFc { get; set; }

        [JsonProperty("WtBaseSumSC", NullValueHandling = NullValueHandling.Ignore)]
        public long? WtBaseSumSc { get; set; }

        [JsonProperty("VatDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? VatDate { get; set; }

        [JsonProperty("TransactionCode", NullValueHandling = NullValueHandling.Ignore)]
        public string TransactionCode { get; set; }

        [JsonProperty("PaymentType", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentType { get; set; }

        [JsonProperty("TransferRealAmount", NullValueHandling = NullValueHandling.Ignore)]
        public long? TransferRealAmount { get; set; }

        [JsonProperty("DocObjectCode", NullValueHandling = NullValueHandling.Ignore)]
        public string DocObjectCode { get; set; }

        [JsonProperty("DocTypte", NullValueHandling = NullValueHandling.Ignore)]
        public string DocTypte { get; set; }

        [JsonProperty("DueDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DueDate { get; set; }

        [JsonProperty("LocationCode", NullValueHandling = NullValueHandling.Ignore)]
        public object LocationCode { get; set; }

        [JsonProperty("Cancelled", NullValueHandling = NullValueHandling.Ignore)]
        public string Cancelled { get; set; }

        [JsonProperty("ControlAccount", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long? ControlAccount { get; set; }

        [JsonProperty("UnderOverpaymentdiffFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? UnderOverpaymentdiffFc { get; set; }

        [JsonProperty("AuthorizationStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthorizationStatus { get; set; }

        [JsonProperty("BPLID", NullValueHandling = NullValueHandling.Ignore)]
        public object Bplid { get; set; }

        [JsonProperty("BPLName", NullValueHandling = NullValueHandling.Ignore)]
        public object BplName { get; set; }

        [JsonProperty("VATRegNum", NullValueHandling = NullValueHandling.Ignore)]
        public object VatRegNum { get; set; }

        [JsonProperty("BlanketAgreement", NullValueHandling = NullValueHandling.Ignore)]
        public object BlanketAgreement { get; set; }

        [JsonProperty("PaymentByWTCertif", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentByWtCertif { get; set; }

        [JsonProperty("Cig", NullValueHandling = NullValueHandling.Ignore)]
        public object Cig { get; set; }

        [JsonProperty("Cup", NullValueHandling = NullValueHandling.Ignore)]
        public object Cup { get; set; }

        [JsonProperty("AttachmentEntry", NullValueHandling = NullValueHandling.Ignore)]
        public object AttachmentEntry { get; set; }

        [JsonProperty("SignatureInputMessage", NullValueHandling = NullValueHandling.Ignore)]
        public object SignatureInputMessage { get; set; }

        [JsonProperty("SignatureDigest", NullValueHandling = NullValueHandling.Ignore)]
        public object SignatureDigest { get; set; }

        [JsonProperty("CertificationNumber", NullValueHandling = NullValueHandling.Ignore)]
        public object CertificationNumber { get; set; }

        [JsonProperty("PrivateKeyVersion", NullValueHandling = NullValueHandling.Ignore)]
        public object PrivateKeyVersion { get; set; }

        [JsonProperty("PaymentChecks", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaymentCheck> PaymentChecks { get; set; }

        [JsonProperty("PaymentInvoices", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaymentInvoice> PaymentInvoices { get; set; }

        [JsonProperty("PaymentCreditCards", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaymentCreditCard> PaymentCreditCards { get; set; }

        [JsonProperty("PaymentAccounts", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> PaymentAccounts { get; set; }

        [JsonProperty("PaymentDocumentReferencesCollection", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> PaymentDocumentReferencesCollection { get; set; }

        [JsonProperty("BillOfExchange", NullValueHandling = NullValueHandling.Ignore)]
        public BillOfExchange BillOfExchange { get; set; }

        [JsonProperty("WithholdingTaxCertificatesCollection", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> WithholdingTaxCertificatesCollection { get; set; }

        [JsonProperty("ElectronicProtocols", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> ElectronicProtocols { get; set; }

        [JsonProperty("CashFlowAssignments", NullValueHandling = NullValueHandling.Ignore)]
        public List<CashFlowAssignment> CashFlowAssignments { get; set; }

        [JsonProperty("Payments_ApprovalRequests", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> PaymentsApprovalRequests { get; set; }

        [JsonProperty("WithholdingTaxDataWTXCollection", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> WithholdingTaxDataWtxCollection { get; set; }
    }

    public partial class BillOfExchange
    {
    }

    public partial class CashFlowAssignment
    {
        [JsonProperty("CashFlowAssignmentsID", NullValueHandling = NullValueHandling.Ignore)]
        public long? CashFlowAssignmentsId { get; set; }

        [JsonProperty("CashFlowLineItemID", NullValueHandling = NullValueHandling.Ignore)]
        public long? CashFlowLineItemId { get; set; }

        [JsonProperty("Credit", NullValueHandling = NullValueHandling.Ignore)]
        public long? Credit { get; set; }

        [JsonProperty("PaymentMeans", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentMeans { get; set; }

        [JsonProperty("CheckNumber", NullValueHandling = NullValueHandling.Ignore)]
        public object CheckNumber { get; set; }

        [JsonProperty("AmountLC", NullValueHandling = NullValueHandling.Ignore)]
        public double? AmountLc { get; set; }

        [JsonProperty("AmountFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? AmountFc { get; set; }

        [JsonProperty("JDTLineId", NullValueHandling = NullValueHandling.Ignore)]
        public long? JdtLineId { get; set; }
    }

    public partial class PaymentCheck
    {
        [JsonProperty("LineNum", NullValueHandling = NullValueHandling.Ignore)]
        public long? LineNum { get; set; }

        [JsonProperty("DueDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DueDate { get; set; }

        [JsonProperty("CheckNumber", NullValueHandling = NullValueHandling.Ignore)]
        public long? CheckNumber { get; set; }

        [JsonProperty("BankCode", NullValueHandling = NullValueHandling.Ignore)]
        public string BankCode { get; set; }

        [JsonProperty("Branch", NullValueHandling = NullValueHandling.Ignore)]
        public object Branch { get; set; }

        [JsonProperty("AccounttNum", NullValueHandling = NullValueHandling.Ignore)]
        public object AccounttNum { get; set; }

        [JsonProperty("Details", NullValueHandling = NullValueHandling.Ignore)]
        public object Details { get; set; }

        [JsonProperty("Trnsfrable", NullValueHandling = NullValueHandling.Ignore)]
        public string Trnsfrable { get; set; }

        [JsonProperty("CheckSum", NullValueHandling = NullValueHandling.Ignore)]
        public double? CheckSum { get; set; }

        [JsonProperty("Currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty("CountryCode", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCode { get; set; }

        [JsonProperty("CheckAbsEntry", NullValueHandling = NullValueHandling.Ignore)]
        public long? CheckAbsEntry { get; set; }

        [JsonProperty("CheckAccount", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long? CheckAccount { get; set; }

        [JsonProperty("ManualCheck", NullValueHandling = NullValueHandling.Ignore)]
        public string ManualCheck { get; set; }

        [JsonProperty("FiscalID", NullValueHandling = NullValueHandling.Ignore)]
        public object FiscalId { get; set; }

        [JsonProperty("OriginallyIssuedBy", NullValueHandling = NullValueHandling.Ignore)]
        public object OriginallyIssuedBy { get; set; }

        [JsonProperty("Endorse", NullValueHandling = NullValueHandling.Ignore)]
        public string Endorse { get; set; }

        [JsonProperty("EndorsableCheckNo", NullValueHandling = NullValueHandling.Ignore)]
        public object EndorsableCheckNo { get; set; }

        [JsonProperty("ECheck", NullValueHandling = NullValueHandling.Ignore)]
        public string ECheck { get; set; }
    }

    public partial class PaymentCreditCard
    {
        [JsonProperty("LineNum", NullValueHandling = NullValueHandling.Ignore)]
        public long? LineNum { get; set; }

        [JsonProperty("CreditCard", NullValueHandling = NullValueHandling.Ignore)]
        public long? CreditCard { get; set; }

        [JsonProperty("CreditAcct", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public string? CreditAcct { get; set; }

        [JsonProperty("CreditCardNumber", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long? CreditCardNumber { get; set; }

        [JsonProperty("CardValidUntil", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? CardValidUntil { get; set; }

        [JsonProperty("VoucherNum", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long? VoucherNum { get; set; }

        [JsonProperty("OwnerIdNum", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long? OwnerIdNum { get; set; }

        [JsonProperty("OwnerPhone", NullValueHandling = NullValueHandling.Ignore)]
        public object OwnerPhone { get; set; }

        [JsonProperty("PaymentMethodCode", NullValueHandling = NullValueHandling.Ignore)]
        public long? PaymentMethodCode { get; set; }

        [JsonProperty("NumOfPayments", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumOfPayments { get; set; }

        [JsonProperty("FirstPaymentDue", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? FirstPaymentDue { get; set; }

        [JsonProperty("FirstPaymentSum", NullValueHandling = NullValueHandling.Ignore)]
        public long? FirstPaymentSum { get; set; }

        [JsonProperty("AdditionalPaymentSum", NullValueHandling = NullValueHandling.Ignore)]
        public long? AdditionalPaymentSum { get; set; }

        [JsonProperty("CreditSum", NullValueHandling = NullValueHandling.Ignore)]
        public long? CreditSum { get; set; }

        [JsonProperty("CreditCur", NullValueHandling = NullValueHandling.Ignore)]
        public string CreditCur { get; set; }

        [JsonProperty("CreditRate", NullValueHandling = NullValueHandling.Ignore)]
        public long? CreditRate { get; set; }

        [JsonProperty("ConfirmationNum", NullValueHandling = NullValueHandling.Ignore)]
        public object ConfirmationNum { get; set; }

        [JsonProperty("NumOfCreditPayments", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumOfCreditPayments { get; set; }

        [JsonProperty("CreditType", NullValueHandling = NullValueHandling.Ignore)]
        public string CreditType { get; set; }

        [JsonProperty("SplitPayments", NullValueHandling = NullValueHandling.Ignore)]
        public string SplitPayments { get; set; }
    }

    public partial class PaymentInvoice
    {
        [JsonProperty("LineNum", NullValueHandling = NullValueHandling.Ignore)]
        public long? LineNum { get; set; }

        [JsonProperty("DocEntry", NullValueHandling = NullValueHandling.Ignore)]
        public string? DocEntry { get; set; }

        [JsonProperty("SumApplied", NullValueHandling = NullValueHandling.Ignore)]
        public double? SumApplied { get; set; }

        [JsonProperty("AppliedFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? AppliedFc { get; set; }

        [JsonProperty("AppliedSys", NullValueHandling = NullValueHandling.Ignore)]
        public double? AppliedSys { get; set; }

        [JsonProperty("DocRate", NullValueHandling = NullValueHandling.Ignore)]
        public long? DocRate { get; set; }

        [JsonProperty("DocLine", NullValueHandling = NullValueHandling.Ignore)]
        public long? DocLine { get; set; }

        [JsonProperty("InvoiceType", NullValueHandling = NullValueHandling.Ignore)]
        public string InvoiceType { get; set; }

        [JsonProperty("DiscountPercent", NullValueHandling = NullValueHandling.Ignore)]
        public long? DiscountPercent { get; set; }

        [JsonProperty("PaidSum", NullValueHandling = NullValueHandling.Ignore)]
        public long? PaidSum { get; set; }

        [JsonProperty("InstallmentId", NullValueHandling = NullValueHandling.Ignore)]
        public long? InstallmentId { get; set; }

        [JsonProperty("WitholdingTaxApplied", NullValueHandling = NullValueHandling.Ignore)]
        public long? WitholdingTaxApplied { get; set; }

        [JsonProperty("WitholdingTaxAppliedFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? WitholdingTaxAppliedFc { get; set; }

        [JsonProperty("WitholdingTaxAppliedSC", NullValueHandling = NullValueHandling.Ignore)]
        public long? WitholdingTaxAppliedSc { get; set; }

        [JsonProperty("LinkDate", NullValueHandling = NullValueHandling.Ignore)]
        public object LinkDate { get; set; }

        [JsonProperty("DistributionRule", NullValueHandling = NullValueHandling.Ignore)]
        public object DistributionRule { get; set; }

        [JsonProperty("DistributionRule2", NullValueHandling = NullValueHandling.Ignore)]
        public object DistributionRule2 { get; set; }

        [JsonProperty("DistributionRule3", NullValueHandling = NullValueHandling.Ignore)]
        public object DistributionRule3 { get; set; }

        [JsonProperty("DistributionRule4", NullValueHandling = NullValueHandling.Ignore)]
        public object DistributionRule4 { get; set; }

        [JsonProperty("DistributionRule5", NullValueHandling = NullValueHandling.Ignore)]
        public object DistributionRule5 { get; set; }

        [JsonProperty("TotalDiscount", NullValueHandling = NullValueHandling.Ignore)]
        public double? TotalDiscount { get; set; }

        [JsonProperty("TotalDiscountFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? TotalDiscountFc { get; set; }

        [JsonProperty("TotalDiscountSC", NullValueHandling = NullValueHandling.Ignore)]
        public double? TotalDiscountSc { get; set; }
    }

    public enum ApplyVat { TYes };

    public enum AuthorizationStatus { PasWithout };

    public enum Cancelled { TNo };

    public enum DocCurrency { Eur, Gbp, Usd };

    public enum DocObjectCode { BopotIncomingPayments };

    public enum DocTyp { RCustomer };

    public enum PayToCode { BillTo };

    public enum InvoiceType { ItInvoice };

    public enum PaymentPriority { BoppPriority6 };

    public enum PaymentType { BoptNone };

    public partial class Payment
    {
        public static Payment FromJson(string json) => JsonConvert.DeserializeObject<Payment>(json, Converter.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson(this Payment self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

}
