namespace SAPLink.Core.Models;

public class RequestResult<T>
{
    public string Message { get; set; }
    public string StatusBarMessage { get; set; }
    public StatusType Status { get; set; } = StatusType.Failed;
    public List<T> EntityList { get; set; } = new();

    public IRestResponse Response { get; set; } = new RestResponse();

    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public static int PageSize { get; set; } = 30;
    public int PageNo { get; set; } = 1;



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

    public int GetTotalItems(IRestResponse response)
    {
        if (response.Headers != null && 
            response.Headers.Any(h => 
            h.Name.Equals("Content-Range", StringComparison.OrdinalIgnoreCase) ||
             h.Name.Equals("Contentrange", StringComparison.OrdinalIgnoreCase) ||
            h.Name.Equals("ContentRange", StringComparison.OrdinalIgnoreCase)))
        {
            // Find the "Content-Range" header and retrieve its value
            var contentRangeHeader = response.Headers.First(h => 
            h.Name.Equals("Content-Range", StringComparison.OrdinalIgnoreCase) ||
            h.Name.Equals("Contentrange", StringComparison.OrdinalIgnoreCase) ||
            h.Name.Equals("ContentRange", StringComparison.OrdinalIgnoreCase));

            var contentRange = contentRangeHeader.Value.ToString();

            if (contentRange.Contains("/"))
            {
                return Convert.ToInt32(contentRange.Split("/")[1]);
            }
        }
        return -1;
    }  
    
    public int GetTotalPages(int totalItems)
    {
        return (totalItems % PageSize == 0) ? (totalItems / PageSize) : ((totalItems / PageSize) + 1);
    }


}