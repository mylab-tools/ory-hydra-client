using System;
using MyLab.ApiClient;

namespace MyLab.OryHydraClient
{
    /// <summary>
    /// Contains OIDC token request parameters
    /// </summary>
    public class TokenRequest
    {
        [UrlFormItem(Name = "grant_type")]
        public string GrantType { get; set; }

        [UrlFormItem(Name = "code")]
        public string Code { get; set; }

        [UrlFormItem(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [UrlFormItem(Name = "redirect_uri")]
        public string RedirectUri { get; set; }

        [UrlFormItem(Name = "client_id")]
        public string ClientId { get; set; }

        [UrlFormItem(Name = "scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Create <see cref="TokenRequest"/> for Authorization Code Grant
        /// </summary>
        /// <remarks>https://openid.net/specs/openid-connect-core-1_0.html#TokenRequest</remarks>
        /// <param name="code">REQUIRED, The authorization code received from the authorization server</param>
        /// <param name="redirectUri">REQUIRED, if the "redirect_uri" parameter was included in the authorization request as described in Section 4.1.1, and their values MUST be identical</param>
        /// <param name="clientId">REQUIRED, if the client is not authenticating with the authorization server as described in Section 3.2.1.</param>
        public TokenRequest CreateAuthorizationCodeGrantRequest(string code, Uri redirectUri, string clientId)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));

            return new TokenRequest
            {
                GrantType = "authorization_code",
                ClientId = clientId,
                Code = code,
                RedirectUri = redirectUri.OriginalString
            };
        }

        /// <summary>
        /// Creates request fro token refreshing
        /// </summary>
        /// <param name="refreshToken">REQUIRED.  The refresh token issued to the client.</param>
        /// <param name="scope">OPTIONAL.  The scope of the access request as described by Section 3.3.</param>
        /// <remarks>https://openid.net/specs/openid-connect-core-1_0.html#RefreshingAccessToken</remarks>
        public TokenRequest CreateRefreshRequest(string refreshToken, string scope)
        {
            if (refreshToken == null) throw new ArgumentNullException(nameof(refreshToken));

            return new TokenRequest
            {
                GrantType = "refresh_token",
                RefreshToken = refreshToken,
                Scope = scope
            };
        }
    }
}