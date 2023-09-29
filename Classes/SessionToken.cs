using Newtonsoft.Json;

namespace AEAQuiz.Classes
{
    public class SessionToken
    {

        [JsonProperty("response_code")]
        public int ResponseCode { get; set; }

        [JsonProperty("response_message")]
        public string ResponseMessage { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}

