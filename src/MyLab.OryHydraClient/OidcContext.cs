using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyLab.OryHydraClient
{
    /// <summary>
    /// OpenID Connect context
    /// </summary>
    public class OidcContext
    {
        /// <summary>
        /// ACRValues is the Authentication AuthorizationContext Class Reference requested in the OAuth 2.0 Authorization request.
        /// </summary>
        [JsonProperty("acr_values")]
        public string[] AcrValues { get; set; }

        /// <summary>
        /// Display is a string value that specifies how the Authorization Server displays the authentication and consent user interface pages to the End-User
        /// </summary>
        [JsonProperty("display")]
        public string Display { get; set; }

        /// <summary>
        /// IDTokenHintClaims are the claims of the ID Token previously issued by the Authorization Server being passed as a hint about the End-User's current or past authenticated session with the Client.
        /// </summary>
        [JsonProperty("id_token_hint_claims")]
        public Dictionary<string,object> IdTokenHintClaims { get; set; }

        /// <summary>
        /// LoginHint hints about the login identifier the End-User might use to log in (if necessary)
        /// </summary>
        [JsonProperty("login_hint")]
        public string LoginHint { get; set; }

        /// <summary>
        /// UILocales is the End-User'id preferred languages and scripts for the user interface, represented as a space-separated list of BCP47 [RFC5646] language tag values, ordered by preference
        /// </summary>
        [JsonProperty("ui_locales")]
        public string[] UiLocales { get; set; }


    }
}