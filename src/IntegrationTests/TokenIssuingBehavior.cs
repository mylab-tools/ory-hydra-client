using System.Collections.Generic;
using System.Threading.Tasks;
using MyLab.OryHydraClient;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class TokenIssuingBehavior : ClientBasedTest
    {
        public TokenIssuingBehavior(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task ShouldStartAuthorization()
        {
            //Arrange

            //Act
            var resp = await StartAuthenticateAsync();

            //Assert
            Assert.StartsWith(TestTools.LoginEndpoint + "?login_challenge=",resp.TargetLocation);
        }

        [Fact]
        public async Task ShouldAcceptLoginRequest()
        {
            //Arrange
            var authResp = await StartAuthenticateAsync();
            var loginChallenge = authResp.LoginChallenge;

            
            //Act
            var resp = await AcceptLoginAsync("foo", loginChallenge);
            var redirectUrl = await VerifyLoginAcceptRequestAsync(resp.RedirectTo, authResp.AuthCsrfCookie);

            Output.WriteLine("URL: " + redirectUrl);

            //Assert
            Assert.StartsWith(TestTools.ConsentEndpoint + "?consent_challenge=", redirectUrl.TargetLocation);
        }

        [Fact]
        public async Task ShouldConsentLoginRequest()
        {
            //Arrange
            var authResp = await StartAuthenticateAsync();
            var accLoginResp = await AcceptLoginAsync("foo", authResp.LoginChallenge);

            var afterLoginAccResp = await VerifyLoginAcceptRequestAsync(accLoginResp.RedirectTo, authResp.AuthCsrfCookie);

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

            var callbackUrl = await VerifyConsentAcceptRequestAsync(acceptConsentResponse.RedirectTo, afterLoginAccResp.ConsentCsrfCookie);

            var sessions = await AdminApi.GetSubjectSessionsAsync("foo");

            Output.WriteLine("URL: " + callbackUrl);

            //Assert
            Assert.StartsWith(TestTools.AvailableRedirectUri, callbackUrl);
            Assert.DoesNotContain("error", callbackUrl);
            Assert.Contains(sessions, s => s.ConsentRequest.Challenge == afterLoginAccResp.ConsentChallenge);
        }
    }
}
