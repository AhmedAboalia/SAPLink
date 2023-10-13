namespace SAPLink.Core.Models.Prism.Receiving;

public static class ReceivingRequest
{
    public static string CreateBody(string Clerksid, string rowVersion, string trackingNo, string note, string storeCode)
    {
        // Create a new instance of the request body
        var requestBody = new RequestBody<AddReceiving>();


        // Create a new instance of the data item
        var dataItem = new AddReceiving
        {
            RowVersion = Convert.ToInt64(rowVersion),
            Status = 4,
            ApprovBySid = Clerksid,
            //ApprovDate = DateTime.Parse("2023-06-13T13:07:56.115Z"),
            ApprovStatus = 2,
            PublishStatus = 2,
            StoreSid = storeCode,
            Trackingno = trackingNo,
            Note = note
        };

        // Add the data item to the request body
        requestBody.Data = new AddReceiving[] { dataItem };

        // Convert the request body to JSON
        return JsonConvert.SerializeObject(requestBody);
    }
    public static string CreateBody2(string Clerksid, string rowVersion, string note, string storeCode)
    {
        // Create a new instance of the request body
        var requestBody = new RequestBody<AddReceiving>();


        // Create a new instance of the data item
        var dataItem = new AddReceiving
        {
            RowVersion = Convert.ToInt64(rowVersion),
            Status = 4,
            ApprovBySid = Clerksid,
            //ApprovDate = DateTime.Parse("2023-06-13T13:07:56.115Z"),
            ApprovStatus = 2,
            PublishStatus = 2,
            StoreSid = storeCode,
            //Trackingno = Convert.ToInt32(trackingNo),
            Note = note
        };

        // Add the data item to the request body
        requestBody.Data = new AddReceiving[] { dataItem };

        // Convert the request body to JSON
        return JsonConvert.SerializeObject(requestBody);
    }
}

public class RequestBody<T>
{
    [JsonProperty("data")]
    public T[] Data { get; set; }
}
public class AddReceiving
{
    [JsonProperty("rowversion")]
    public long RowVersion { get; set; }

    [JsonProperty("status")]
    public long Status { get; set; }

    [JsonProperty("approvbysid")]
    public string ApprovBySid { get; set; }

    //[JsonProperty("approvdate")]
    //public DateTime ApprovDate { get; set; }

    [JsonProperty("approvstatus")]
    public int ApprovStatus { get; set; } = 2;

    [JsonProperty("publishstatus")]
    public int PublishStatus { get; set; } = 2;

    [JsonProperty("trackingno")]
    public string Trackingno { get; set; }

    [JsonProperty("note")]
    public string Note { get; set; }

    [JsonProperty("storesid")]
    public string StoreSid { get; set; }
}