namespace SAPLink.Domain.SAP.MasterData.Items;

public class ItemGroups
{
    [JsonProperty("Number")]
    public string ItemGroupCode { get; set; }

    [JsonProperty("GroupName")]
    public string ItemGroupName { get; set; }

    //[JsonProperty("createDate")]
    //public string CreateDate { get; set; }

    //[JsonProperty("updateDate")]
    //public string UpdateDate { get; set; }
}