namespace SAPLink.Core.Models.SAP.Documents;

public class Goods
{
    public string DocNum { get; set; }
    public string DocEntry { get; set; }
    public string CardCode { get; set; }
    public string CardName { get; set; }
    public string WarehouseCode { get; set; }
    public string Remarks { get; set; }
    public List<Line> Lines { get; set; }
}
public class Line
{
    public string DocEntry { get; set; }
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public string WarehouseCode { get; set; }
}