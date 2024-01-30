namespace SAPLink.Domain.Models.Prism.Merchandise.Departments;

public class Department
{
    [JsonProperty("originapplication")]
    public string OriginApplication { get; set; }

    [JsonProperty("sid")]
    public string Sid { get; set; }

    [JsonProperty("rowversion")]
    public long RowVersion { get; set; }


    [JsonProperty("dcscode")]
    public string DcsCode { get; set; }

    [JsonProperty("sbssid")]
    public long SbsSid { get; set; }

    [JsonProperty("active")]
    public bool Active { get; set; }

    [JsonProperty("dname")]
    public string DepartmentName { get; set; }

    [JsonProperty("cname")]
    public string Cname { get; set; }

    [JsonProperty("sname")]
    public string Sname { get; set; }

    [JsonProperty("dlongname")]
    public string Dlongname { get; set; }

    [JsonProperty("clongname")]
    public string Clongname { get; set; }

    [JsonProperty("slongname")]
    public string Slongname { get; set; }


    [JsonProperty("regional")]
    public bool Regional { get; set; }

    //[JsonIgnore]
    [JsonProperty("publishstatus")]
    public long Publishstatus { get; set; }

    [JsonProperty("d")]
    public string D { get; set; }

    [JsonProperty("c")]
    public string C { get; set; }

    [JsonProperty("s")]
    public object S { get; set; }
}

//public class DepartmentModel
//{
//    [JsonProperty("sid")]
//    public string Sid { get; set; }

//    [JsonIgnore]
//    [JsonProperty("createdby")]
//    public string Createdby { get; set; }

//    [JsonIgnore]
//    [JsonProperty("createddatetime")]
//    public DateTimeOffset Createddatetime { get; set; }

//    [JsonIgnore]
//    [JsonProperty("modifiedby")]
//    public string Modifiedby { get; set; }

//    [JsonIgnore]
//    [JsonProperty("modifieddatetime")]
//    public DateTimeOffset Modifieddatetime { get; set; }

//    [JsonIgnore]
//    [JsonProperty("controllersid")]
//    public string Controllersid { get; set; }

//    [JsonProperty("originapplication")]
//    public string Originapplication { get; set; }

//    [JsonIgnore]
//    [JsonProperty("postdate")]
//    public DateTimeOffset Postdate { get; set; }


//    [JsonProperty("rowversion")]
//    public long Rowversion { get; set; }

//    [JsonIgnore]
//    [JsonProperty("tenantsid")]
//    public string Tenantsid { get; set; }

//    [JsonProperty("dcscode")]
//    public string Dcscode { get; set; }

//    [JsonProperty("sbssid")]
//    public long Sbssid { get; set; }

//    [JsonProperty("useqtydecimals")]
//    public long Useqtydecimals { get; set; }

//    [JsonProperty("taxcodesid")]
//    public string Taxcodesid { get; set; }

//    [JsonIgnore]
//    [JsonProperty("margintype")]
//    public int Margintype { get; set; }

//    [JsonProperty("active")]
//    public int Active { get; set; }

//    [JsonIgnore]
//    [JsonProperty("marginvalue")]
//    public int Marginvalue { get; set; }

//    [JsonIgnore]
//    [JsonProperty("patternsid")]
//    public object Patternsid { get; set; }

//    [JsonProperty("dname")]
//    public string Dname { get; set; }

//    [JsonProperty("cname")]
//    public string Cname { get; set; }

//    [JsonProperty("sname")]
//    public string Sname { get; set; }

//    [JsonProperty("dlongname")]
//    public string Dlongname { get; set; }

//    [JsonProperty("clongname")]
//    public string Clongname { get; set; }

//    [JsonProperty("slongname")]
//    public string Slongname { get; set; }

//    [JsonProperty("tagcodesid")]
//    public object Tagcodesid { get; set; }

//    [JsonProperty("regional")]
//    public bool Regional { get; set; }

//    [JsonIgnore]
//    [JsonProperty("image")]
//    public object Image { get; set; }

//    [JsonIgnore]
//    [JsonProperty("publishstatus")]
//    public long Publishstatus { get; set; }

//    [JsonProperty("d")]
//    public string D { get; set; }

//    [JsonProperty("c")]
//    public string C { get; set; }

//    [JsonProperty("s")]
//    public object S { get; set; }

//    [JsonIgnore]
//    [JsonProperty("marginpctg")]
//    public long Marginpctg { get; set; }

//    [JsonIgnore]
//    [JsonProperty("markuppctg")]
//    public long Markuppctg { get; set; }

//    [JsonIgnore]
//    [JsonProperty("coefficient")]
//    public long Coefficient { get; set; }

//    [JsonProperty("taxcode")]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long Taxcode { get; set; }

//    [JsonProperty("sbsno")]
//    public long Sbsno { get; set; }

//    [JsonIgnore]
//    [JsonProperty("patternname")]
//    public object Patternname { get; set; }

//    [JsonProperty("taxname")]
//    public string Taxname { get; set; }
//}