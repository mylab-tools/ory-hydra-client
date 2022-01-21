using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyLab.OryHydraClient
{
    public class Session
    {
        /// <summary>
        /// AccessToken sets session data for the access 
        /// </summary>
        [JsonProperty("access_token")]
        public Dictionary<string, string> AccessToken { get; set; }

        /// <summary>
        /// IDToken sets session data for the OpenID Connect ID token.
        /// </summary>
        [JsonProperty("id_token")]
        public Dictionary<string, string> IdToken { get; set; }
    }
}