using System;
using System.Net;
using System.Threading.Tasks;
using MyLab.ApiClient;
using MyLab.OryHydraClient;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class ClientsBehavior
    {
        private readonly IOryHydraAdminV110 _admin;

        public ClientsBehavior(ITestOutputHelper output)
        {
            _admin = TestTools.CreateAdminApiClient(output);
        }

        [Fact]
        public async Task ShouldCreateClient()
        {
            //Arrange
            var newClient = new ClientDto
            {
                ClientId = Guid.NewGuid().ToString("N"),
                ClientName = "bar",
                ClientSecret = "secret",
                Scope = "foo bar",
                Audience = new string[]{ "foo", "bar" },
                GrantTypes = new []{"authorization_code"},
                Metadata = JToken.FromObject("metadata")
            };

            //Act
            var createdClient = await _admin.CreateClientAsync(newClient);

            await _admin.DeleteClientAsync(newClient.ClientId);

            //Assert
            Assert.Equal(newClient.ClientId, createdClient.ClientId);
            Assert.Equal(newClient.ClientName, createdClient.ClientName);
            Assert.Equal(newClient.Scope, createdClient.Scope);
            Assert.Equal(newClient.Audience, createdClient.Audience);
            Assert.Equal(newClient.GrantTypes, createdClient.GrantTypes);
            Assert.Equal(newClient.Metadata, createdClient.Metadata);
        }

        [Fact]
        public async Task ShouldGetClient()
        {
            //Arrange
            var newClient = new ClientDto
            {
                ClientId = Guid.NewGuid().ToString("N"),
                ClientName = "bar",
                ClientSecret = "secret",
                Scope = "foo bar",
                Audience = new [] { "foo", "bar" },
                GrantTypes = new[] { "authorization_code" },
                Metadata = JToken.FromObject("metadata")
            };

            //Act
            await _admin.CreateClientAsync(newClient);
            var createdClient = await _admin.GetClientAsync(newClient.ClientId);

            await _admin.DeleteClientAsync(newClient.ClientId);

            //Assert
            Assert.Equal(newClient.ClientId, createdClient.ClientId);
            Assert.Equal(newClient.ClientName, createdClient.ClientName);
            Assert.Equal(newClient.Scope, createdClient.Scope);
            Assert.Equal(newClient.Audience, createdClient.Audience);
            Assert.Equal(newClient.GrantTypes, createdClient.GrantTypes);
            Assert.Equal(newClient.Metadata, createdClient.Metadata);
        }

        [Fact]
        public async Task ShouldDeleteClient()
        {
            //Arrange
            var newClient = new ClientDto
            {
                ClientId = Guid.NewGuid().ToString("N")
            };

            //Act
            await _admin.CreateClientAsync(newClient);
            await _admin.DeleteClientAsync(newClient.ClientId);

            ResponseCodeException getAbsentClientException = null;

            try
            {
                await _admin.GetClientAsync(newClient.ClientId);
            }
            catch (ResponseCodeException e)
            {
                getAbsentClientException = e;
            }

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, getAbsentClientException?.StatusCode);
        }
    }
}
