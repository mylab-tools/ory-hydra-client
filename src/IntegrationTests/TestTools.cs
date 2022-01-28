using System;
using System.Linq;
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
        public const string LogoutEndpoint = "http://localhost/test/logout";
        public const string PostLogoutEndpoint = "http://localhost/test/post-logout";
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

        public static async Task<(string TargetLocation, string LoginChallenge, string AuthCsrfCookie)> StartAuthenticate(
            IOryHydraPublicV110 api, string clientId, string state = "foo-state")
        {
            var resp = await api.AuthenticateAsync(TestTools.ScopesFooBar, clientId, TestTools.AvailableRedirectUri,
                state);

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
}