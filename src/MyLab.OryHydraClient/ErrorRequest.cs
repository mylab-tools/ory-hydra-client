using Newtonsoft.Json;

namespace MyLab.OryHydraClient
{
    /// <summary>
    /// Contains error
    /// </summary>
    public class ErrorRequest
    {
        /// <summary>
        /// The error should follow the OAuth2 error format (e.g. invalid_request, login_required).Defaults to request_denied.
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        /// <summary>
        /// Debug contains information to help resolve the problem as a developer. Usually not exposed to the public but only in the server logs.
        /// </summary>
        [JsonProperty("error_debug")]
        public string ErrorDebug { get; set; }

        /// <summary>
        /// Description of the error in a human readable format.
        /// </summary>
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Hint to help resolve the error.
        /// </summary>
        [JsonProperty("error_hint")]
        public string ErrorHint { get; set; }

        /// <summary>
        /// Represents the HTTP status code of the error (e.g. 401 or 403). Defaults to 400
        /// </summary>
        [JsonProperty("status_code")]
        public string StatusCode { get; set; }
    }
}