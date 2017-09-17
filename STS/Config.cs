using System.Collections.Generic;
using IdentityServer4.Models;

namespace STS
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "API 1")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {                
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "api1" }
                },
                new Client
                {
                    ClientId = "angular_spa",
                    ClientName = "Angular 4 Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new List<string> {"openid", "profile", "api1"},
                    RedirectUris = new List<string> { "http://localhost:4200/auth.html", "http://localhost:4200/silent-renew.html", "http://localhost:4200/auth-callback", "http://localhost:4200/silent-renew"},
                    PostLogoutRedirectUris = new List<string> {"http://localhost:4200/"},
                    AllowedCorsOrigins = new List<string> {"http://localhost:4200"},
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AccessTokenLifetime = 120 // Timeout after 2 minutes.  3600 secs = 1 hr is the default when not specified
                }
            };
        }
    }
}
