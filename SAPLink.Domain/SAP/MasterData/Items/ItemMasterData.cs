using SAPLink.Domain.Utilities;

namespace SAPLink.Domain.SAP.MasterData.Items;

public class ItemMasterData
{
    public ItemMasterData()
    {

    }
    private string _cardCode;
    private string _cardName;

    [Required]
    public string ItemCode { get; set; } // OITM.ItemCode

    [Required]
    public bool Active { get; set; } // OITM.validFor if Y  (0 = Not Active; 1 = Active) in Prism

    public string ColorCode { get; set; } // Code From table @AICL
    public string InventoryUoM { get; set; } // OITM.InvntryUom

    [Required]
    public string DesigGroupName { get; set; } // OITM.U_DesigGrpN
    public string Size { get; set; } // OITM.U_SIZE
    public string ColorGroup { get; set; } // OITM.U_ColorGrpN
    public string ProductGroupName { get; set; } // OITM.U_ProdGrpN
    public string ItemName { get; set; } // OITM.ItemName

    [Required]
    public string CardCode  // OITM.CardCode >> Preferred Vendor If Exist. If NOT "001"
    {
        get => _cardCode;
        set => _cardCode = value.IsNullOrEmpty() ? "001" : value;
    }

    public string CardName  // "OCRD.CardName >> Preferred Vendor Name If Exist. If NOT  "ALKAFFARY"
    {
        get => _cardName;
        set => _cardName = value.IsNullOrEmpty() ? "ALKAFFARY" : value;
    }

    [Required]
    public string ItemGroupCode { get; set; } // OITB.ItmsGrpNO
    public string ItemGroupName { get; set; } // OITM.ItmsGrpNam


    public string IsTaxable { get; set; } // OITM.VATLiable if Y --> Taxable or N --> Exempt"

    public bool InvntoryItem { get; set; } // OITM.InvnItem >> if inventory item or value Y Select 0 (0-YES 1- SERVICE) in Prism"

    public double AveragePrice { get; set; } // OITM.AvgPrice 
    public int PriceListCode { get; set; }
    public string SalesPrice { get; set; }

    //public ItemPrice ItemPrice { get; set; } // 1 for 1 and 2 for 2 an so on
    public string ForeignName { get; set; } // OITM.ForeignName
    public string OrignGroupName { get; set; } // OITM.U_OrignGrpN
    public string Sticker { get; set; }  // OITM.U_Sticker
    public string StickerForeign { get; set; } // OITM.U_StickerF
    public string TypeGroupName { get; set; } // OITM.U_TypeGrpN
    public string BarCode { get; set; } // OITM.CodeBars
    public string ItemsPerSaleUoM { get; set; } // OITM.NumInSale
    public string SalesUoM { get; set; } // OITM.SalUnitMsr
}