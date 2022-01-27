using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyLab.OryHydraClient
{
    /// <summary>
    /// Contains consent details
    /// </summary>
    public class LoginConsentDto
    {
        /// <summary>
        /// ACR represents the Authentication AuthorizationContext Class Reference value for this authentication session.
        /// </summary>
        [JsonProperty("acr")]
        public string Acr { get; set; }

        [JsonProperty("amr")]
        public string[] Amr { get; set; }

        /// <summary>
        /// ID is the identifier ("authorization challenge") of the consent authorization request. It is used to identify the session.
        /// </summary>
        [JsonProperty("challenge")]
        public string Challenge { get; set; }

        [JsonProperty("client")]
        public ClientDto Client { get; set; }

        [JsonProperty("context")]
        public JToken Context { get; set; }

        /// <summary>
        /// LoginChallenge is the login challenge this consent challenge belongs to.
        /// </summary>
        [JsonProperty("login_challenge")]
        public string LoginChallenge { get; set; }

        /// <summary>
        /// LoginSessionID is the login session ID. If the user-agent reuses a login session (via cookie / remember flag) this ID will remain the same.
        /// </summary>
        [JsonProperty("login_session_id")]
        public string LoginSessionId { get; set; }

        [JsonProperty("oidc_context")]
        public OidcContext OidcContext { get; set; }

        /// <summary>
        /// RequestURL is the original OAuth 2.0 Authorization URL requested by the OAuth 2.0 client.
        /// </summary>
        [JsonProperty("request_url")]
        public string RequestUrl { get; set; }

        [JsonProperty("requested_access_token_audience")]
        public string[] RequestedAccessTokenAudience { get; set; }

        [JsonProperty("requested_scope")]
        public string[] RequestedScope { get; set; }

        /// <summary>
        /// Skip, if true, implies that the client has requested the same scopes from the same user previously
        /// </summary>
        [JsonProperty("skip")]
        public bool Skip { get; set; }

        /// <summary>
        /// Subject is the user ID of the end-user that authenticated. Now, that end user needs to grant or deny the scope requested by the OAuth 2.0 client.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }
    }
}