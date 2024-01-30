namespace SAPLink.Domain.Models;

public class RequestResult<T>
{
    public string Message { get; set; }
    public string StatusBarMessage { get; set; }
    public StatusType Status { get; set; } = StatusType.Failed;
    public List<T> EntityList { get; set; } = new();
    public IRestResponse Response { get; set; } = new RestResponse();



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