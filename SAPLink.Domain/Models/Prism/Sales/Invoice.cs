// To parse this JSON data:
// var invoice = Invoice.FromJson(jsonString);

using SAPLink.Domain.Utilities;

namespace SAPLink.Domain.Models.Prism.Sales;

public partial class Invoice
{
    //[JsonProperty("bt_id", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("bt_info1", NullValueHandling = NullValueHandling.Ignore)]
    public string CustomerID { get; set; }

    [JsonProperty("bt_cuid", NullValueHandling = NullValueHandling.Ignore)]
    public string CustomerSID { get; set; }

    [JsonProperty("bt_first_name", NullValueHandling = NullValueHandling.Ignore)]
    public string CustomerName { get; set; }

    [JsonProperty("bt_email", NullValueHandling = NullValueHandling.Ignore)]
    public string CustomerEmail { get; set; }

    [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
    public string Link { get; set; }

    [JsonProperty("sid", NullValueHandling = NullValueHandling.Ignore)]
    public string Sid { get; set; }

    [JsonProperty("created_by", NullValueHandling = NullValueHandling.Ignore)]
    public string CreatedBy { get; set; }

    [JsonProperty("created_datetime", NullValueHandling = NullValueHandling.Ignore)]
    public DateTime CreatedDatetime { get; set; }

    [JsonProperty("bt_company_name", NullValueHandling = NullValueHandling.Ignore)]
    public string WholesaleCustomerCode { get; set; }

    [JsonProperty("bt_title", NullValueHandling = NullValueHandling.Ignore)]
    public string IsWholesale { get; set; }

    [JsonProperty("row_version", NullValueHandling = NullValueHandling.Ignore)]
    public string RowVersion { get; set; }

    [JsonProperty("document_number", NullValueHandling = NullValueHandling.Ignore)]
    public long DocumentNumber { get; set; }

    [JsonProperty("invoice_posted_date", NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset InvoicePostedDate { get; set; }

    [JsonProperty("subsidiary_number", NullValueHandling = NullValueHandling.Ignore)]
    public long SubsidiaryNumber { get; set; }

    [JsonProperty("store_number", NullValueHandling = NullValueHandling.Ignore)]
    public long StoreNumber { get; set; }

    [JsonProperty("store_code", NullValueHandling = NullValueHandling.Ignore)]
    public string StoreCode { get; set; }

    [JsonProperty("employee1_login_name", NullValueHandling = NullValueHandling.Ignore)]
    public string SalesEmpolyee { get; set; }

    [JsonProperty("original_store_number", NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(ParseStringConverter))]
    public long OriginalStoreNumber { get; set; }

    [JsonProperty("original_store_code", NullValueHandling = NullValueHandling.Ignore)]
    public string OriginalStoreCode { get; set; }

    [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
    public List<Item> Items { get; set; }

    [JsonProperty("tenders", NullValueHandling = NullValueHandling.Ignore)]
    public List<Tender> Tenders { get; set; }

    [JsonProperty("transaction_total_tax_amt", NullValueHandling = NullValueHandling.Ignore)]
    public double TransactionTotalTaxAmt { get; set; }

    [JsonProperty("transaction_total_amt", NullValueHandling = NullValueHandling.Ignore)]
    public double TransactionTotalAmt { get; set; }

    [JsonProperty("transaction_subtotal_with_tax", NullValueHandling = NullValueHandling.Ignore)]
    public double TransactionSubtotalWithTax { get; set; }

    [JsonProperty("shipping_tax_percentage", NullValueHandling = NullValueHandling.Ignore)]
    public double ShippingTaxPercentage { get; set; }

    [JsonProperty("subsidiary_name", NullValueHandling = NullValueHandling.Ignore)]
    public string SubsidiaryName { get; set; }

    [JsonProperty("store_name", NullValueHandling = NullValueHandling.Ignore)]
    public string StoreName { get; set; }

    [JsonProperty("subsidiary_uid", NullValueHandling = NullValueHandling.Ignore)]
    public string SubsidiaryUid { get; set; }

    [JsonProperty("store_uid", NullValueHandling = NullValueHandling.Ignore)]
    public string StoreUid { get; set; }

    [JsonProperty("original_store_uid", NullValueHandling = NullValueHandling.Ignore)]
    public string OriginalStoreUid { get; set; }

    [JsonProperty("receipt_type", NullValueHandling = NullValueHandling.Ignore)]
    public long ReceiptType { get; set; }

    [JsonProperty("transaction_total_shipping_tax", NullValueHandling = NullValueHandling.Ignore)]
    public double TransactionTotalShippingTax { get; set; }

    [JsonProperty("transaction_total_shipping_amt_no_tax", NullValueHandling = NullValueHandling.Ignore)]
    public double TransactionTotalShippingAmtNoTax { get; set; }

    [JsonProperty("transaction_total_shipping_amt_with_tax", NullValueHandling = NullValueHandling.Ignore)]
    public long TransactionTotalShippingAmtWithTax { get; set; }

    [JsonProperty("total_fee_amt", NullValueHandling = NullValueHandling.Ignore)]
    public double TotalFeeAmount { get; set; }

    [JsonProperty("fee_amt1", NullValueHandling = NullValueHandling.Ignore)]
    public double FeeAmount { get; set; }

    [JsonProperty("fee_tax_amt1", NullValueHandling = NullValueHandling.Ignore)]
    public long FeeTaxAmount { get; set; }

    [JsonProperty("fee_tax_perc1", NullValueHandling = NullValueHandling.Ignore)]
    public long FeeTaxPrecentage { get; set; }

    [JsonProperty("lty_redeem_amt", NullValueHandling = NullValueHandling.Ignore)]
    public object LtyRedeemAmt { get; set; }

    [JsonProperty("lty_gift_amt", NullValueHandling = NullValueHandling.Ignore)]
    public object LtyGiftAmt { get; set; }

    [JsonProperty("lty_sale_total_based_disc", NullValueHandling = NullValueHandling.Ignore)]
    public object LtySaleTotalBasedDisc { get; set; }

    [JsonProperty("lty_perc_reward_disc_perc", NullValueHandling = NullValueHandling.Ignore)]
    public object LtyPercRewardDiscPerc { get; set; }

    [JsonProperty("lty_perc_reward_disc_amt", NullValueHandling = NullValueHandling.Ignore)]
    public object LtyPercRewardDiscAmt { get; set; }

    [JsonProperty("lty_earned_points_positive", NullValueHandling = NullValueHandling.Ignore)]
    public long LtyEarnedPointsPositive { get; set; }

    [JsonProperty("lty_earned_points_negative", NullValueHandling = NullValueHandling.Ignore)]
    public long LtyEarnedPointsNegative { get; set; }

    [JsonProperty("lty_used_points_positive", NullValueHandling = NullValueHandling.Ignore)]
    public long LtyUsedPointsPositive { get; set; }

    [JsonProperty("lty_used_points_negative", NullValueHandling = NullValueHandling.Ignore)]
    public long LtyUsedPointsNegative { get; set; }

    [JsonProperty("lty_used_points", NullValueHandling = NullValueHandling.Ignore)]
    public long LtyUsedPoints { get; set; }

    [JsonProperty("lty_earned_points", NullValueHandling = NullValueHandling.Ignore)]
    public long LtyEarnedPoints { get; set; }

    [JsonProperty("ref_sale_sid", NullValueHandling = NullValueHandling.Ignore)]//ref_sale_doc_no
    public string RefSaleDocSid { get; set; }

    [JsonProperty("lty_order_total_based_disc", NullValueHandling = NullValueHandling.Ignore)]
    public object LtyOrderTotalBasedDisc { get; set; }

    [JsonProperty("lty_order_earned_points_positive", NullValueHandling = NullValueHandling.Ignore)]
    public object LtyOrderEarnedPointsPositive { get; set; }

    [JsonProperty("lty_order_earned_points_negative", NullValueHandling = NullValueHandling.Ignore)]
    public object LtyOrderEarnedPointsNegative { get; set; }

    [JsonProperty("lty_order_used_points_positive", NullValueHandling = NullValueHandling.Ignore)]
    public object LtyOrderUsedPointsPositive { get; set; }

    [JsonProperty("lty_order_used_points_negative", NullValueHandling = NullValueHandling.Ignore)]
    public object LtyOrderUsedPointsNegative { get; set; }

    [JsonProperty("lty_sale_earned_points_positive", NullValueHandling = NullValueHandling.Ignore)]
    public object LtySaleEarnedPointsPositive { get; set; }

    [JsonProperty("lty_sale_earned_points_negative", NullValueHandling = NullValueHandling.Ignore)]
    public object LtySaleEarnedPointsNegative { get; set; }

    [JsonProperty("lty_sale_used_points_positive", NullValueHandling = NullValueHandling.Ignore)]
    public object LtySaleUsedPointsPositive { get; set; }

    [JsonProperty("lty_sale_used_points_negative", NullValueHandling = NullValueHandling.Ignore)]
    public object LtySaleUsedPointsNegative { get; set; }
}

public partial class Item
{
    [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
    public string Link { get; set; }

    [JsonProperty("quantity", NullValueHandling = NullValueHandling.Ignore)]
    public double Quantity { get; set; }

    [JsonProperty("original_price", NullValueHandling = NullValueHandling.Ignore)]
    public long OriginalPrice { get; set; }

    [JsonProperty("original_tax_amount", NullValueHandling = NullValueHandling.Ignore)]
    public double OriginalTaxAmount { get; set; }

    [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
    public double Price { get; set; }

    [JsonProperty("tax_percent", NullValueHandling = NullValueHandling.Ignore)]
    public long TaxPercent { get; set; }

    [JsonProperty("tax_amount", NullValueHandling = NullValueHandling.Ignore)]
    public double TaxAmount { get; set; }

    [JsonProperty("cost", NullValueHandling = NullValueHandling.Ignore)]
    public long Cost { get; set; }

    [JsonProperty("scan_upc", NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(ParseStringConverter))]
    public long ScanUpc { get; set; }

    [JsonProperty("total_discount_percent", NullValueHandling = NullValueHandling.Ignore)]
    public double TotalDiscountPercent { get; set; }

    [JsonProperty("total_discount_amount", NullValueHandling = NullValueHandling.Ignore)]
    public double TotalDiscountAmount { get; set; }

    [JsonProperty("item_description1", NullValueHandling = NullValueHandling.Ignore)]
    public string ItemDescription1 { get; set; }

    [JsonProperty("item_description2", NullValueHandling = NullValueHandling.Ignore)]
    public string ItemDescription2 { get; set; }

    [JsonProperty("item_description3", NullValueHandling = NullValueHandling.Ignore)]
    public string ItemDescription3 { get; set; }

    [JsonProperty("alu", NullValueHandling = NullValueHandling.Ignore)]
    public string Alu { get; set; }

    [JsonProperty("discount_amt", NullValueHandling = NullValueHandling.Ignore)]
    public double DiscountAmt { get; set; }

    [JsonProperty("discount_perc", NullValueHandling = NullValueHandling.Ignore)]
    public double DiscountPerc { get; set; }

    [JsonProperty("discount_reason", NullValueHandling = NullValueHandling.Ignore)]
    public string DiscountReason { get; set; }
}

public partial class Tender
{
    [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
    public string Link { get; set; }

    [JsonProperty("sid", NullValueHandling = NullValueHandling.Ignore)]
    public string Sid { get; set; }

    [JsonProperty("created_by", NullValueHandling = NullValueHandling.Ignore)]
    public string CreatedBy { get; set; }

    [JsonProperty("created_datetime", NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedDatetime { get; set; }

    [JsonProperty("post_date", NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset PostDate { get; set; }

    [JsonProperty("row_version", NullValueHandling = NullValueHandling.Ignore)]
    public long RowVersion { get; set; }

    [JsonProperty("tender_type", NullValueHandling = NullValueHandling.Ignore)]
    public int TenderType { get; set; }

    [JsonProperty("tender_pos", NullValueHandling = NullValueHandling.Ignore)]
    public long TenderPos { get; set; }

    [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
    public double Amount { get; set; }

    [JsonProperty("taken", NullValueHandling = NullValueHandling.Ignore)]
    public double Taken { get; set; }

    [JsonProperty("given", NullValueHandling = NullValueHandling.Ignore)]
    public long Given { get; set; }

    [JsonProperty("currency_name", NullValueHandling = NullValueHandling.Ignore)]
    public string CurrencyName { get; set; }

    [JsonProperty("tender_name", NullValueHandling = NullValueHandling.Ignore)]
    public string TenderName { get; set; }
}

public partial class Invoice
{
    public static List<Invoice> FromJson(string json) => JsonConvert.DeserializeObject<List<Invoice>>(json, Converter.Settings);
}

public static partial class Serialize
{
    public static string ToJson(this List<Invoice> self) => JsonConvert.SerializeObject(self, Converter.Settings);
}