namespace SAPLink.Domain.Models.Prism.Sales;

public partial class Customers
{
    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("metatype", NullValueHandling = NullValueHandling.Ignore)]
    public string Metatype { get; set; }

    [JsonProperty("comment", NullValueHandling = NullValueHandling.Ignore)]
    public string Comment { get; set; }

    [JsonProperty("translationid", NullValueHandling = NullValueHandling.Ignore)]
    public string Translationid { get; set; }

    [JsonProperty("errors")]
    public object Errors { get; set; }

    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    public List<Datum> Data { get; set; }
}

public partial class Datum
{
    [JsonProperty("sid", NullValueHandling = NullValueHandling.Ignore)]
    public string Sid { get; set; }

    [JsonProperty("rowversion", NullValueHandling = NullValueHandling.Ignore)]
    public long? Rowversion { get; set; }

    [JsonProperty("custid", NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(ParseStringConverter))]
    public long? Custid { get; set; }

    [JsonProperty("lastname")]
    public string Lastname { get; set; }

    [JsonProperty("firstname", NullValueHandling = NullValueHandling.Ignore)]
    public string Firstname { get; set; }

    [JsonProperty("active", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Active { get; set; }

    [JsonProperty("udf1string")]
    public string Udf1String { get; set; }

    [JsonProperty("udf2string")]
    public string Udf2String { get; set; }

    [JsonProperty("udf3string")]
    public string Udf3String { get; set; }

    [JsonProperty("custaddress", NullValueHandling = NullValueHandling.Ignore)]
    public List<Custaddress> Custaddress { get; set; }

    [JsonProperty("custemail", NullValueHandling = NullValueHandling.Ignore)]
    public List<Cust> Custemail { get; set; }

    [JsonProperty("custphone", NullValueHandling = NullValueHandling.Ignore)]
    public List<Cust> Custphone { get; set; }

    [JsonProperty("fullname", NullValueHandling = NullValueHandling.Ignore)]
    public string Fullname { get; set; }

    [JsonProperty("fromcentrals", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Fromcentrals { get; set; }
}

public partial class Custaddress
{
    [JsonProperty("sid", NullValueHandling = NullValueHandling.Ignore)]
    public string Sid { get; set; }

    [JsonProperty("createdby", NullValueHandling = NullValueHandling.Ignore)]
    public string Createdby { get; set; }

    [JsonProperty("createddatetime", NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset? Createddatetime { get; set; }

    [JsonProperty("modifiedby")]
    public Edby? Modifiedby { get; set; }

    [JsonProperty("modifieddatetime")]
    public DateTimeOffset? Modifieddatetime { get; set; }

    [JsonProperty("controllersid", NullValueHandling = NullValueHandling.Ignore)]
    public string Controllersid { get; set; }

    [JsonProperty("originapplication", NullValueHandling = NullValueHandling.Ignore)]
    public Originapplication? Originapplication { get; set; }

    [JsonProperty("postdate", NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset? Postdate { get; set; }

    [JsonProperty("rowversion", NullValueHandling = NullValueHandling.Ignore)]
    public long? Rowversion { get; set; }

    [JsonProperty("tenantsid", NullValueHandling = NullValueHandling.Ignore)]
    public string Tenantsid { get; set; }

    [JsonProperty("custsid", NullValueHandling = NullValueHandling.Ignore)]
    public string Custsid { get; set; }

    [JsonProperty("primaryflag", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Primaryflag { get; set; }

    [JsonProperty("active", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Active { get; set; }

    [JsonProperty("addressname")]
    public object Addressname { get; set; }

    [JsonProperty("companyname")]
    public object Companyname { get; set; }

    [JsonProperty("address1")]
    public string Address1 { get; set; }

    [JsonProperty("address2")]
    public string Address2 { get; set; }

    [JsonProperty("address3")]
    public object Address3 { get; set; }

    [JsonProperty("city")]
    public object City { get; set; }

    [JsonProperty("state")]
    public object State { get; set; }

    [JsonProperty("postalcode")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long? Postalcode { get; set; }

    [JsonProperty("postalcodeextension")]
    public object Postalcodeextension { get; set; }

    [JsonProperty("countrysid")]
    public string Countrysid { get; set; }

    [JsonProperty("begindate")]
    public object Begindate { get; set; }

    [JsonProperty("enddate")]
    public object Enddate { get; set; }

    [JsonProperty("seasonalbegindate")]
    public object Seasonalbegindate { get; set; }

    [JsonProperty("seasonalenddate")]
    public object Seasonalenddate { get; set; }

    [JsonProperty("addresstypesid")]
    public string Addresstypesid { get; set; }

    [JsonProperty("addresscode")]
    public object Addresscode { get; set; }

    [JsonProperty("billship")]
    public object Billship { get; set; }

    [JsonProperty("phonesid")]
    public object Phonesid { get; set; }

    [JsonProperty("altphonesid")]
    public object Altphonesid { get; set; }

    [JsonProperty("addressallowcontact", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Addressallowcontact { get; set; }

    [JsonProperty("address4")]
    public object Address4 { get; set; }

    [JsonProperty("address5")]
    public object Address5 { get; set; }

    [JsonProperty("address6")]
    public object Address6 { get; set; }

    [JsonProperty("taxareasid")]
    public object Taxareasid { get; set; }

    [JsonProperty("taxarea2sid")]
    public object Taxarea2Sid { get; set; }

    [JsonProperty("addressline1")]
    public object Addressline1 { get; set; }

    [JsonProperty("seqno", NullValueHandling = NullValueHandling.Ignore)]
    public long? Seqno { get; set; }

    [JsonProperty("taxarea")]
    public object Taxarea { get; set; }

    [JsonProperty("taxarea2")]
    public object Taxarea2 { get; set; }

    [JsonProperty("addresstype")]
    public string Addresstype { get; set; }

    [JsonProperty("countrycode")]
    public string Countrycode { get; set; }
}

public partial class Cust
{
    [JsonProperty("sid", NullValueHandling = NullValueHandling.Ignore)]
    public string Sid { get; set; }

    [JsonProperty("createdby", NullValueHandling = NullValueHandling.Ignore)]
    public Edby? Createdby { get; set; }

    [JsonProperty("createddatetime", NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset? Createddatetime { get; set; }

    [JsonProperty("modifiedby")]
    public Edby? Modifiedby { get; set; }

    [JsonProperty("modifieddatetime")]
    public DateTimeOffset? Modifieddatetime { get; set; }

    [JsonProperty("controllersid", NullValueHandling = NullValueHandling.Ignore)]
    public string Controllersid { get; set; }

    [JsonProperty("originapplication", NullValueHandling = NullValueHandling.Ignore)]
    public Originapplication? Originapplication { get; set; }

    [JsonProperty("postdate", NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset? Postdate { get; set; }

    [JsonProperty("rowversion", NullValueHandling = NullValueHandling.Ignore)]
    public long? Rowversion { get; set; }

    [JsonProperty("tenantsid", NullValueHandling = NullValueHandling.Ignore)]
    public string Tenantsid { get; set; }

    [JsonProperty("custsid", NullValueHandling = NullValueHandling.Ignore)]
    public string Custsid { get; set; }

    [JsonProperty("emailaddress", NullValueHandling = NullValueHandling.Ignore)]
    public string Emailaddress { get; set; }

    [JsonProperty("description")]
    public object Description { get; set; }

    [JsonProperty("emailallowcontact", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Emailallowcontact { get; set; }

    [JsonProperty("begindate")]
    public object Begindate { get; set; }

    [JsonProperty("enddate")]
    public object Enddate { get; set; }

    [JsonProperty("extension")]
    public object Extension { get; set; }

    [JsonProperty("primaryflag", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Primaryflag { get; set; }

    [JsonProperty("seqno", NullValueHandling = NullValueHandling.Ignore)]
    public long? Seqno { get; set; }

    [JsonProperty("emailtypesid")]
    public object Emailtypesid { get; set; }

    [JsonProperty("emailtype")]
    public object Emailtype { get; set; }

    [JsonProperty("phoneno", NullValueHandling = NullValueHandling.Ignore)]
    public string Phoneno { get; set; }

    [JsonProperty("phoneallowcontact", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Phoneallowcontact { get; set; }

    [JsonProperty("phonetypesid")]
    public object Phonetypesid { get; set; }

    [JsonProperty("phonetype")]
    public object Phonetype { get; set; }
}

public enum Edby { Sysadmin };

public enum Originapplication { RProPrismWeb };

public partial class Customers
{
    public static Customers FromJson(string json) => JsonConvert.DeserializeObject<Customers>(json, Converter.Settings);
}

public static partial class Serialize
{
    public static string ToJson(this Customers self) => JsonConvert.SerializeObject(self, Converter.Settings);
}