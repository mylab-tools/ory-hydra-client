using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class LoginRequestBehavior : ClientBasedTest
    {
        public LoginRequestBehavior(ITestOutputHelper output) 
            : base(output)
        {
        }

        [Fact]
        public async Task ShouldProvideLoginRequestDetails()
        {
            //Arrange
            var resp = await StartAuthenticateAsync();

            //Act
            var loginDto = await AdminApi.GetLoginRequest(resp.LoginChallenge);

            //Assert
            Assert.Empty(loginDto.Subject);
            Assert.Equal(Client.ClientId, loginDto.Client.ClientId);
        }
    }
}
