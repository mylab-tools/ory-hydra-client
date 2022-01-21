using System.Net;
using System.Threading.Tasks;
using MyLab.ApiClient;

namespace MyLab.OryHydraClient
{
    [Api(Key = "OryHydraPublic")]
    public interface IOryHydraPublicV110
    {
        /// <summary>
        /// The client makes a request to the token endpoint by sending the following parameters using the "application/x-www-form-urlencoded" HTTP request entity-body.
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api#operation/oauth2Token</remarks>
        [Post("oauth2/token")]
        Task<TokenResponse> RequestTokenAsync([Header("Authorization")] string authorization, [FormContent] TokenRequest request);

        [Get("oauth2/auth")]
        Task<RedirectResponse> AfterLoginRequestAsync(
            [Query("client_id")]string clientId,
            [Query("login_verifier")]string loginVerifier,
            [Query("redirect_uri")]string redirectUri,
            [Query("response_type")]string responseType,
            [Query("scope")]string scope,
            [Query("state")]string state);

        /// <summary>
        /// Authentication endpoint
        /// </summary>
        /// <param name="scope">REQUIRED. OpenID Connect requests MUST contain the openid scope value. If the openid scope value is not present, the behavior is entirely unspecified. Other scope values MAY be present. Scope values used that are not understood by an implementation SHOULD be ignored. See Sections 5.4 and 11 for additional scope values defined by this specification.</param>
        /// <param name="clientId">REQUIRED. OAuth 2.0 Client Identifier valid at the Authorization Server.</param>
        /// <param name="redirectUri">REQUIRED. Redirection URI to which the response will be sent. This URI MUST exactly match one of the Redirection URI values for the Client pre-registered at the OpenID Provider, with the matching performed as described in Section 6.2.1 of [RFC3986] (Simple String Comparison). </param>
        /// <param name="state"> REQUIRED.Opaque value used to maintain state between the request and the callback.</param>
        /// <param name="responseType">REQUIRED. OAuth 2.0 Response Type value that determines the authorization processing flow to be used, including what parameters are returned from the endpoints used. When using the Authorization Code Flow, this value is code.</param>
        /// <remarks>https://openid.net/specs/openid-connect-core-1_0.html#AuthorizationEndpoint</remarks>
        [Get("oauth2/auth")]
        [ExpectedCode(HttpStatusCode.Redirect)]
        Task<CallDetails> Authenticate(
            [Query("scope")]string scope, 
            [Query("client_id")]string clientId, 
            [Query("redirect_uri")]string redirectUri, 
            [Query("state")]string state, 
            [Query("response_type")]string responseType = "code");
    }
}