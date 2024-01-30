namespace SAPLink.Domain.Models;

public class Logger<T>
{
    [Key]
    public Guid Id;
    public string RequestBody { get; set; }
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public string ErrorMessage { get; set; }
    public StatusType Status { get; set; }

    public Logger(StatusType status, string requestBody, string errorMessage)
    {
        //var product = JsonConvert.DeserializeObject<Product>(requestBody);
        Status = status;
        RequestBody = requestBody;
        ErrorMessage = errorMessage;
    }


    public void Log(RequestResult<T> requestResult)
    {
        Id = new Guid();
        Status = requestResult.Status;
        RequestBody = requestResult.Message;
        ErrorMessage = requestResult.Message;
    }
}