namespace SAPLink.Core.Models;

public class Error
{
    public DateTime Date { get; set; }
    public string Class { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public int HttpStatusCode { get; set; }
    public string HttpMessage { get; set; }
    public string FunctionName { get; set; }
    public object ParamValues { get; set; }
}
