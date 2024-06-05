using SAPLink.Core.Models.System;
using Converter = SAPLink.Core.Utilities.Converter;

namespace SAPLink.Core.Models.SAP.Sales
{

    public partial class Invoice
    {
        
        [JsonProperty("DocEntry", NullValueHandling = NullValueHandling.Ignore)]
        public string? DocEntry { get; set; }

        [JsonProperty("DocNum", NullValueHandling = NullValueHandling.Ignore)]
        public string? DocNum { get; set; }

        [JsonProperty("DocDueDate", NullValueHandling = NullValueHandling.Ignore)]
        public string? DocDueDate { get; set; }  
        
        [JsonProperty("TaxDate", NullValueHandling = NullValueHandling.Ignore)]
        public string? TaxDate { get; set; }   
        
        [JsonProperty("DocDate", NullValueHandling = NullValueHandling.Ignore)]
        public string? DocDate { get; set; }

        [JsonProperty("CardCode")] 
        public string? CardCode { get; set; } = "001";

        [JsonProperty("CardName", NullValueHandling = NullValueHandling.Ignore)]
        public string CardName { get; set; }

        [JsonProperty("U_InvoiceType")]
        public string? InvoiceType { get; set; } = "جديده";

        [JsonProperty("U_EmpID", NullValueHandling = NullValueHandling.Ignore)]
        public string SalesEmployeeUDF { get; set; }

        [JsonProperty("U_SyncToPrism", NullValueHandling = NullValueHandling.Ignore)]
        public string SyncdToSAP { get; set; }

        [JsonProperty("U_PrismSid", NullValueHandling = NullValueHandling.Ignore)]
        public string PrismSid { get; set; }

        [JsonProperty("SalesPersonCode", NullValueHandling = NullValueHandling.Ignore)]
        public string SalesEmployee { get; set; }

        [JsonProperty("DocTotal", NullValueHandling = NullValueHandling.Ignore)]
        public object DocTotal { get; set; }
        
        [JsonProperty("Comments", NullValueHandling = NullValueHandling.Ignore)]
        public object Remarks { get; set; }

        [JsonProperty("DocumentLines", NullValueHandling = NullValueHandling.Ignore)]
        public List<DocumentLine> DocumentLines { get; set; }

        [JsonProperty("DocumentAdditionalExpenses", NullValueHandling = NullValueHandling.Ignore)]
        public List<DocumentAdditionalExpense> DocumentAdditionalExpenses { get; set; }

        [JsonProperty("Series", NullValueHandling = NullValueHandling.Ignore)]
        public string Series { get; set; }

    }
    public partial class DocumentAdditionalExpense
    {
        [JsonProperty("ExpenseCode", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpenseCode { get; set; }

        [JsonProperty("LineTotal", NullValueHandling = NullValueHandling.Ignore)]
        public double? LineTotal { get; set; }

        [JsonProperty("LineTotalFC", NullValueHandling = NullValueHandling.Ignore)]
        public double? LineTotalFc { get; set; }

        [JsonProperty("LineTotalSys", NullValueHandling = NullValueHandling.Ignore)]
        public double? LineTotalSys { get; set; }

        [JsonProperty("PaidToDate", NullValueHandling = NullValueHandling.Ignore)]
        public double? PaidToDate { get; set; }

        [JsonProperty("PaidToDateFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? PaidToDateFc { get; set; }

        [JsonProperty("PaidToDateSys", NullValueHandling = NullValueHandling.Ignore)]
        public long? PaidToDateSys { get; set; }

        [JsonProperty("Remarks", NullValueHandling = NullValueHandling.Ignore)]
        public string Remarks { get; set; }

        [JsonProperty("DistributionMethod", NullValueHandling = NullValueHandling.Ignore)]
        public string DistributionMethod { get; set; } 

        [JsonProperty("TaxLiable", NullValueHandling = NullValueHandling.Ignore)]
        public string TaxLiable { get; set; }

        [JsonProperty("VatGroup", NullValueHandling = NullValueHandling.Ignore)]
        public string VatGroup { get; set; }

        [JsonProperty("TaxPercent", NullValueHandling = NullValueHandling.Ignore)]
        public double? TaxPercent { get; set; }

        [JsonProperty("TaxSum", NullValueHandling = NullValueHandling.Ignore)]
        public double? TaxSum { get; set; }

        [JsonProperty("TaxSumFC", NullValueHandling = NullValueHandling.Ignore)]
        public double? TaxSumFc { get; set; }

        [JsonProperty("TaxSumSys", NullValueHandling = NullValueHandling.Ignore)]
        public double? TaxSumSys { get; set; }

        [JsonProperty("DeductibleTaxSum", NullValueHandling = NullValueHandling.Ignore)]
        public double? DeductibleTaxSum { get; set; }

        [JsonProperty("DeductibleTaxSumFC", NullValueHandling = NullValueHandling.Ignore)]
        public double? DeductibleTaxSumFc { get; set; }

        [JsonProperty("DeductibleTaxSumSys", NullValueHandling = NullValueHandling.Ignore)]
        public long? DeductibleTaxSumSys { get; set; }

        [JsonProperty("AquisitionTax", NullValueHandling = NullValueHandling.Ignore)]
        public string AquisitionTax { get; set; }

        [JsonProperty("TaxCode", NullValueHandling = NullValueHandling.Ignore)]
        public object TaxCode { get; set; }

        [JsonProperty("TaxType", NullValueHandling = NullValueHandling.Ignore)]
        public string TaxType { get; set; }

        [JsonProperty("TaxPaid", NullValueHandling = NullValueHandling.Ignore)]
        public long? TaxPaid { get; set; }

        [JsonProperty("TaxPaidFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? TaxPaidFc { get; set; }

        [JsonProperty("TaxPaidSys", NullValueHandling = NullValueHandling.Ignore)]
        public long? TaxPaidSys { get; set; }

        [JsonProperty("EqualizationTaxPercent", NullValueHandling = NullValueHandling.Ignore)]
        public long? EqualizationTaxPercent { get; set; }

        [JsonProperty("EqualizationTaxSum", NullValueHandling = NullValueHandling.Ignore)]
        public long? EqualizationTaxSum { get; set; }

        [JsonProperty("EqualizationTaxFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? EqualizationTaxFc { get; set; }

        [JsonProperty("EqualizationTaxSys", NullValueHandling = NullValueHandling.Ignore)]
        public long? EqualizationTaxSys { get; set; }

        [JsonProperty("TaxTotalSum", NullValueHandling = NullValueHandling.Ignore)]
        public long? TaxTotalSum { get; set; }

        [JsonProperty("TaxTotalSumFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? TaxTotalSumFc { get; set; }

        [JsonProperty("TaxTotalSumSys", NullValueHandling = NullValueHandling.Ignore)]
        public long? TaxTotalSumSys { get; set; }

        [JsonProperty("BaseDocEntry", NullValueHandling = NullValueHandling.Ignore)]
        public long? BaseDocEntry { get; set; }

        [JsonProperty("BaseDocLine", NullValueHandling = NullValueHandling.Ignore)]
        public long? BaseDocLine { get; set; }

        [JsonProperty("BaseDocType", NullValueHandling = NullValueHandling.Ignore)]
        public long? BaseDocType { get; set; }

        [JsonProperty("BaseDocumentReference", NullValueHandling = NullValueHandling.Ignore)]
        public object BaseDocumentReference { get; set; }

        [JsonProperty("LineNum", NullValueHandling = NullValueHandling.Ignore)]
        public long? LineNum { get; set; }

        [JsonProperty("LastPurchasePrice", NullValueHandling = NullValueHandling.Ignore)]
        public object LastPurchasePrice { get; set; }

        [JsonProperty("Status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("Stock", NullValueHandling = NullValueHandling.Ignore)]
        public string Stock { get; set; }

        [JsonProperty("TargetAbsEntry", NullValueHandling = NullValueHandling.Ignore)]
        public long? TargetAbsEntry { get; set; }

        [JsonProperty("TargetType", NullValueHandling = NullValueHandling.Ignore)]
        public object TargetType { get; set; }

        [JsonProperty("WTLiable", NullValueHandling = NullValueHandling.Ignore)]
        public string WtLiable { get; set; }

        [JsonProperty("DistributionRule", NullValueHandling = NullValueHandling.Ignore)]
        public object DistributionRule { get; set; }

        [JsonProperty("Project", NullValueHandling = NullValueHandling.Ignore)]
        public object Project { get; set; }

        [JsonProperty("DistributionRule2", NullValueHandling = NullValueHandling.Ignore)]
        public object DistributionRule2 { get; set; }

        [JsonProperty("DistributionRule3", NullValueHandling = NullValueHandling.Ignore)]
        public object DistributionRule3 { get; set; }

        [JsonProperty("DistributionRule4", NullValueHandling = NullValueHandling.Ignore)]
        public object DistributionRule4 { get; set; }

        [JsonProperty("DistributionRule5", NullValueHandling = NullValueHandling.Ignore)]
        public object DistributionRule5 { get; set; }

        [JsonProperty("LineGross", NullValueHandling = NullValueHandling.Ignore)]
        public double? LineGross { get; set; }

        [JsonProperty("LineGrossSys", NullValueHandling = NullValueHandling.Ignore)]
        public long? LineGrossSys { get; set; }

        [JsonProperty("LineGrossFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? LineGrossFc { get; set; }

        [JsonProperty("ExternalCalcTaxRate", NullValueHandling = NullValueHandling.Ignore)]
        public long? ExternalCalcTaxRate { get; set; }

        [JsonProperty("ExternalCalcTaxAmount", NullValueHandling = NullValueHandling.Ignore)]
        public long? ExternalCalcTaxAmount { get; set; }

        [JsonProperty("ExternalCalcTaxAmountFC", NullValueHandling = NullValueHandling.Ignore)]
        public long? ExternalCalcTaxAmountFc { get; set; }

        [JsonProperty("ExternalCalcTaxAmountSC", NullValueHandling = NullValueHandling.Ignore)]
        public long? ExternalCalcTaxAmountSc { get; set; }

        [JsonProperty("CUSplit", NullValueHandling = NullValueHandling.Ignore)]
        public string CuSplit { get; set; }

        [JsonProperty("DocExpenseTaxJurisdictions", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> DocExpenseTaxJurisdictions { get; set; }

        [JsonProperty("DocFreightEBooksDetails", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> DocFreightEBooksDetails { get; set; }
    }
    public partial class DocumentLine
    {
        [JsonProperty("ItemCode", NullValueHandling = NullValueHandling.Ignore)]
        public string ItemCode { get; set; }

        [JsonProperty("Quantity", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public double? Quantity { get; set; }

        [JsonProperty("VatGroup", NullValueHandling = NullValueHandling.Ignore)]
        public string VatGroup { get; set; }

        [JsonProperty("UnitPrice", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public double? UnitPrice { get; set; }   
        
        [JsonProperty("CostingCode", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string CostCenter { get; set; }

        [JsonProperty("CostingCode2", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Region { get; set; }

        [JsonProperty("CostingCode3", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string City { get; set; }

        [JsonProperty("CostingCode4", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string Branch { get; set; }
        

        [JsonProperty("DiscountPercent", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public double? DiscountPercent { get; set; }

        [JsonProperty("WarehouseCode", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string WarehouseCode { get; set; }
    }



    public partial class Invoice
    {
        public static Invoice FromJson(string json) => JsonConvert.DeserializeObject<Invoice>(json, Converter.Settings);

        public static string CreateOrderBody(Prism.Sales.Invoice invoice, List<TaxCodes> taxCodes, string customerCode)
        {

            var ShippingTaxCode = "X0";
            var FeeTaxCode = "X0";
            try
            {
                ShippingTaxCode = taxCodes.FirstOrDefault(x => x.Rate == -invoice.ShippingTaxPercentage).Code;
            }
            catch (Exception e)
            {
                //
            }
            try
            {
                FeeTaxCode = taxCodes.FirstOrDefault(x => x.Rate == -invoice.FeeTaxPrecentage).Code;
            }
            catch (Exception e)
            {
                //
            }
            var dateTime = invoice.InvoicePostedDate;

            var arInvoice = new Invoice
            {
                CardCode = customerCode,
                DocDueDate = dateTime,
                DocDate = dateTime,
                TaxDate = dateTime,
                Remarks = $"Prism Transaction: {invoice.DocumentNumber} - Branch ({invoice.StoreCode} - {invoice.StoreName}) - Created By: {invoice.CreatedBy}",
            };

            return arInvoice.ToJson();
        }
    }

   
    public static partial class Serialize
    {
        public static string ToJson(this Invoice self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
