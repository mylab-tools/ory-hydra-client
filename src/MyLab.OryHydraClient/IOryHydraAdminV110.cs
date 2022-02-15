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

        /// <summary>
        /// Provides login request details
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/next/reference/api/#operation/getLoginRequest</remarks>
        [Get("oauth2/auth/requests/login")]
        Task<LoginRequestDto> GetLoginRequestAsync([Query("login_challenge")] string loginChallenge);

        /// <summary>
        /// Provides consent request details
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api/#operation/getConsentRequest</remarks>
        [Get("oauth2/auth/requests/consent")]
        Task<ConsentRequestDto> GetConsentRequestAsync([Query("consent_challenge")] string consentChallenge);

        /// <summary>
        /// Provider logout request details
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api/#operation/getLogoutRequest</remarks>
        [Get("oauth2/auth/requests/logout")]
        Task<LogoutRequestDto> GetLogoutRequestAsync([Query("logout_challenge")] string logoutChallenge);

        /// <summary>
        /// When a user or an application requests ORY Hydra to log out a user, this endpoint is used to confirm that logout request. 
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api/#operation/acceptLogoutRequest</remarks>
        [Put("oauth2/auth/requests/logout/accept")]
        Task<RedirectResponse> AcceptLogoutRequestAsync([Query("logout_challenge")] string logoutChallenge);

        /// <summary>
        /// When a user or an application requests ORY Hydra to log out a user, this endpoint is used to deny that logout request. 
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api/#operation/rejectLogoutRequest</remarks>
        [Put("oauth2/auth/requests/logout/reject")]
        [ExpectedCode(HttpStatusCode.NoContent)]
        Task RejectLogoutRequestAsync([Query("logout_challenge")] string logoutChallenge);

        /// <summary>
        /// This endpoint lists all subject's granted consent sessions, including client and granted scope. If the subject is unknown or has not granted any consent sessions yet, the endpoint returns an empty JSON array with status code 200 OK.
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api/#operation/listSubjectConsentSessions</remarks>
        [Get("oauth2/auth/sessions/consent")]
        Task<ConsentSessionDto[]> GetSubjectSessionsAsync([Query]string subject);

        /// <summary>
        /// This endpoint tells ORY Hydra that the subject has not authenticated and includes a reason why the authentication was be denied.
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api/#operation/rejectLoginRequest</remarks>
        [Put("oauth2/auth/requests/login/reject")]
        Task<RedirectResponse> RejectLoginRequest([Query("login_challenge")] string loginChallenge, [JsonContent]ErrorRequest errorRequest);

        /// <summary>
        /// This endpoint tells ORY Hydra that the subject has not authorized the OAuth 2.0 client to access resources on his/her behalf. The consent provider must include a reason why the consent was not granted.
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api/#operation/rejectConsentRequest</remarks>
        [Put("oauth2/auth/requests/consent/reject")]
        Task<RedirectResponse> RejectConsentRequest([Query("consent_challenge")] string consentChallenge, [JsonContent] ErrorRequest errorRequest);

        /// <summary>
        /// This endpoint invalidates a subject's authentication session. After revoking the authentication session, the subject has to re-authenticate at ORY Hydra
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api/#operation/revokeAuthenticationSession</remarks>
        [ExpectedCode(HttpStatusCode.NoContent)]
        [Delete("oauth2/auth/sessions/login")]
        Task InvalidateAllLoginSessions([Query]string subject);
    }
}