namespace SAPLink.Core.Models.Prism.Inventory.Departments;

public class DepartmentSyncRequest
{
    [JsonProperty("rowversion")]
    public long Rowversion { get; set; }

    [JsonProperty("dname")]
    public string DepartmentName { get; set; }

    [JsonProperty("d")]
    public string Department { get; set; }
}