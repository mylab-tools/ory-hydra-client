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
        Task<CallDetails<RedirectResponse>> AcceptLoginRequest([JsonContent] AcceptLoginReqRequest request, [Query("login_challenge")] string loginChallenge);

        /// <summary>
        /// This endpoint tells ORY Hydra that the subject has authorized the OAuth 2.0 client to access resources on his/her behalf. The consent provider includes additional information, such as session data for access and ID tokens, and if the consent request should be used as basis for future requests.
        /// </summary>
        /// <remarks>https://www.ory.sh/hydra/docs/reference/api#operation/acceptConsentRequest</remarks>
        [Put("oauth2/auth/requests/consent/accept")]
        Task<CallDetails<RedirectResponse>> AcceptConsentRequest([JsonContent] AcceptConsentReqRequest request, [Query("consent_challenge")] string consentChallenge);
    }
}