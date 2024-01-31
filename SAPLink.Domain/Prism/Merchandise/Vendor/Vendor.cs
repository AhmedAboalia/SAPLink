namespace SAPLink.Domain.Models.Prism.Merchandise.Vendor;

public class Vendor
{
    [JsonProperty("sid")]
    public string Sid { get; set; }

    [JsonIgnore]
    [JsonProperty("createdby")]
    public Edby Createdby { get; set; }

    [JsonIgnore]
    [JsonProperty("createddatetime")]
    public DateTimeOffset Createddatetime { get; set; }

    [JsonIgnore]
    [JsonProperty("modifiedby")]
    public Edby? Modifiedby { get; set; }

    [JsonIgnore]
    [JsonProperty("modifieddatetime")]
    public DateTimeOffset? Modifieddatetime { get; set; }

    [JsonIgnore]
    [JsonProperty("controllersid")]
    public string Controllersid { get; set; }

    [JsonProperty("originapplication")]
    public string Originapplication { get; set; } = "RProPrismWeb";

    [JsonIgnore]
    [JsonProperty("postdate")]
    public DateTimeOffset Postdate { get; set; }

    [JsonProperty("rowversion")]
    public long Rowversion { get; set; }

    [JsonIgnore]
    [JsonProperty("tenantsid")]
    public string Tenantsid { get; set; }

    [JsonProperty("vendcode")]
    public string Vendcode { get; set; }

    [JsonProperty("active")]
    public bool Active { get; set; }

    [JsonProperty("vendname")]
    public string Vendname { get; set; }

    [JsonIgnore]
    [JsonProperty("info1")]
    public object Info1 { get; set; }

    [JsonIgnore]
    [JsonProperty("info2")]
    public object Info2 { get; set; }

    [JsonIgnore]
    [JsonProperty("termtype")]
    public object Termtype { get; set; }

    [JsonIgnore]
    [JsonProperty("accountno")]
    public object Accountno { get; set; }

    [JsonIgnore]
    [JsonProperty("tradediscperc")]
    public object Tradediscperc { get; set; }

    [JsonIgnore]
    [JsonProperty("vendleadtime")]
    public object Vendleadtime { get; set; }

    [JsonIgnore]
    [JsonProperty("apflag")]
    public long? Apflag { get; set; }

    [JsonIgnore]
    [JsonProperty("currencysid")]
    public string Currencysid { get; set; }

    [JsonProperty("regional")]
    public bool Regional { get; set; }

    [JsonIgnore]
    [JsonProperty("countrysid")]
    public string Countrysid { get; set; }

    [JsonIgnore]
    [JsonProperty("qbid")]
    public object Qbid { get; set; }

    [JsonIgnore]
    [JsonProperty("udf1string")]
    public object Udf1String { get; set; }

    [JsonIgnore]
    [JsonProperty("udf2string")]
    public object Udf2String { get; set; }

    [JsonIgnore]
    [JsonProperty("udf3string")]
    public object Udf3String { get; set; }

    [JsonIgnore]
    [JsonProperty("udf4string")]
    public object Udf4String { get; set; }

    [JsonIgnore]
    [JsonProperty("udf5string")]
    public object Udf5String { get; set; }

    [JsonIgnore]
    [JsonProperty("udf6string")]
    public object Udf6String { get; set; }

    [JsonIgnore]
    [JsonProperty("udf1date")]
    public object Udf1Date { get; set; }

    [JsonIgnore]
    [JsonProperty("udf2date")]
    public object Udf2Date { get; set; }

    [JsonIgnore]
    [JsonProperty("notes")]
    public object Notes { get; set; }

    [JsonIgnore]
    [JsonProperty("image")]
    public object Image { get; set; }

    [JsonProperty("vendid")]
    public long? Vendid { get; set; }

    [JsonProperty("sbssid")]
    public string Sbssid { get; set; }

    [JsonIgnore]
    [JsonProperty("languagesid")]
    public object Languagesid { get; set; }

    [JsonIgnore]
    [JsonProperty("publishstatus")]
    public long Publishstatus { get; set; }

    [JsonIgnore]
    [JsonProperty("mincost")]
    public object Mincost { get; set; }

    [JsonIgnore]
    [JsonProperty("minqty")]
    public object Minqty { get; set; }

    [JsonIgnore]
    [JsonProperty("primarycontactfirstname")]
    public string Primarycontactfirstname { get; set; }

    [JsonIgnore]
    [JsonProperty("primarycontactlastname")]
    public string Primarycontactlastname { get; set; }

    [JsonIgnore]
    [JsonProperty("primarycontactphone1")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long? Primarycontactphone1 { get; set; }

    [JsonIgnore]
    [JsonProperty("primarycontactphone2")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long? Primarycontactphone2 { get; set; }

    [JsonIgnore]
    [JsonProperty("primarycontactemail")]
    public string Primarycontactemail { get; set; }

    [JsonProperty("sbsno")]
    public long Sbsno { get; set; }

    [JsonIgnore]
    [JsonProperty("countrycode")]
    public Countrycode Countrycode { get; set; }

    [JsonIgnore]
    [JsonProperty("currencycode")]
    public Currencycode Currencycode { get; set; }

    [JsonIgnore]
    [JsonProperty("languagename")]
    public object Languagename { get; set; }
}
public enum Countrycode { Sau };

public enum Edby { Sysadmin };

public enum Currencycode { Sar };

public enum Originapplication { RProPrismWeb };