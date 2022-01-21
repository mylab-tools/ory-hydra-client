using System.Net;
using System.Threading.Tasks;
using MyLab.ApiClient;

namespace MyLab.OryHydraClient
{
    [Api(Key = "OryHydraAdmin")]
    public interface IOryHydraAdminV110
    {
        /// <summary>
        /// This endpoint tells ORY Hydra that the subject has successfully authenticated and includes additional information such as the subject's ID and if ORY Hydra should remember the subject's subject agent for future authentication attempts by setting a cookie.
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api#operation/acceptLoginRequest</remarks>
        [Put("oauth2/auth/requests/login/accept")]
        Task<RedirectResponse> AcceptLoginRequestAsync([JsonContent] AcceptLoginReqRequest request, [Query("login_challenge")] string loginChallenge);

        /// <summary>
        /// This endpoint tells ORY Hydra that the subject has authorized the OAuth 2.0 client to access resources on his/her behalf. The consent provider includes additional information, such as session data for access and ID tokens, and if the consent request should be used as basis for future requests.
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api#operation/acceptConsentRequest</remarks>
        [Put("oauth2/auth/requests/consent/accept")]
        Task<RedirectResponse> AcceptConsentRequestAsync([JsonContent] AcceptConsentReqRequest request, [Query("consent_challenge")] string consentChallenge);

        /// <summary>
        /// Create a new OAuth 2.0 client If you pass client_secret the secret will be used, otherwise a random secret will be generated.
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api#operation/createOAuth2Client</remarks>
        [Post("clients")]
        [ExpectedCode(HttpStatusCode.Created)]
        Task<ClientDto> CreateClientAsync([JsonContent]ClientDto client);

        /// <summary>
        /// Get an OAUth 2.0 client by its ID. This endpoint never returns passwords.
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api/#operation/getOAuth2Client</remarks>
        [Get("clients/{id}")]
        Task<ClientDto> GetClientAsync([Path]string id);


        /// <summary>
        /// Delete an existing OAuth 2.0 Client by its ID.
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api/#operation/deleteOAuth2Client</remarks>
        [Delete("clients/{id}")]
        [ExpectedCode(HttpStatusCode.NoContent)]
        Task DeleteClientAsync([Path] string id);
    }
}