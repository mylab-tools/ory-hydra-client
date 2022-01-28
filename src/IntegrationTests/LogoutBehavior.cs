using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using MyLab.OryHydraClient;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class LogoutBehavior : ClientBasedTest
    {
        public LogoutBehavior(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ShouldProvideLogoutRequestDetails()
        {
            //Arrange
            var loginResp = await LoginAsync();

            var logoutChallenge = await LogoutAsync(loginResp.AuthSessCookie);

            LogoutRequestDto logoutRequest = null;

            //Act

            if (logoutChallenge != null)
            {
                logoutRequest = await AdminApi.GetLogoutRequestAsync(logoutChallenge);
            }

            //Assert
            Assert.NotNull(logoutRequest);
            Assert.Null(logoutRequest.Client); //Why?
            Assert.Equal("foo", logoutRequest.Subject); 
            Assert.Equal(logoutChallenge, logoutRequest.Challenge);
        }

        [Fact]
        public async Task ShouldLogoutSession()
        {
            //Arrange
            var loginResp = await LoginAsync(); 
            
            //var sessions1Probe = await AdminApi.GetSubjectSessionsAsync("foo");

            var logoutChallenge = await LogoutAsync(loginResp.AuthSessCookie);

            //Act
            var logoutResp = await AdminApi.AcceptLogoutRequestAsync(logoutChallenge);

            var logoutRedirectUrl = await VerifyLogoutAsync(logoutResp.RedirectTo);

            Output.WriteLine("Post logout redirect: " + logoutRedirectUrl);
            
            //var sessions2Probe = await AdminApi.GetSubjectSessionsAsync("foo");

            //Output.WriteLine("Probe1:");
            //Output.WriteLine(JsonConvert.SerializeObject(sessions1Probe, Formatting.Indented));
            //Output.WriteLine("Probe2:");
            //Output.WriteLine(JsonConvert.SerializeObject(sessions2Probe, Formatting.Indented));

            //Assert
            Assert.StartsWith(TestTools.PostLogoutEndpoint, logoutRedirectUrl);
            Assert.DoesNotContain("error", logoutRedirectUrl);
            
            //How to check - no idea

            //Assert.Contains(sessions1Probe, s => s.ConsentRequest.Challenge == loginResp.ConsentChallenge);
            //Assert.DoesNotContain(sessions2Probe, s => s.ConsentRequest.Challenge == loginResp.ConsentChallenge);
        }

        async Task<string> VerifyLogoutAsync(string uri)
        {
            var httpHandler = new HttpClientHandler();

            httpHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

            var httpClient = new HttpClient(httpHandler)
            {
                //DefaultRequestHeaders =
                //{
                //    { "cookie", consentCsrfCookie }
                //}
            };

            var msg = await httpClient.GetAsync(uri);

            if (msg.StatusCode != HttpStatusCode.Redirect)
                throw new InvalidOperationException("Wrong response status code: " + msg.StatusCode);

            var locationStr = msg.Headers.Location?.ToString();

            if (locationStr == null)
                throw new InvalidOperationException("Target location is empty");

            return locationStr;
        }

        async Task<(string AuthSessCookie, string ConsentChallenge)> LoginAsync()
        {
            var startAuthResp = await StartAuthenticateAsync();
            
            var accLoginResp = await AcceptLoginAsync("foo", startAuthResp.LoginChallenge, true);
            var afterLoginAccResp = await VerifyLoginAcceptRequestAsync(accLoginResp.RedirectTo, startAuthResp.AuthCsrfCookie);

            var consentResp = await AdminApi.AcceptConsentRequestAsync(new AcceptConsentReqRequest
            {

            }, afterLoginAccResp.ConsentChallenge);

            await VerifyConsentAcceptRequestAsync(consentResp.RedirectTo, afterLoginAccResp.ConsentCsrfCookie);

            return (afterLoginAccResp.AuthSessCookie, afterLoginAccResp.ConsentChallenge);
        }

        async Task<string> LogoutAsync(string authSessCookie)
        {
            var logoutResp = await PubApi.LogoutAsync(authSessCookie);
            var redirectLocation = logoutResp.ResponseMessage.Headers.Location;

            string logoutChallenge = null;

            if (redirectLocation != null)
            {
                if (!redirectLocation.OriginalString.StartsWith(TestTools.LogoutEndpoint + "?"))
                    throw new InvalidOperationException("Wrong redirect target URL");

                if (!redirectLocation.Query.StartsWith("?logout_challenge="))
                    throw new InvalidOperationException("Wrong redirect target URL format");

                logoutChallenge = HttpUtility.ParseQueryString(redirectLocation.Query)["logout_challenge"];
            }

            return logoutChallenge;
        }
    }
}
