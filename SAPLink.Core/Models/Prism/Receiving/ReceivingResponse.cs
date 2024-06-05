namespace SAPLink.Core.Models.Prism.Receiving;

public class ReceivingResponse
{
    [JsonProperty("originapplication")]
    public string Originapplication { get; set; }

    [JsonProperty("voureasonsid")]
    public object Voureasonsid { get; set; }

    [JsonProperty("sbssid")]
    public string Sbssid { get; set; }

    [JsonProperty("clerksid")]
    public string Clerksid { get; set; }

    [JsonProperty("storesid")]
    public string Storesid { get; set; }

    [JsonProperty("publishstatus")]
    public long Publishstatus { get; set; }

    [JsonProperty("voutype")]
    public long Voutype { get; set; }

    [JsonProperty("vouclass")]
    public long Vouclass { get; set; }

    public static string CreateBody(string sbssid, string Clerksid, string storeSid)
    {
        // Create a new instance of the request body
        var requestBody = new RequestBody<ReceivingResponse>();

        // Create a new instance of the data item
        var dataItem = new ReceivingResponse
        {
            Originapplication = "RProPrismWeb",
            Voureasonsid = null,
            Sbssid = sbssid,
            Clerksid = Clerksid,
            Storesid = storeSid,
            Publishstatus = 1,
            Vouclass = 0,
            Voutype = 0
        };

        // Add the data item to the request body
        requestBody.Data = new[] { dataItem };

        // Convert the request body to JSON
        return JsonConvert.SerializeObject(requestBody);
    }
}