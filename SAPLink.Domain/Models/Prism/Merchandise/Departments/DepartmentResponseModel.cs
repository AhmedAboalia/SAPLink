namespace SAPLink.Domain.Models.Prism.Merchandise.Departments;

public class DepartmentResponseModel
{
    [JsonProperty("originapplication")]
    public string Originapplication { get; set; }

    [JsonProperty("sid")]
    public string Sid { get; set; }

    [JsonProperty("d")]
    public string Department { get; set; }

    [JsonProperty("c")]
    public string Class { get; set; }

    [JsonProperty("s")]
    public string SubClass { get; set; }

    [JsonProperty("dcscode")]
    public string DcsCode { get; set; }

    //public string TaxCode { get; set; }

    [JsonProperty("rowversion")]
    public long Rowversion { get; set; }

    //[JsonProperty("publishstatus")]
    //public long Publishstatus { get; set; } = 2;

    //[JsonProperty("dname")]
    //public string DepartmentName { get; set; }

    //[JsonProperty("cname")]
    //public string ClassName { get; set; }

    //[JsonProperty("sname")]
    //public string SubClassName { get; set; }

    //[JsonProperty("dlongname")]
    //public string DepartmentLongName { get; set; }

    //[JsonProperty("clongname")]
    //public string ClassLongName { get; set; }

    //[JsonProperty("slongname")]
    //public string SubclassLongName { get; set; }
}