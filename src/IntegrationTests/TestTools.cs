using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MyLab.ApiClient;
using MyLab.OryHydraClient;
using Xunit.Abstractions;

namespace IntegrationTests
{
    static class TestTools
    {
        public const string AvailableRedirectUri = "http://localhost/callback";

        public const string ScopeFoo = "fooscope";
        public const string ScopeBar = "barscope";
        public const string ScopesFooBar = ScopeFoo + " " + ScopeBar + " " + "openid";

        public const string LoginEndpoint = "http://localhost/test/login";
        public const string ConsentEndpoint = "http://localhost/test/consent";
        public const string HydraPublicPoint = "https://localhost:9000";

        public static IOryHydraPublicV110 CreatePublicApiClient(ITestOutputHelper output)
        {
            return CreateServiceProvider(output).GetService<IOryHydraPublicV110>();
        }

        public static IOryHydraAdminV110 CreateAdminApiClient(ITestOutputHelper output)
        {
            return CreateServiceProvider(output).GetService<IOryHydraAdminV110>();
        }

        public static async Task<CreatedClient> CreateClientAsync(IOryHydraAdminV110 adminApi)
        {
            var clientId = Guid.NewGuid().ToString("N");
            var clientSecret = Guid.NewGuid().ToString("N");

            var client = new ClientDto
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                RedirectUris = new []{ AvailableRedirectUri },
                Scope = ScopesFooBar,
                GrantTypes = new []{ "authorization_code" },
                ResponseTypes = new []{"code"}
            };

            await adminApi.CreateClientAsync(client);

            return new CreatedClient(clientId, clientSecret, adminApi);
        }

        static IServiceProvider CreateServiceProvider(ITestOutputHelper output)
        {
            return new ServiceCollection()
                .AddApiClients(reg =>
                    {
                        reg.RegisterContract<IOryHydraAdminV110>();
                        reg.RegisterContract<IOryHydraPublicV110>();
                    },
                    o =>
                    {
                        o.JsonSettings.IgnoreNullValues = true;
                        o.UrlFormSettings.EscapeSymbols = false;
                        o.List.Add("OryHydraAdmin",
                            new ApiConnectionOptions
                            {
                                Url = "https://localhost:9001",
                                SkipServerSslCertVerification = true
                            });
                        o.List.Add("OryHydraPublic",
                            new ApiConnectionOptions
                            {
                                Url = HydraPublicPoint,
                                SkipServerSslCertVerification = true
                            });
                    }
                )
                .AddLogging(l => l.AddFilter(_ => true).AddXUnit(output))
                .BuildServiceProvider();
        }
    }

    class CreatedClient : IAsyncDisposable
    {
        private readonly string _clientSecret;
        private readonly IOryHydraAdminV110 _admin;

        public string ClientId { get; }

        public CreatedClient(string clientId, string clientSecret, IOryHydraAdminV110 admin)
        {
            ClientId = clientId;
            _clientSecret = clientSecret;
            _admin = admin;
        }

        public async ValueTask DisposeAsync()
        {
            await _admin.DeleteClientAsync(ClientId);
        }

        public string GetAuthorizationHeader()
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(ClientId + ":" + _clientSecret));
        }
    }
}