namespace SAPLink.Core.Models;

public class RequestResult<T> 
{
    public string Message { get; set; }
    public string StatusBarMessage { get; set; }
    public StatusType Status { get; set; } = StatusType.Failed;
    public List<T> EntityList { get; set; } = new();

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
    public RequestResult(StatusType status, string message, string statusBarMessage, List<T> entityList, IRestResponse response)
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