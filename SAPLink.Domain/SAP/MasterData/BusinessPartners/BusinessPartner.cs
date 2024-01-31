namespace SAPLink.Domain.SAP.MasterData.BusinessPartners;

public class BusinessPartner
{
    [JsonProperty("CardCode")]
    public string CardCode { get; set; } = null!;

    [JsonProperty("CardName")]
    public string CardName { get; set; }

    //[JsonProperty("CreateDate")]
    //public string CreateDate { get; set; }

    //[JsonProperty("UpdateDate")]
    //public string UpdateDate { get; set; }
}