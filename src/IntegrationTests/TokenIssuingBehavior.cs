using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using MyLab.OryHydraClient;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class TokenIssuingBehavior : IAsyncLifetime
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

        [Fact]
        public async Task ShouldStartAuthorization()
        {
            //Arrange

            //Act
            var resp = await Authenticate();

            //Assert
            Assert.StartsWith(TestTools.LoginEndpoint + "?login_challenge=",resp.TargetLocation);
        }

        [Fact]
        public async Task ShouldAcceptLoginRequest()
        {
            //Arrange
            var authResp = await Authenticate();
            var loginChallenge = authResp.LoginChallenge;

            var acceptReq = new AcceptLoginReqRequest
            {
                Subject = "foo"
            };

            //Act
            var resp = await _admin.AcceptLoginRequestAsync(acceptReq, loginChallenge);
            var redirectUrl = await AfterLoginRequest(resp.ResponseContent.RedirectTo, authResp.Cookies);

            _output.WriteLine("URL: " + redirectUrl);

            //Assert
            Assert.StartsWith(TestTools.ConsentEndpoint + "?consent_challenge=", redirectUrl);
        }

        public async Task InitializeAsync()
        {
            _client = await TestTools.CreateClientAsync(_admin);
        }

        public async Task DisposeAsync()
        {
            await _client.DisposeAsync();
        }

        async Task<(string TargetLocation, string LoginChallenge, string Cookies)> Authenticate()
        {
            var resp = await _pub.Authenticate(TestTools.ScopesFooBar, _client.ClientId, TestTools.AvailableRedirectUri, "12345678");

            var targetLocation = resp?.ResponseMessage?.Headers?.Location?.ToString();

            if (targetLocation == null)
                throw new InvalidOperationException("Target location is empty");

            int loginChallengeDelimiter = targetLocation.IndexOf("=", StringComparison.InvariantCulture);
            if(loginChallengeDelimiter < 0)
                throw new InvalidOperationException("Target location ash wrong content");

            var loginChallenge = targetLocation.Substring(loginChallengeDelimiter + 1);

            var cookies = resp.ResponseMessage.Headers.GetValues("Set-Cookie").FirstOrDefault();

            return (targetLocation, loginChallenge, cookies);
        }

        async Task<string> AfterLoginRequest(string uri, string csrfCookies)
        {
            var httpHandler = new HttpClientHandler();

            httpHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

            var httpClient = new HttpClient(httpHandler)
            {
                DefaultRequestHeaders =
                {
                    {"cookie", csrfCookies}
                }
            };

            var msg = await httpClient.GetAsync(uri);

            if(msg.StatusCode != HttpStatusCode.Redirect)
                throw new InvalidOperationException("Wrong response status code: " + msg.StatusCode);

            return msg.Headers.Location?.ToString();
        }
    }
}
