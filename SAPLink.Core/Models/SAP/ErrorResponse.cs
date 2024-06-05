using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPLink.Core.Models.SAP
{
    public class ErrorResponse
    {
        [JsonProperty("error")]
        public ErrorDetails Error { get; set; }
    }

    public class ErrorDetails
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public ErrorMessage Message { get; set; }
    }

    public class ErrorMessage
    {
        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
