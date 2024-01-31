namespace SAPLink.Domain.Models.Prism.Sales
{
    public partial class InvoiceReturn
    {
        [JsonProperty("lty_order_used_points_negative")]
        public object LtyOrderUsedPointsNegative { get; set; }

        [JsonProperty("lty_sale_used_points_negative")]
        public object LtySaleUsedPointsNegative { get; set; }

        [JsonProperty("row_version")]
        public long RowVersion { get; set; }

        [JsonProperty("lty_redeem_amt")]
        public object LtyRedeemAmt { get; set; }

        [JsonProperty("store_number")]
        public long StoreNumber { get; set; }

        [JsonProperty("lty_order_earned_points_negative")]
        public object LtyOrderEarnedPointsNegative { get; set; }

        [JsonProperty("lty_order_used_points_positive")]
        public object LtyOrderUsedPointsPositive { get; set; }

        [JsonProperty("reason_code")]
        public object ReasonCode { get; set; }

        [JsonProperty("transaction_total_amt")]
        public string TransactionTotalAmt { get; set; }

        [JsonProperty("bt_cuid")]
        public string BtCuid { get; set; }

        [JsonProperty("lty_used_points_positive")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long LtyUsedPointsPositive { get; set; }

        [JsonProperty("lty_gift_amt")]
        public object LtyGiftAmt { get; set; }

        [JsonProperty("fee_amt_returned1")]
        public object FeeAmtReturned1 { get; set; }

        [JsonProperty("lty_used_points_negative")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long LtyUsedPointsNegative { get; set; }

        [JsonProperty("lty_order_total_based_disc")]
        public object LtyOrderTotalBasedDisc { get; set; }

        [JsonProperty("fee_amt1")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long FeeAmt1 { get; set; }

        [JsonProperty("lty_sale_earned_points_positive")]
        public object LtySaleEarnedPointsPositive { get; set; }

        [JsonProperty("lty_perc_reward_disc_amt")]
        public object LtyPercRewardDiscAmt { get; set; }

        [JsonProperty("invoice_posted_date")]
        public string InvoicePostedDate { get; set; }

        [JsonProperty("total_item_count")]
        public long TotalItemCount { get; set; }

        [JsonProperty("lty_earned_points")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long LtyEarnedPoints { get; set; }

        [JsonProperty("lty_sale_earned_points_negative")]
        public object LtySaleEarnedPointsNegative { get; set; }

        [JsonProperty("fee_type1_sid")]
        public string FeeType1Sid { get; set; }

        [JsonProperty("document_number")]
        public long DocumentNumber { get; set; }

        [JsonProperty("sid")]
        public string Sid { get; set; }

        [JsonProperty("bt_last_name")]
        public object BtLastName { get; set; }

        [JsonProperty("shipping_amt_manual")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long ShippingAmtManual { get; set; }

        [JsonProperty("lty_earned_points_positive")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long LtyEarnedPointsPositive { get; set; }

        [JsonProperty("lty_earned_points_negative")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long LtyEarnedPointsNegative { get; set; }

        [JsonProperty("lty_used_points")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long LtyUsedPoints { get; set; }

        [JsonProperty("lty_sale_used_points_positive")]
        public object LtySaleUsedPointsPositive { get; set; }

        [JsonProperty("bt_first_name")]
        public string BtFirstName { get; set; }

        [JsonProperty("store_name")]
        public string StoreName { get; set; }

        [JsonProperty("lty_perc_reward_disc_perc")]
        public object LtyPercRewardDiscPerc { get; set; }

        [JsonProperty("shipping_amt_manual_returned")]
        public object ShippingAmtManualReturned { get; set; }

        [JsonProperty("lty_sale_total_based_disc")]
        public object LtySaleTotalBasedDisc { get; set; }

        [JsonProperty("lty_order_earned_points_positive")]
        public object LtyOrderEarnedPointsPositive { get; set; }

        [JsonProperty("from_centrals")]
        public long FromCentrals { get; set; }
    }

    public partial class InvoiceReturn
    {
        public static List<InvoiceReturn> FromJson(string json) => JsonConvert.DeserializeObject<List<InvoiceReturn>>(json, Converter.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson(this List<InvoiceReturn> self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
