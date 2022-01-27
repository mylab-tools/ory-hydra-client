using System;
using System.Text;
using System.Threading.Tasks;
using MyLab.OryHydraClient;

namespace IntegrationTests
{
    public class CreatedClient : IAsyncDisposable
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