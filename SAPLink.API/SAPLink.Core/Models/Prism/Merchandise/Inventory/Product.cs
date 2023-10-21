namespace SAPLink.Core.Models.Prism.Inventory.Products;

public class Product
{
    [JsonProperty("OriginApplication", NullValueHandling = NullValueHandling.Ignore)]
    public string OriginApplication { get; set; } = "RProPrismWeb";

    //[JsonProperty("sid")]
    //public string Sid { get; set; }

    [JsonProperty("PrimaryItemDefinition", NullValueHandling = NullValueHandling.Ignore)]
    public PrimaryItemDefinition PrimaryItemDefinition { get; set; }

    [JsonProperty("InventoryItems", NullValueHandling = NullValueHandling.Ignore)]
    public InventoryItem[] InventoryItems { get; set; }

    [JsonProperty("UpdateStyleDefinition", NullValueHandling = NullValueHandling.Ignore)]
    public bool UpdateStyleDefinition { get; set; }

    [JsonProperty("UpdateStyleCost", NullValueHandling = NullValueHandling.Ignore)]
    public bool UpdateStyleCost { get; set; }

    [JsonProperty("UpdateStylePrice", NullValueHandling = NullValueHandling.Ignore)]
    public bool UpdateStylePrice { get; set; }

    [JsonProperty("DefaultReasonSidForQtyMemo", NullValueHandling = NullValueHandling.Ignore)]
    public string DefaultReasonSidForQtyMemo { get; set; }

    [JsonProperty("DefaultReasonSidForCostMemo", NullValueHandling = NullValueHandling.Ignore)]
    public string DefaultReasonSidForCostMemo { get; set; }

    [JsonProperty("DefaultReasonSidForPriceMemo", NullValueHandling = NullValueHandling.Ignore)]
    public string DefaultReasonSidForPriceMemo { get; set; }

    // public static Odata CreateInstance() => new();

    public Product(PrimaryItemDefinition primaryItemDefinition, InventoryItem[] inventoryItems, string defaultReasonSidForQtyMemo, string defaultReasonSidForCostMemo, string defaultReasonSidForPriceMemo)
    {
        PrimaryItemDefinition = primaryItemDefinition;
        InventoryItems = inventoryItems;
        DefaultReasonSidForQtyMemo = defaultReasonSidForQtyMemo;
        DefaultReasonSidForCostMemo = defaultReasonSidForCostMemo;
        DefaultReasonSidForPriceMemo = defaultReasonSidForPriceMemo;
    }

    public Product()
    {
    }

}
public class PrimaryItemDefinition
{
    [JsonProperty("dcssid")]
    public string Dcssid { get; set; }

    [JsonProperty("vendsid")]
    public string Vendsid { get; set; }

    [JsonProperty("description1")]
    public string Description1 { get; set; }

    [JsonProperty("description2")]
    public string Description2 { get; set; }

    [JsonProperty("attribute", NullValueHandling = NullValueHandling.Ignore)]
    public string Attribute { get; set; }

    [JsonProperty("itemsize", NullValueHandling = NullValueHandling.Ignore)]
    public string Itemsize { get; set; }

    [JsonProperty("sid", NullValueHandling = NullValueHandling.Ignore)]
    public string Sid { get; set; }

    [JsonProperty("udf3string", NullValueHandling = NullValueHandling.Ignore)]
    public string UDF3 { get; set; }

    [JsonProperty("udf4string", NullValueHandling = NullValueHandling.Ignore)]
    public string UDF4 { get; set; }

    [JsonProperty("udf5string", NullValueHandling = NullValueHandling.Ignore)]
    public string UDF5 { get; set; }

    [JsonProperty("udf6string", NullValueHandling = NullValueHandling.Ignore)]
    public string UDF6 { get; set; }

    [JsonProperty("udf7string", NullValueHandling = NullValueHandling.Ignore)]
    public string UDF7 { get; set; }

    [JsonProperty("udf8string", NullValueHandling = NullValueHandling.Ignore)]
    public string UDF8 { get; set; }

    [JsonProperty("udf9string", NullValueHandling = NullValueHandling.Ignore)]
    public string UDF9 { get; set; }

    [JsonProperty("udf10string", NullValueHandling = NullValueHandling.Ignore)]
    public string UDF10 { get; set; }

    [JsonProperty("udf11string", NullValueHandling = NullValueHandling.Ignore)]
    public string UDF11 { get; set; }

    [JsonProperty("udf12string", NullValueHandling = NullValueHandling.Ignore)]
    public string UDF12 { get; set; }

}
public class Invnquantity
{
    [JsonProperty("qty")]
    public long Qty { get; set; }

    [JsonProperty("invnsbsitemsid")]
    public long Invnsbsitemsid { get; set; }

    [JsonProperty("sbssid")]
    public string Sbssid { get; set; }

    [JsonProperty("storesid")]
    public string Storesid { get; set; }

    [JsonProperty("minqty")]
    public long Minqty { get; set; }

    [JsonProperty("maxqty")]
    public long Maxqty { get; set; }
}
public class Invnprice
{
    [JsonProperty("price")]
    public long Price { get; set; }

    [JsonProperty("invnsbsitemsid")]
    public long Invnsbsitemsid { get; set; }

    [JsonProperty("sbssid")]
    public string Sbssid { get; set; }

    [JsonProperty("pricelvlsid")]
    public string Pricelvlsid { get; set; }

    [JsonProperty("seasonsid")]
    public string Seasonsid { get; set; }
}
public class InventoryItem
{
    [JsonProperty("sbssid")]
    public string Sbssid { get; set; }

    [JsonProperty("alu")]
    public string Alu { get; set; }

    [JsonProperty("dcssid")]
    public string Dcssid { get; set; }

    [JsonProperty("vendsid")]
    public string Vendsid { get; set; }

    [JsonProperty("description1")]
    public string Description1 { get; set; }

    [JsonProperty("description2")]
    public string Description2 { get; set; }

    [JsonProperty("description3")]
    public string Description3 { get; set; }

    [JsonProperty("description4")]
    public string Description4 { get; set; }

    [JsonProperty("longdescription")]
    public string Longdescription { get; set; }

    [JsonProperty("attribute")]
    public string Attribute { get; set; }

    [JsonProperty("itemsize")]
    public string ItemSize { get; set; }

    [JsonProperty("lastrcvdcost")]
    public decimal Lastrcvdcost { get; set; }

    [JsonProperty("spif")]
    public long Spif { get; set; }

    [JsonProperty("taxcodesid")]
    public string Taxcodesid { get; set; }

    ////[JsonProperty("actstrcaseqty")]//qtypercase
    ////public string QtyPerCase { get; set; }

    [JsonProperty("useqtydecimals")]
    public long Useqtydecimals { get; set; }

    [JsonProperty("regional")]
    public bool Regional { get; set; }

    [JsonProperty("active")]
    public bool Active { get; set; }

    //[JsonProperty("upc")]
    //[JsonConverter(typeof(ParseStringConverter))]
    //public object Upc { get; set; }

    [JsonProperty("maxdiscperc1")]
    public long Maxdiscperc1 { get; set; } = 100;

    [JsonProperty("maxdiscperc2")]
    public long Maxdiscperc2 { get; set; } = 100;

    [JsonProperty("serialtype")]
    public long Serialtype { get; set; }

    [JsonProperty("lottype")]
    public long Lottype { get; set; }

    [JsonProperty("activestoresid")]
    public string Activestoresid { get; set; }

    [JsonProperty("noninventory")]
    public bool Noninventory { get; set; }

    [JsonProperty("activepricelevelsid")]
    public string Activepricelevelsid { get; set; }

    [JsonProperty("activeseasonsid")]
    public string Activeseasonsid { get; set; }

    //[JsonProperty("actstrprice")]
    //public long Actstrprice { get; set; }

    //[JsonProperty("actstrpricewt")]
    //public long Actstrpricewt { get; set; }

    //[JsonProperty("actstrohqty")]
    //public long Actstrohqty { get; set; }

    [JsonProperty("dcscode")]
    public string Dcscode { get; set; }


    //[JsonProperty("price")]
    //public int Price { get; set; } 




    [JsonProperty("sid")]
    public string Sid { get; set; }


    //[JsonProperty("invnquantity")]
    //public Invnquantity[] Invnquantity { get; set; }

    //[JsonProperty("invnprice")]
    //public Invnprice[] Invnprice { get; set; }

    [JsonProperty("text1")]
    public string TEXT1 { get; set; }

    [JsonProperty("text2")]
    public string TEXT2 { get; set; }

    [JsonProperty("text3")]
    public string TEXT3 { get; set; }

}