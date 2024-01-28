namespace SAPLink.Core.Models;

public class RequestResult<T> 
{
    public string Message { get; set; }
    public string StatusBarMessage { get; set; }
    public StatusType Status { get; set; } = StatusType.Failed;
    public List<T> EntityList { get; set; } = new();
    public Responses Response { get; set; } = new Responses();

    public RequestResult(StatusType status, string message, string statusBarMessage, List<T> entityList, Responses response)
    {
        Status = status;
        Message = message;
        StatusBarMessage = statusBarMessage;
        EntityList = entityList;
        Response = response;
    }

    public RequestResult()
    {

    }
}

public class Responses : IRestResponse
{
    public IRestResponse Response = new RestResponse();
    public int PagesCount = 0;

    public void GetContentRange(string range, int pageSize)
    {
        if (range.Contains("/"))
        {
            var pageCount = Convert.ToInt32(range.Split("/")[1]);
            PagesCount = (pageCount % pageSize == 0) ? (pageCount / pageSize) : ((pageCount / pageSize) + 1);
        }
    }

    public IRestRequest Request { get; set; }
    public string ContentType { get; set; }
    public long ContentLength { get; set; }
    public string ContentEncoding { get; set; }
    public string Content { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccessful { get; }
    public string StatusDescription { get; set; }
    public byte[] RawBytes { get; set; }
    public Uri ResponseUri { get; set; }
    public string Server { get; set; }
    public IList<RestResponseCookie> Cookies { get; }
    public IList<Parameter> Headers { get; }
    public ResponseStatus ResponseStatus { get; set; }
    public string ErrorMessage { get; set; }
    public Exception? ErrorException { get; set; }
    public Version ProtocolVersion { get; set; }
}