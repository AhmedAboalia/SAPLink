using SAPLink.Core.Utilities;

namespace SAPLink.Core.Models.Prism.Settings;

public class Store
{
    //[JsonProperty("link")]
    //public string Link { get; set; }

    //[Key]
    [JsonProperty("sid")]
    public string Sid { get; set; }

    //[JsonProperty("subsidiary_sid")]
    //public long SubsidiarySid { get; set; }

    [JsonProperty("store_number")]
    //[JsonConverter(typeof(ParseStringConverter))]
    public int StoreNumber { get; set; }

    [JsonProperty("store_name")]
    public string StoreName { get; set; }

    [JsonProperty("active")]
    public bool Active { get; set; }

    [JsonProperty("store_code")]
    public string StoreCode { get; set; }

    // This is the property that will hold the combined value
    private string displayMember;
    public string DisplayMember
    {
        get
        {
            // If displayMember is null or empty, return combined value
            // Otherwise, return displayMember directly
            return string.IsNullOrEmpty(displayMember) ? $"{StoreCode} - {StoreName}" : displayMember;
        }
        set
        {
            displayMember = value;
        }
    }

    public static List<Store> FromJson(string json) => JsonConvert.DeserializeObject<List<Store>>(json, Converter.Settings);
    public static string ToJson(List<Store> self) => JsonConvert.SerializeObject(self, Converter.Settings);
    //[JsonProperty("active_price_level_sid")]
    //public string ActivePriceLevelSid { get; set; }

    public Store(string sid, long sbssid, long storeNum, string storeName, string storeCode, string activePriceLevelSid, bool active)
    {
        Sid = sid;
        //SubsidiarySid = sbssid;
        //StoreNumber = storeNum;
        StoreName = storeName;
        StoreCode = storeCode;
        //ActivePriceLevelSid = activePriceLevelSid;
        Active = active;
    }

    public Store()
    {

    }
        
    public static class Serialize
    {
           
    }

}