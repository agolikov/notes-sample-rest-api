using Newtonsoft.Json;

namespace notes.api.Models
{
    public class ApiErrorResponse
    {
        [JsonProperty("id")]
        public string EntityId { get; set; }

        [JsonProperty("code")]
        public string ResponseCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("details")]
        public string[] Details { get; set; }
    }
}