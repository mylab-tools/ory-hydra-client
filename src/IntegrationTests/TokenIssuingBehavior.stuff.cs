using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MyLab.OryHydraClient;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public partial class TokenIssuingBehavior
    {
        private readonly ITestOutputHelper _output;
        private readonly IOryHydraPublicV110 _pub;
        private readonly IOryHydraAdminV110 _admin;
        private CreatedClient _client;

        public TokenIssuingBehavior(ITestOutputHelper output)
        {
            _output = output;
            _pub = TestTools.CreatePublicApiClient(output);
            _admin = TestTools.CreateAdminApiClient(output);
        }

        public async Task InitializeAsync()
        {
            _client = await TestTools.CreateClientAsync(_admin);
        }

        public async Task DisposeAsync()
        {
            await _client.DisposeAsync();
        }

        private async Task<(string TargetLocation, string LoginChallenge, string AuthCsrfCookie)> Authenticate()
        {
            var resp = await _pub.Authenticate(TestTools.ScopesFooBar, _client.ClientId, TestTools.AvailableRedirectUri,
                "12345678");

            var targetLocation = resp?.ResponseMessage?.Headers?.Location?.ToString();

            if (targetLocation == null)
                throw new InvalidOperationException("Target location is empty");

            int loginChallengeDelimiter = targetLocation.IndexOf("=", StringComparison.InvariantCulture);
            if (loginChallengeDelimiter < 0)
                throw new InvalidOperationException("Target location ash wrong content");

            var loginChallenge = targetLocation.Substring(loginChallengeDelimiter + 1);

            var authCsrfCookie = resp.ResponseMessage.Headers
                .GetValues("Set-Cookie")
                .FirstOrDefault(c => c.StartsWith("oauth2_authentication_csrf="));

            return (targetLocation, loginChallenge, authCsrfCookie);
        }

        private async Task<(string TargetLocation, string ConsentChallenge, string AuthSessCookie, string ConsentCsrfCookie)> AfterLoginAcceptRequest(string uri, string authCsrfCookie)
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

        private async Task<string> AfterConsentAcceptRequest(string uri, string consentCsrfCookie)
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
            //int loginChallengeDelimiter = locationStr.IndexOf("=", StringComparison.InvariantCulture);
            //if (loginChallengeDelimiter < 0)
            //    throw new InvalidOperationException("Target location ash wrong content");

            //var consentChallenge = locationStr.Substring(loginChallengeDelimiter + 1);

            return locationStr;
        }
    }
}