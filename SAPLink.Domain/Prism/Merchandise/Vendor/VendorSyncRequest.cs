using SAPLink.Domain.Utilities;

namespace SAPLink.Domain.Models.Prism.Merchandise.Vendor;

public class VendorSyncRequest
{
    [JsonProperty("rowversion")]
    public long Rowversion { get; set; }

    [JsonProperty("vendname")]
    public string Vendname { get; set; }

    [JsonProperty("sbssid")]
    public string Sbssid { get; set; }

    [JsonProperty("vendorterm")]
    public object[] Vendorterm { get; set; }

    [JsonProperty("vendoraddress")]
    public Vendoraddress[] Vendoraddress { get; set; }

    [JsonProperty("vendorcontact")]
    public Vendorcontact[] Vendorcontact { get; set; }
}
public partial class Vendoraddress
{
    [JsonProperty("sid")]
    public string Sid { get; set; }

    [JsonProperty("createdby")]
    public string Createdby { get; set; }

    [JsonProperty("createddatetime")]
    public DateTimeOffset Createddatetime { get; set; }

    [JsonProperty("modifiedby")]
    public object Modifiedby { get; set; }

    [JsonProperty("modifieddatetime")]
    public object Modifieddatetime { get; set; }

    [JsonProperty("controllersid")]
    public string Controllersid { get; set; }

    [JsonProperty("originapplication")]
    public string Originapplication { get; set; }

    [JsonProperty("postdate")]
    public DateTimeOffset Postdate { get; set; }

    [JsonProperty("rowversion")]
    public long Rowversion { get; set; }

    [JsonProperty("tenantsid")]
    public string Tenantsid { get; set; }

    [JsonProperty("vendsid")]
    public string Vendsid { get; set; }

    [JsonProperty("primaryflag")]
    public bool Primaryflag { get; set; }

    [JsonProperty("active")]
    public bool Active { get; set; }

    [JsonProperty("companyname")]
    public object Companyname { get; set; }

    [JsonProperty("address1")]
    public object Address1 { get; set; }

    [JsonProperty("address2")]
    public object Address2 { get; set; }

    [JsonProperty("address3")]
    public object Address3 { get; set; }

    [JsonProperty("address4")]
    public object Address4 { get; set; }

    [JsonProperty("address5")]
    public object Address5 { get; set; }

    [JsonProperty("address6")]
    public object Address6 { get; set; }

    [JsonProperty("city")]
    public object City { get; set; }

    [JsonProperty("state")]
    public object State { get; set; }

    [JsonProperty("postalcode")]
    public object Postalcode { get; set; }

    [JsonProperty("postalcodeextension")]
    public object Postalcodeextension { get; set; }

    [JsonProperty("countrysid")]
    public string Countrysid { get; set; }

    [JsonProperty("seqno")]
    public long Seqno { get; set; }

    [JsonProperty("addresstypesid")]
    public object Addresstypesid { get; set; }

    [JsonProperty("countrycode")]
    public string Countrycode { get; set; }

    [JsonProperty("addresstypename")]
    public object Addresstypename { get; set; }
}

public partial class Vendorcontact
{
    [JsonProperty("sid")]
    public string Sid { get; set; }

    [JsonProperty("createdby")]
    public string Createdby { get; set; }

    [JsonProperty("createddatetime")]
    public DateTimeOffset Createddatetime { get; set; }

    [JsonProperty("modifiedby")]
    public object Modifiedby { get; set; }

    [JsonProperty("modifieddatetime")]
    public object Modifieddatetime { get; set; }

    [JsonProperty("controllersid")]
    public string Controllersid { get; set; }

    [JsonProperty("originapplication")]
    public string Originapplication { get; set; }

    [JsonProperty("postdate")]
    public DateTimeOffset Postdate { get; set; }

    [JsonProperty("rowversion")]
    public long Rowversion { get; set; }

    [JsonProperty("tenantsid")]
    public string Tenantsid { get; set; }

    [JsonProperty("vendsid")]
    public string Vendsid { get; set; }

    [JsonProperty("firstname")]
    public string Firstname { get; set; }

    [JsonProperty("lastname")]
    public string Lastname { get; set; }

    [JsonProperty("titlesid")]
    public object Titlesid { get; set; }

    [JsonProperty("contacttypesid")]
    public object Contacttypesid { get; set; }

    [JsonProperty("primaryflag")]
    public bool Primaryflag { get; set; }

    [JsonProperty("emailtypesid")]
    public object Emailtypesid { get; set; }

    [JsonProperty("emailaddress")]
    public string Emailaddress { get; set; }

    [JsonProperty("phone1typesid")]
    public string Phone1Typesid { get; set; }

    [JsonProperty("phone1no")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long Phone1No { get; set; }

    [JsonProperty("phone2typesid")]
    public string Phone2Typesid { get; set; }

    [JsonProperty("phone2no")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long Phone2No { get; set; }

    [JsonProperty("active")]
    public bool Active { get; set; }

    [JsonProperty("seqno")]
    public long Seqno { get; set; }

    [JsonProperty("title")]
    public object Title { get; set; }

    [JsonProperty("phonetype1")]
    public string Phonetype1 { get; set; }

    [JsonProperty("phonetype2")]
    public string Phonetype2 { get; set; }

    [JsonProperty("emailtype")]
    public object Emailtype { get; set; }

    [JsonProperty("titleid")]
    public object Titleid { get; set; }
}