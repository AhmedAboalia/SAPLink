namespace SAPLink.Domain.Models.Prism.Receiving;

public class ReceivingResponseDto
{
    [JsonProperty("sid")]
    public string Sid { get; set; }

    [JsonProperty("rowversion")]
    public string RowVersion { get; set; }

    [JsonProperty("note")]
    public string Note { get; set; }

    [JsonProperty("trackingno")]
    public string TrackingNo { get; set; }
}