using Newtonsoft.Json;

namespace MyLab.OryHydraClient
{
    /// <summary>
    /// Contains logout request details
    /// </summary>
    public class LogoutRequestDto
    {
        /// <summary>
        /// Challenge is the identifier ("logout challenge") of the logout authentication request. It is used to identify the session.
        /// </summary>
        [JsonProperty("challenge")]
        public string Challenge { get; set; }

        [JsonProperty("client")]
        public ClientDto Client { get; set; }

        /// <summary>
        /// RequestURL is the original Logout URL requested.
        /// </summary>
        [JsonProperty("request_url")]
        public string RequestUrl { get; set; }

        /// <summary>
        /// RPInitiated is set to true if the request was initiated by a Relying Party (RP), also known as an OAuth 2.0 Client
        /// </summary>
        [JsonProperty("rp_initiated")]
        public bool? RpInitiated { get; set; }

        /// <summary>
        /// SessionID is the login session ID that was requested to log out.
        /// </summary>
        [JsonProperty("sid")]
        public string Sid { get; set; }

        /// <summary>
        /// Subject is the user for whom the logout was request.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }
    }
}