using SAPLink.Domain.Models.Prism.Merchandise.Inventory;
using SAPLink.Domain.Models.SAP.Documents;

namespace SAPLink.Domain.Models.Prism.Receiving;

public static class ReceivingItemRequest
{
    public static string CreateBody(ProductResponseModel product, string Vousid, Line line)
    {
        // Create a new instance of the request body
        var requestBody = new ReceivingItemData();

        // Create a new instance of the data item
        var dataItem = new Datum
        {
            ReceivingItem = new ReceivingItem()
            {
                Itemsid = product.Sid,
                Description1 = line.ItemName,
                //Price = Product.actstrprice,
                Price = Convert.ToInt64(line.Price),
                Qty = Convert.ToInt64(line.Quantity),
                Vousid = Vousid,
                Upc = product.Upc,
            }
        };

        // Add the data item to the request body
        requestBody.Data = new Datum[] { dataItem };

        // Convert the request body to JSON
        return JsonConvert.SerializeObject(requestBody);
    }
}

public class Datum
{
    [JsonProperty("recvItem")]
    public ReceivingItem ReceivingItem { get; set; }
}

public class ReceivingItemData
{
    [JsonProperty("data")]
    public Datum[] Data { get; set; }
}

public class ReceivingItem
{
    [JsonProperty("resource")]
    public string Resource { get; set; } = "recvitem";

    [JsonProperty("endpoint")]
    public string Endpoint { get; set; } = "backoffice/receiving/:vousid/recvitem/:sid/";

    [JsonProperty("originapplication")]
    public string Originapplication { get; set; } = "RProPrismWeb";

    [JsonProperty("itemsid")]
    public string Itemsid { get; set; }

    [JsonProperty("qty")]
    public long Qty { get; set; }

    [JsonProperty("upc")]
    public string Upc { get; set; }

    [JsonProperty("description1")]
    public string Description1 { get; set; }

    [JsonProperty("serialno")]
    public object Serialno { get; set; } = null;

    [JsonProperty("lotnumber")]
    public object Lotnumber { get; set; } = null;

    [JsonProperty("serialtype")]
    public long Serialtype { get; set; } = 0;

    [JsonProperty("lottype")]
    public long Lottype { get; set; } = 0;

    [JsonProperty("price")]
    public long Price { get; set; } = 0;

    [JsonProperty("vousid")]
    public string Vousid { get; set; }
}