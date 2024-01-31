namespace SAPLink.Domain.Models.SAP.Sales
{
    public partial class DownPayment
    {
        [JsonProperty("DocType", NullValueHandling = NullValueHandling.Ignore)]
        public string DocType { get; set; } = "dDocument_Items";// Type: dDocument_Items or dDocument_Service

        [JsonProperty("DownPaymentType", NullValueHandling = NullValueHandling.Ignore)]
        public string DocObjectCode { get; set; } = "dptInvoice";// Object Code: oPurchaseDownPayments for A/P or oDownPayments for A/R

        [JsonProperty("DocEntry", NullValueHandling = NullValueHandling.Ignore)]
        public string? DocEntry { get; set; }

        [JsonProperty("DocNum", NullValueHandling = NullValueHandling.Ignore)]
        public string? DocNum { get; set; }

        [JsonProperty("DocDueDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DocDueDate { get; set; }

        [JsonProperty("TaxDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? TaxDate { get; set; }

        [JsonProperty("DocDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DocDate { get; set; }

        [JsonProperty("CardCode")]
        public string? CardCode { get; set; } = "001";

        [JsonProperty("CardName", NullValueHandling = NullValueHandling.Ignore)]
        public string CardName { get; set; }

        [JsonProperty("U_EmpID", NullValueHandling = NullValueHandling.Ignore)]
        public string SalesEmployeeUDF { get; set; }

        [JsonProperty("U_SyncToPrism", NullValueHandling = NullValueHandling.Ignore)]
        public string SyncToPrism { get; set; }

        [JsonProperty("U_PrismSid", NullValueHandling = NullValueHandling.Ignore)]
        public string PrismSid { get; set; }

        [JsonProperty("DocTotal", NullValueHandling = NullValueHandling.Ignore)]
        public object DocTotal { get; set; }

        [JsonProperty("Comments", NullValueHandling = NullValueHandling.Ignore)]
        public object Remarks { get; set; }

        [JsonProperty("DocumentLines", NullValueHandling = NullValueHandling.Ignore)]
        public List<DownPaymentLine> DocumentLines { get; set; }

        //[JsonProperty("DocumentAdditionalExpenses", NullValueHandling = NullValueHandling.Ignore)]
        //public List<DocumentAdditionalExpense> DocumentAdditionalExpenses { get; set; }

        [JsonProperty("Series", NullValueHandling = NullValueHandling.Ignore)]
        public string Series { get; set; }
    }
    public partial class DownPaymentLine
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

        [JsonProperty("DiscountPercent", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public double? DiscountPercent { get; set; }

        [JsonProperty("WarehouseCode", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string WarehouseCode { get; set; }

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
    }

    public partial class DownPayment
    {
        public static DownPayment FromJson(string json) => JsonConvert.DeserializeObject<DownPayment>(json, Converter.Settings);

    }
    public static partial class Serialize
    {
        public static string ToJson(this DownPayment self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
