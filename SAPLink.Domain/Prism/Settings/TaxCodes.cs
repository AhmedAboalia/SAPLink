using SAPLink.Domain.Common;

namespace SAPLink.Domain.Models.Prism.Settings;

public partial class TaxCodes
{
    [JsonProperty("sid")]
    public string Sid { get; set; }


    [JsonProperty("sbssid")]
    public long SbsSid { get; set; }

    [JsonProperty("taxcode")]
    //[JsonConverter(typeof(ParseStringConverter))]
    public int TaxCode { get; set; }

    [JsonProperty("taxname")]
    public string TaxName { get; set; } = "TAXABLE";

    [JsonProperty("isdefault")]
    public bool IsDefault { get; set; }


    public static OdataPrism<TaxCodes> FromJson(string json) => JsonConvert.DeserializeObject<OdataPrism<TaxCodes>>(json, Converter.Settings);


    public static class Serialize
    {
        public static string ToJson(TaxCodes self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
    public TaxCodes(string sid, long sbsSid, int taxCode, string taxName, bool isDefault)
    {
        Sid = sid;
        SbsSid = sbsSid;
        TaxCode = taxCode;
        TaxName = taxName;
        IsDefault = isDefault;
    }

    public TaxCodes()
    {

    }
}

//public class TaxCodeDto
//{
//    [JsonProperty("sid")]
//    public string Sid { get; set; }


//    [JsonProperty("sbssid")]
//    public string Sbssid { get; set; }

//    [JsonProperty("taxcode")]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long TaxCode { get; set; }

//    [JsonProperty("taxname")]
//    public string TaxName { get; set; }

//    [JsonProperty("isdefault")]
//    public bool IsDefault { get; set; }

//    [JsonProperty("sbssid")]
//    public string Subsidiary_Sid { get; set; }
//}