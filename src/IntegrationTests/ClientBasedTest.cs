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

        protected Task<(string TargetLocation, string LoginChallenge, string AuthCsrfCookie)> StartAuthenticate()
        {
            return TestTools.StartAuthenticate(PubApi, Client.ClientId);
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