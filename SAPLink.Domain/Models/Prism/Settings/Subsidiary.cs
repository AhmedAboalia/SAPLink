namespace SAPLink.Domain.Models.Prism.Settings;

public class Subsidiary
{
    [Key]
    [JsonProperty("sid")]
    public long Sid { get; set; }

    [JsonProperty("subsidiary_number")]
    public long SubsidiaryNumber { get; set; }

    [JsonProperty("subsidiary_name")]
    public string SubsidiaryName { get; set; }

    [JsonProperty("active_price_level_sid")]
    public string ActivePriceLevelSid { get; set; }

    public List<PriceLevel> PriceLevel { get; set; }


    [JsonProperty("active_season_sid")]
    public string ActiveSeasonSid { get; set; }

    public List<Season> Season { get; set; }


    [JsonProperty("price_level_name")]
    public string PriceLevelName { get; set; }

}