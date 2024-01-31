namespace SAPLink.Domain.Models.Prism.Settings;

public class PriceLevel
{
    [Key]
    [JsonProperty("sid")]
    public string Sid { get; set; }


    [JsonProperty("originapplication")]
    public string Originapplication { get; set; } = "RProPrismWeb";

    [JsonProperty("sbssid")]
    public long Sbssid { get; set; }

    [JsonProperty("pricelvl")]
    public int Pricelvl { get; set; }

    [JsonProperty("pricelvlname")]
    public string Pricelvlname { get; set; }

    [JsonProperty("active")]
    public bool Active { get; set; }

    public PriceLevel(string sid, long sbssid, int priceLevel, string pricelvlname, bool active)
    {
        Sid = sid;
        Sbssid = sbssid;
        Pricelvl = priceLevel;
        Pricelvlname = pricelvlname;
        Active = active;
    }

    public PriceLevel()
    {
    }
}

//public class PriceLevel
//{
//    [Key]
//    [JsonProperty("sid")]
//    public string Sid { get; set; }

//    [JsonProperty("originapplication")]
//    public string Originapplication { get; set; }

//    [JsonProperty("rowversion")]
//    public int Rowversion { get; set; }

//    [JsonProperty("sbssid")]
//    [ForeignKey("SubsidiarySid")]
//    public string Sbssid { get; set; }

//    [JsonProperty("pricelvl")]
//    public int Pricelvl { get; set; }

//    [JsonProperty("pricelvlname")]
//    public string Pricelvlname { get; set; }

//    [JsonProperty("secured")]
//    public bool Secured { get; set; }

//    //[JsonProperty("discperc")] public string Discperc { get; set; } = null;

//    [JsonProperty("usediscperc")]
//    public bool Usediscperc { get; set; }

//    [JsonProperty("active")]
//    public bool Active { get; set; }

//    //[JsonProperty("pricelvldescription")]
//    //public string Pricelvldescription { get; set; }

//    [JsonProperty("sbsno")]
//    public int Sbsno { get; set; }
//}