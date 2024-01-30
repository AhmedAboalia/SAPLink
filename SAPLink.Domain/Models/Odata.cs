namespace SAPLink.Domain.Models
{
    public class OdataPrism<T>
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }

    public class OdataSAP<T>
    {
        [JsonProperty("odata.nextLink")]
        public string NextLink { get; set; }
        public List<T> Value { get; set; }
    }
}