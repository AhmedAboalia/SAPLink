namespace SAPLink.Domain.Models.Prism.Receiving;

public partial class Comment
{
    [JsonProperty("data")]
    public Datum2[] Data { get; set; }
}

public partial class Datum2
{
    [JsonProperty("originapplication")]
    public string Originapplication { get; set; }

    [JsonProperty("comments")]
    public string Comments { get; set; }

    [JsonProperty("vousid")]
    public string Vousid { get; set; }
}

public partial class Comment
{
    public static Comment FromJson(string json) => JsonConvert.DeserializeObject<Comment>(json, Converter.Settings);

}

public static partial class Serialize
{
    public static string ToJson(this Comment self) => JsonConvert.SerializeObject(self, Converter.Settings);
}

