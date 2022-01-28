using System;
using Newtonsoft.Json;

namespace MyLab.OryHydraClient
{
    /// <summary>
    /// Contains consent session details
    /// </summary>
    public class ConsentSessionDto
    {
        [JsonProperty("consent_request")]
        public ConsentRequestDto ConsentRequest { get; set; }

        [JsonProperty("grant_access_token_audience")]
        public string[] GrantAccessTokenAudience { get; set; }

        [JsonProperty("grant_scope")]
        public string[] GrantScope { get; set; }

        [JsonProperty("handled_at")]
        public DateTime? HandledAt { get; set; }

        /// <summary>
        /// Remember, if set to true, tells ORY Hydra to remember this consent authorization and reuse it if the same client asks the same user for the same, or a subset of, scope.
        /// </summary>
        [JsonProperty("remember")]
        public bool? Remember { get; set; }

        /// <summary>
        /// RememberFor sets how long the consent authorization should be remembered for in seconds. If set to 0, the authorization will be remembered indefinitely.
        /// </summary>
        [JsonProperty("remember_for")]
        public long? RememberFor { get; set; }

        [JsonProperty("session")]
        public Session Session { get; set; }
    }
}