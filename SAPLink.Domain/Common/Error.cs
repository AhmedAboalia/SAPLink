using Newtonsoft.Json.Linq;

namespace SAPLink.Domain.Common;

//public class Error
//{
//    public DateTime Date { get; set; }
//    public string Class { get; set; }
//    public string ErrorCode { get; set; }
//    public string ErrorMessage { get; set; }
//    public int HttpStatusCode { get; set; }
//    public string HttpMessage { get; set; }
//    public string FunctionName { get; set; }
//    public object ParamValues { get; set; }
//}
public class Error
{
    [JsonProperty("date")]
    public DateTime Date { get; set; }

    [JsonProperty("class")]
    public string ErrorClass { get; set; }

    [JsonProperty("errorcode")]
    public string ErrorCode { get; set; }

    [JsonProperty("errormsg")]
    public string ErrorMessage { get; set; }

    [JsonProperty("httpcode")]
    public int HttpStatusCode { get; set; }

    [JsonProperty("httpmessage")]
    public string HttpMessage { get; set; }

    [JsonProperty("functionname")]
    public string FunctionName { get; set; }

    [JsonProperty("paramvalues")]
    public JArray ParamValues { get; set; }
}

