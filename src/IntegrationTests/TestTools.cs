using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.ApiClient;
using MyLab.OryHydraClient;
using Xunit.Abstractions;

namespace IntegrationTests
{
    static class TestTools
    {
        public static IOryHydraPublicV110 CreatePublicApiClient(ITestOutputHelper output)
        {
            return CreateServiceProvider(output).GetService<IOryHydraPublicV110>();
        }

        public static IOryHydraAdminV110 CreateAdminApiClient(ITestOutputHelper output)
        {
            return CreateServiceProvider(output).GetService<IOryHydraAdminV110>();
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
                                Url = "https://localhost:9000",
                                SkipServerSslCertVerification = true
                            });
                    }
                )
                .AddLogging(l => l.AddFilter(_ => true).AddXUnit(output))
                .BuildServiceProvider();
        }
    }
}