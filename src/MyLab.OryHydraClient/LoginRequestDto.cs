using Newtonsoft.Json;

namespace MyLab.OryHydraClient
{
    /// <summary>
    /// Contains login request details
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// ID is the identifier ("login challenge") of the login request. 
        /// </summary>
        [JsonProperty("challenge")]
        public string Challenge { get; set; }

        [JsonProperty("client")]
        public ClientDto Client { get; set; }

        [JsonProperty("oidc_context")]
        public OidcContext OidcContext { get; set; }

        /// <summary>
        /// RequestURL is the original OAuth 2.0 Authorization URL requested by the OAuth 2.0 client. It is the URL which initiates the OAuth 2.0 Authorization Code or OAuth 2.0 Implicit flow
        /// </summary>
        [JsonProperty("request_url")]
        public string RequestUrl { get; set; }

        [JsonProperty("requested_access_token_audience")]
        public string[] RequestedAccessTokenAudience { get; set; }

        [JsonProperty("requested_scope")]
        public string[] RequestedScope { get; set; }

        /// <summary>
        /// SessionID is the login session ID
        /// </summary>
        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        /// <summary>
        /// Skip, if true, implies that the client has requested the same scopes from the same user previously.
        /// </summary>
        [JsonProperty("skip")]
        public bool Skip { get; set; }

        /// <summary>
        /// Subject is the user ID of the end-user that authenticated.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }


    }
}