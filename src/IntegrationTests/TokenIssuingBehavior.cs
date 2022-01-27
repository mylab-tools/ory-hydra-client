using System.Collections.Generic;
using System.Threading.Tasks;
using MyLab.OryHydraClient;
using Xunit;

namespace IntegrationTests
{
    public partial class TokenIssuingBehavior
    {
        [Fact]
        public async Task ShouldStartAuthorization()
        {
            //Arrange

            //Act
            var resp = await StartAuthenticate();

            //Assert
            Assert.StartsWith(TestTools.LoginEndpoint + "?login_challenge=",resp.TargetLocation);
        }

        [Fact]
        public async Task ShouldAcceptLoginRequest()
        {
            //Arrange
            var authResp = await StartAuthenticate();
            var loginChallenge = authResp.LoginChallenge;

            var acceptReq = new AcceptLoginReqRequest
            {
                Subject = "foo"
            };

            //Act
            var resp = await AdminApi.AcceptLoginRequestAsync(acceptReq, loginChallenge);
            var redirectUrl = await AfterLoginAcceptRequest(resp.ResponseContent.RedirectTo, authResp.AuthCsrfCookie);

            Output.WriteLine("URL: " + redirectUrl);

            //Assert
            Assert.StartsWith(TestTools.ConsentEndpoint + "?consent_challenge=", redirectUrl.TargetLocation);
        }

        [Fact]
        public async Task ShouldConsentLoginRequest()
        {
            //Arrange
            var authResp = await StartAuthenticate();
            var accLoginResp = await AdminApi.AcceptLoginRequestAsync(
                new AcceptLoginReqRequest
                {
                    Subject = "foo"
                }, 
                authResp.LoginChallenge);

            var afterLoginAccResp = await AfterLoginAcceptRequest(accLoginResp.ResponseContent.RedirectTo, authResp.AuthCsrfCookie);

            var consentRequest = new AcceptConsentReqRequest
            {
                Session = new Session
                {
                    AccessToken = new Dictionary<string, object>
                    {
                        {"name", "Bob"}
                    },
                    IdToken = new Dictionary<string, object>
                    {
                        {"role", "tester"}
                    }
                },
                GrantScope = new []
                {
                    TestTools.ScopeBar
                }
            };

            //Act
            var acceptConsentResponse = await AdminApi.AcceptConsentRequestAsync(consentRequest, afterLoginAccResp.ConsentChallenge);

            var callbackUrl = await AfterConsentAcceptRequest(acceptConsentResponse.RedirectTo, afterLoginAccResp.ConsentCsrfCookie);

            Output.WriteLine("URL: " + callbackUrl);

            //Assert
            Assert.StartsWith(TestTools.AvailableRedirectUri, callbackUrl);
            Assert.DoesNotContain("error", callbackUrl);
        }
    }
}
