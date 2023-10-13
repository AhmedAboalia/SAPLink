using SAPLink.Core.Utilities;

namespace SAPLink.Core.Models.Prism.Sales;

public partial class Invoices
{
    [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
    public string Link { get; set; }

    [JsonProperty("sid", NullValueHandling = NullValueHandling.Ignore)]
    public string Sid { get; set; }

    [JsonProperty("row_version", NullValueHandling = NullValueHandling.Ignore)]
    public long? RowVersion { get; set; }

    [JsonProperty("document_number")]
    public object DocumentNumber { get; set; }

    [JsonProperty("invoice_posted_date")]
    public object InvoicePostedDate { get; set; }

    [JsonProperty("doc_tender_type")]
    public object DocTenderType { get; set; }

    [JsonProperty("store_number", NullValueHandling = NullValueHandling.Ignore)]
    public long? StoreNumber { get; set; }

    [JsonProperty("store_code", NullValueHandling = NullValueHandling.Ignore)]
    public string? StoreCode { get; set; }
    //public StoreCode? StoreCode { get; set; }

    [JsonProperty("bt_last_name")]
    public string BtLastName { get; set; }

    [JsonProperty("bt_first_name")]
    public string BtFirstName { get; set; }

    [JsonProperty("comment1")]
    public object Comment1 { get; set; }

    [JsonProperty("comment2")]
    public object Comment2 { get; set; }

    [JsonProperty("subsidiary_uid", NullValueHandling = NullValueHandling.Ignore)]
    public string SubsidiaryUid { get; set; }

    [JsonProperty("store_uid", NullValueHandling = NullValueHandling.Ignore)]
    public string StoreUid { get; set; }

    [JsonProperty("order_document_number")]
    public object OrderDocumentNumber { get; set; }

    [JsonProperty("has_sale")]
    public bool? HasSale { get; set; }

    [JsonProperty("has_return")]
    public bool? HasReturn { get; set; }

    [JsonProperty("lty_redeem_amt")]
    public object LtyRedeemAmt { get; set; }

    [JsonProperty("lty_gift_amt")]
    public object LtyGiftAmt { get; set; }

    [JsonProperty("lty_sale_total_based_disc")]
    public object LtySaleTotalBasedDisc { get; set; }

    [JsonProperty("lty_perc_reward_disc_perc")]
    public object LtyPercRewardDiscPerc { get; set; }

    [JsonProperty("lty_perc_reward_disc_amt")]
    public object LtyPercRewardDiscAmt { get; set; }

    [JsonProperty("lty_earned_points_positive", NullValueHandling = NullValueHandling.Ignore)]
    public long? LtyEarnedPointsPositive { get; set; }

    [JsonProperty("lty_earned_points_negative", NullValueHandling = NullValueHandling.Ignore)]
    public long? LtyEarnedPointsNegative { get; set; }

    [JsonProperty("lty_used_points_positive", NullValueHandling = NullValueHandling.Ignore)]
    public long? LtyUsedPointsPositive { get; set; }

    [JsonProperty("lty_used_points_negative", NullValueHandling = NullValueHandling.Ignore)]
    public long? LtyUsedPointsNegative { get; set; }

    [JsonProperty("lty_used_points", NullValueHandling = NullValueHandling.Ignore)]
    public long? LtyUsedPoints { get; set; }

    [JsonProperty("lty_earned_points", NullValueHandling = NullValueHandling.Ignore)]
    public long? LtyEarnedPoints { get; set; }

    [JsonProperty("lty_order_total_based_disc")]
    public object LtyOrderTotalBasedDisc { get; set; }

    [JsonProperty("lty_order_earned_points_positive")]
    public object LtyOrderEarnedPointsPositive { get; set; }

    [JsonProperty("lty_order_earned_points_negative")]
    public object LtyOrderEarnedPointsNegative { get; set; }

    [JsonProperty("lty_order_used_points_positive")]
    public object LtyOrderUsedPointsPositive { get; set; }

    [JsonProperty("lty_order_used_points_negative")]
    public object LtyOrderUsedPointsNegative { get; set; }

    [JsonProperty("lty_sale_earned_points_positive")]
    public object LtySaleEarnedPointsPositive { get; set; }

    [JsonProperty("lty_sale_earned_points_negative")]
    public object LtySaleEarnedPointsNegative { get; set; }

    [JsonProperty("lty_sale_used_points_positive")]
    public object LtySaleUsedPointsPositive { get; set; }

    [JsonProperty("lty_sale_used_points_negative")]
    public object LtySaleUsedPointsNegative { get; set; }

    [JsonProperty("bt_info1")]
    public string BtInfo1 { get; set; }
}

public enum StoreCode
{
    B135, The000, The001, The005, The012, The044
}
internal class StoreCodeConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(StoreCode) || t == typeof(StoreCode?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        switch (value)
        {
            case "000":
                return StoreCode.The000;
            case "001":
                return StoreCode.The001;
            case "005":
                return StoreCode.The005;
            case "012":
                return StoreCode.The012;
            case "044":
                return StoreCode.The044;
            case "b135":
                return StoreCode.B135;
        }
        throw new Exception("Cannot unmarshal type StoreCode");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (StoreCode)untypedValue;
        switch (value)
        {
            case StoreCode.The000:
                serializer.Serialize(writer, "000");
                return;
            case StoreCode.The001:
                serializer.Serialize(writer, "001");
                return;
            case StoreCode.The005:
                serializer.Serialize(writer, "005");
                return;
            case StoreCode.The012:
                serializer.Serialize(writer, "012");
                return;
            case StoreCode.The044:
                serializer.Serialize(writer, "044");
                return;
            case StoreCode.B135:
                serializer.Serialize(writer, "b135");
                return;
        }
        throw new Exception("Cannot marshal type StoreCode");
    }

    public static readonly StoreCodeConverter Singleton = new StoreCodeConverter();
}

public partial class Invoices
{
    public static List<Invoices> FromJson(string json) => JsonConvert.DeserializeObject<List<Invoices>>(json, Converter.Settings);
}

public static partial class Serialize
{
    public static string ToJson(this Invoices self) => JsonConvert.SerializeObject(self, Converter.Settings);
}