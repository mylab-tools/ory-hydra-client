using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyLab.OryHydraClient
{
    public class AcceptLoginReqRequest
    {
        /// <summary>
        /// ACR sets the Authentication AuthorizationContext Class Reference value for this authentication session. You can use it to express that, for example, a user authenticated using two factor authentication.
        /// </summary>
        [JsonProperty("acr")]
        public string Acr { get; set; }

        [JsonProperty("context")]
        public JToken Context { get; set; }
        /// <summary>
        /// ForceSubjectIdentifier forces the "pairwise" user ID of the end-user that authenticated. The "pairwise" user ID refers to the Pairwise Identifier Algorithm of the OpenID Connect specification. It allows you to set an obfuscated subject ("user") identifier that is unique to the client.
        /// </summary>
        [JsonProperty("force_subject_identifier")]
        public string ForceSubjectId { get; set; }
        /// <summary>
        /// Remember, if set to true, tells ORY Hydra to remember this user by telling the user agent (browser) to store a cookie with authentication data. If the same user performs another OAuth 2.0 Authorization Request, he/she will not be asked to log in again.
        /// </summary>
        [JsonProperty("remember")]
        public bool? Remember { get; set; }
        /// <summary>
        /// RememberFor sets how long the authentication should be remembered for in seconds. If set to 0, the authorization will be remembered for the duration of the browser session (using a session cookie)
        /// </summary>
        [JsonProperty("remember_for")]
        public long? RememberFor { get; set; }
        /// <summary>
        /// REQUIRED. Subject is the user ID of the end-user that authenticated.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }
    }
}