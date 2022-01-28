using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyLab.OryHydraClient
{
    /// <summary>
    /// Contains client properties
    /// </summary>
    public class ClientDto
    {
        [JsonProperty("allowed_cors_origins")]
        public string[] AllowedCorsOrigins { get; set; }

        [JsonProperty("audience")]
        public string[] Audience { get; set; }

        /// <summary>
        /// Boolean value specifying whether the RP requires that a sid (session ID) Claim be included in the Logout Token to identify the RP session with the OP when the backchannel_logout_uri is used. If omitted, the default value is false.
        /// </summary>
        [JsonProperty("backchannel_logout_session_required")]
        public bool? BackchannelLogoutSessionRequired { get; set; }

        /// <summary>
        /// RP URL that will cause the RP to log itself out when sent a Logout Token by the OP.
        /// </summary>
        [JsonProperty("backchannel_logout_uri")]
        public string BackchannelLogoutUri { get; set; }

        /// <summary>
        /// ID is the id for this client.
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Name is the human-readable string name of the client to be presented to the end-user during authorization.
        /// </summary>
        [JsonProperty("client_name")]
        public string ClientName { get; set; }

        /// <summary>
        /// Secret is the client's secret. The secret will be included in the create request as cleartext, and then never again. The secret is stored using BCrypt so it is impossible to recover it. Tell your users that they need to write the secret down as it will not be made available again.
        /// </summary>
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// SecretExpiresAt is an integer holding the time at which the client secret will expire or 0 if it will not expire. The time is represented as the number of seconds from 1970-01-01T00:00:00Z as measured in UTC until the date/time of expiration.
        /// </summary>
        [JsonProperty("client_secret_expires_at")]
        public long? ClientSecretExpiresAt { get; set; }

        /// <summary>
        /// ClientURI is an URL string of a web page providing information about the client. If present, the server SHOULD display this URL to the end-user in a clickable fashion.
        /// </summary>
        [JsonProperty("client_uri")]
        public string ClientUri { get; set; }
        
        [JsonProperty("contacts")]
        public string[] Contacts { get; set; }

        /// <summary>
        /// CreatedAt returns the timestamp of the client's creation.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Boolean value specifying whether the RP requires that iss (issuer) and sid (session ID) query parameters be included to identify the RP session with the OP when the frontchannel_logout_uri is used. If omitted, the default value is false.
        /// </summary>
        [JsonProperty("frontchannel_logout_session_required")]
        public string FrontchannelLogoutSessionRequired { get; set; }

        /// <summary>
        /// RP URL that will cause the RP to log itself out when rendered in an iframe by the OP. An iss (issuer) query parameter and a sid (session ID) query parameter MAY be included by the OP to enable the RP to validate the request and to determine which of the potentially multiple sessions is to be logged out; if either is included, both MUST be.
        /// </summary>
        [JsonProperty("frontchannel_logout_uri")]
        public string FrontchannelLogoutUri { get; set; }

        [JsonProperty("grant_types")]
        public string[] GrantTypes { get; set; }

        [JsonProperty("jwks")]
        public JToken Jwks { get; set; }

        /// <summary>
        /// URL for the Client's JSON Web Key Set [JWK] document. If the Client signs requests to the Server, it contains the signing key(s) the Server uses to validate signatures from the Client.
        /// </summary>
        [JsonProperty("jwks_uri")]
        public string JwksUri { get; set; }

        /// <summary>
        /// LogoURI is an URL string that references a logo for the client.
        /// </summary>
        [JsonProperty("logo_uri")]
        public string LogoUri { get; set; }

        [JsonProperty("metadata")]
        public JToken Metadata { get; set; }

        /// <summary>
        /// Owner is a string identifying the owner of the OAuth 2.0 Client
        /// </summary>
        [JsonProperty("owner")]
        public string Owner { get; set; }

        /// <summary>
        /// PolicyURI is a URL string that points to a human-readable privacy policy document that describes how the deployment organization collects, uses, retains, and discloses personal data.
        /// </summary>
        [JsonProperty("policy_uri")]
        public string PolicyUri { get; set; }

        [JsonProperty("post_logout_redirect_uris")]
        public string[] PostLogoutRedirectUris { get; set; }

        [JsonProperty("redirect_uris")]
        public string[] RedirectUris { get; set; }

        /// <summary>
        /// JWS [JWS] alg algorithm [JWA] that MUST be used for signing Request Objects sent to the OP. All Request Objects from this Client MUST be rejected, if not signed with this algorithm.
        /// </summary>
        [JsonProperty("request_object_signing_alg")]
        public string RequestObjectSigningAlg { get; set; }

        [JsonProperty("request_uris")]
        public string[] RequestUris { get; set; }

        [JsonProperty("response_types")]
        public string[] ResponseTypes { get; set; }

        /// <summary>
        /// Scope is a string containing a space-separated list of scope values (as described in Section 3.3 of OAuth 2.0 [RFC6749]) that the client can use when requesting access tokens.
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// URL using the https scheme to be used in calculating Pseudonymous Identifiers by the OP. The URL references a file with a single JSON array of redirect_uri values.
        /// </summary>
        [JsonProperty("sector_identifier_uri")]
        public string SectorIdentifierUri { get; set; }

        /// <summary>
        /// SubjectType requested for responses to this Client. The subject_types_supported Discovery parameter contains a list of the supported subject_type values for this server. Valid types include pairwise and public.
        /// </summary>
        [JsonProperty("subject_type")]
        public string SubjectType { get; set; }

        /// <summary>
        /// Requested Client Authentication method for the Token Endpoint. The options are client_secret_post, client_secret_basic, private_key_jwt, and none.
        /// </summary>
        [JsonProperty("token_endpoint_auth_method")]
        public string TokenEndpointAuthMethod { get; set; }

        /// <summary>
        /// Requested Client Authentication signing algorithm for the Token Endpoint.
        /// </summary>
        [JsonProperty("token_endpoint_auth_signing_alg")]
        public string TokenEndpointAuthSigningAlg { get; set; }

        /// <summary>
        /// TermsOfServiceURI is a URL string that points to a human-readable terms of service document for the client that describes a contractual relationship between the end-user and the client that the end-user accepts when authorizing the client.
        /// </summary>
        [JsonProperty("tos_uri")]
        public string TosUri { get; set; }

        /// <summary>
        /// UpdatedAt returns the timestamp of the last update.
        /// </summary>
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// JWS alg algorithm [JWA] REQUIRED for signing UserInfo Responses. If this is specified, the response will be JWT [JWT] serialized, and signed using JWS. The default, if omitted, is for the UserInfo Response to return the Claims as a UTF-8 encoded JSON object using the application/json content-type.
        /// </summary>
        [JsonProperty("userinfo_signed_response_alg")]
        public string UserinfoSignedResponseAlg { get; set; }


    }
}