using SAPLink.Core.Models.Prism.Receiving;
using SAPLink.Core.Utilities;

namespace SAPLink.Core.Models.Prism.StockManagement;

public partial class VerifiedVoucher
{
    [JsonProperty("sid")]
    public string Sid { get; set; }

    [JsonProperty("rowversion")]
    public long Rowversion { get; set; }

    [JsonProperty("storesid")]
    public string Storesid { get; set; }

    [JsonProperty("vouno")]
    public long Vouno { get; set; }

    [JsonProperty("origstoresid")]
    public string Origstoresid { get; set; }

    [JsonProperty("pkgno")]
    public string Pkgno { get; set; }

    [JsonProperty("recvitem")]
    public Recvitem[] Recvitem { get; set; }

    [JsonProperty("origstoreno")]
    public long Origstoreno { get; set; }

    [JsonProperty("origstorename")]
    public string Origstorename { get; set; }

    [JsonProperty("storeno")]
    public long Storeno { get; set; }

    [JsonProperty("storename")]
    public string Storename { get; set; }

    [JsonProperty("slipsbsno")]
    public long Slipsbsno { get; set; }

    [JsonProperty("slipno")]
    public long Slipno { get; set; }

    [JsonProperty("storecode")]
    public string Storecode { get; set; }

    [JsonProperty("origstorecode")]
    public string Origstorecode { get; set; }

    [JsonProperty("slipstorecode")]
    public string SlipStoreCode { get; set; }

}
public partial class Recvitem
{
    [JsonProperty("sid")]
    public string Sid { get; set; }

    [JsonProperty("itemsid")]
    public string Itemsid { get; set; }

    [JsonProperty("qty")]
    public decimal Qty { get; set; }

    [JsonProperty("price")]
    public long Price { get; set; }

    [JsonProperty("description1")]
    public string Description1 { get; set; }

    [JsonProperty("description2")]
    public string Description2 { get; set; }


    [JsonProperty("alu")]
    public string Alu { get; set; }

    [JsonProperty("udfvalue5")]
    public string SalesPerUnitFactor { get; set; }
}

public partial class VerifiedVoucher
{
    public static Response<VerifiedVoucher> FromJson(string json) => JsonConvert.DeserializeObject<Response<VerifiedVoucher>>(json, Converter.Settings);
}

public static partial class Serialize
{
    public static string ToJson(this VerifiedVoucher self) => JsonConvert.SerializeObject(self, Converter.Settings);
}