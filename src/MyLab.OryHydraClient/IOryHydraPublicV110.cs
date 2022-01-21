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
        Task<CallDetails<TokenResponse>> Token([Header("Authorization")] string authorization, [FormContent] TokenRequest request);
    }
}