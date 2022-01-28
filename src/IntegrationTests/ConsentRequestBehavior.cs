using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class ConsentRequestBehavior : ClientBasedTest
    {
        public ConsentRequestBehavior(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task ShouldProvideConsentRequestDetails()
        {
            //Arrange
            var startLoginResp = await StartAuthenticateAsync();
            var loginResp = await AcceptLoginAsync("foo", startLoginResp.LoginChallenge);
            var afterLoginResp =
                await VerifyLoginAcceptRequestAsync(loginResp.RedirectTo, startLoginResp.AuthCsrfCookie);

            //Act
            var consentDto = await AdminApi.GetConsentRequestAsync(afterLoginResp.ConsentChallenge);

            //Assert
            Assert.Equal("foo", consentDto.Subject);
            Assert.Equal(Client.ClientId, Client.ClientId);
        }
    }
}