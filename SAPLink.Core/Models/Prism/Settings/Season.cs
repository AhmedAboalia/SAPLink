namespace SAPLink.Core.Models.Prism.Settings;

public class Season
{
    [Key]
    [JsonProperty("sid")]
    public string Sid { get; set; }

    [JsonProperty("rowversion")]
    public long RowVersion { get; set; }


    [JsonProperty("seasoncode")]
    public string SeasonCode { get; set; }

    [JsonProperty("seasonname")]
    public string Seasonname { get; set; }

    [JsonProperty("active")]
    public bool Active { get; set; }

    [JsonProperty("seasonid")]
    public int SeasonId { get; set; }

    public Season(string sid, string seasonCode, string seasonName, int seasonId, bool active)
    {
        Sid = sid;
        SeasonCode = seasonCode;
        Seasonname = seasonName;
        SeasonId = seasonId;
        Active = active;
    }

    public Season()
    {
            
    }
}