namespace SAPLink.Application.SAP.Connection
{
    /// <summary>
    /// Container for Hold Service Layer Session Data
    /// </summary>
    public class Session
    {
        [JsonProperty("SessionId")]
        public string Id;

        [JsonProperty("Version")]
        public string Version;

        [JsonProperty("SessionTimeout")]
        public int Timeout;
    }
}
