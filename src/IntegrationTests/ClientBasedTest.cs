using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MyLab.OryHydraClient;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public abstract class ClientBasedTest : IAsyncLifetime
    {
        protected ITestOutputHelper Output { get; }
        protected IOryHydraPublicV110 PubApi { get; }
        protected IOryHydraAdminV110 AdminApi { get; }
        protected CreatedClient Client { get; private set; }

        protected ClientBasedTest(ITestOutputHelper output)
        {
            Output = output;
            PubApi = TestTools.CreatePublicApiClient(output);
            AdminApi = TestTools.CreateAdminApiClient(output);
        }

        protected Task<(string TargetLocation, string LoginChallenge, string AuthCsrfCookie)> StartAuthenticateAsync()
        {
            return TestTools.StartAuthenticate(PubApi, Client.ClientId);
        }

        protected Task<RedirectResponse> AcceptLoginAsync(string subject, string loginChallenge)
        {
            return AcceptLoginAsync(new AcceptLoginReqRequest
            {
                Subject = subject
            }, loginChallenge);
        }

        protected Task<RedirectResponse> AcceptLoginAsync(AcceptLoginReqRequest req, string loginChallenge)
        {
            return AdminApi.AcceptLoginRequestAsync(req, loginChallenge);
        }

        protected async Task<(string TargetLocation, string ConsentChallenge, string AuthSessCookie, string ConsentCsrfCookie)> AfterLoginAcceptRequestAsync(string uri, string authCsrfCookie)
        {
            var httpHandler = new HttpClientHandler();

            httpHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

            var httpClient = new HttpClient(httpHandler)
            {
                DefaultRequestHeaders =
                {
                    { "cookie", authCsrfCookie }
                }
            };

            var msg = await httpClient.GetAsync(uri);

            var cookieHeader = msg.Headers.FirstOrDefault(h => h.Key == "Set-Cookie");

            var authSessCookie = cookieHeader.Value.FirstOrDefault(v => v.StartsWith("oauth2_authentication_session="));
            var consentCsrfCookie = cookieHeader.Value.FirstOrDefault(v => v.StartsWith("oauth2_consent_csrf="));

            if (msg.StatusCode != HttpStatusCode.Redirect)
                throw new InvalidOperationException("Wrong response status code: " + msg.StatusCode);

            var locationStr = msg.Headers.Location?.ToString();

            if (locationStr == null)
                throw new InvalidOperationException("Target location is empty");
            int loginChallengeDelimiter = locationStr.IndexOf("=", StringComparison.InvariantCulture);
            if (loginChallengeDelimiter < 0)
                throw new InvalidOperationException("Target location ash wrong content");

            var consentChallenge = locationStr.Substring(loginChallengeDelimiter + 1);

            return (locationStr, consentChallenge, authSessCookie, consentCsrfCookie);
        }
        protected async Task<string> AfterConsentAcceptRequestAsync(string uri, string consentCsrfCookie)
        {
            var httpHandler = new HttpClientHandler();

            httpHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

            var httpClient = new HttpClient(httpHandler)
            {
                DefaultRequestHeaders =
                {
                    { "cookie", consentCsrfCookie }
                }
            };

            var msg = await httpClient.GetAsync(uri);

            if (msg.StatusCode != HttpStatusCode.Redirect)
                throw new InvalidOperationException("Wrong response status code: " + msg.StatusCode);

            var locationStr = msg.Headers.Location?.ToString();

            if (locationStr == null)
                throw new InvalidOperationException("Target location is empty");

            return locationStr;
        }

        public async Task InitializeAsync()
        {
            Client = await TestTools.CreateClientAsync(AdminApi);
        }

        public async Task DisposeAsync()
        {
            await Client.DisposeAsync();
        }
    }
}