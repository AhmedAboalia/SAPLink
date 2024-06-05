

using SAPLink.Core.Utilities;

namespace SAPLink.Core.Models.SAP.MasterData.Items;

public class ItemMasterDataReport
{
    public ItemMasterDataReport()
    {

    }
    [Display(Name = "No.")]
    public int RowNumber { get; set; }

    [Display(Name = "Item Code")]
    public string ItemCode { get; set; }

    [Display(Name = "Item Name")]
    public string ItemName { get; set; }

    [Display(Name = "Foreign Name")]
    public string ForeignName { get; set; }

}