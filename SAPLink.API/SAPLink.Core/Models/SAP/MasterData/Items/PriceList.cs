namespace SAPLink.Core.Models.SAP.MasterData.Items;

public class PriceList
{
    [JsonProperty("PriceListNo")]
    public int PriceListNo { get; set; }

    [JsonProperty("PriceListName")]
    public string PriceListName { get; set; }
}