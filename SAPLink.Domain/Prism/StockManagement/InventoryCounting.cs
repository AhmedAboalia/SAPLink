using SAPLink.Domain.Common;
using SAPLink.Domain.Utilities;

namespace SAPLink.Domain.Models.Prism.StockManagement
{
    public partial class InventoryPosting
    {
        [JsonProperty("sid")]
        public string Sid { get; set; }

        [JsonProperty("rowversion")]
        public long Rowversion { get; set; }

        [JsonProperty("adjno")]
        public long Adjno { get; set; }

        [JsonProperty("adjtype")]
        public long Adjtype { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("adjitem")]
        public Adjitem[] Adjitem { get; set; }

        [JsonProperty("storecode")]
        public string StoreCode { get; set; }  
        
        [JsonProperty("createddatetime")]
        public string CreateDate { get; set; }
    }

    public partial class Adjitem
    {
        [JsonProperty("sid")]
        public string Sid { get; set; }

        [JsonProperty("rowversion")]
        public long Rowversion { get; set; }

        [JsonProperty("itemsid")]
        public string Itemsid { get; set; }

        [JsonProperty("origvalue")]
        public double Origvalue { get; set; }

        [JsonProperty("adjvalue")]
        public double Adjvalue { get; set; }

        [JsonProperty("cost")]
        public long Cost { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("description1")]
        public string Description1 { get; set; }

        [JsonProperty("description2")]
        public string Description2 { get; set; }

        [JsonProperty("alu")]
        public string Alu { get; set; }

        [JsonProperty("size")]
        public string SalesPerUnitFactor { get; set; }
    }

    public partial class InventoryPosting
    {
        public static Response<InventoryPosting> FromJson(string json) => JsonConvert.DeserializeObject<Response<InventoryPosting>>(json, Converter.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson(this InventoryPosting self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
