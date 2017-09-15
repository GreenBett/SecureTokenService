using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        public static async Task MainAsync()
        {
            Console.WriteLine("Started...");
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");

            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            Console.WriteLine("Here is the Access Token the Client Console Application retrieved from calling the token endpoint on the STS:");
            Console.WriteLine(tokenResponse.AccessToken);

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetStringAsync("http://localhost:5002/api/protected");

            Console.WriteLine();
            Console.WriteLine("This is the view of the claims from the TestApi perspective upon receiving the Access Token on the GET endpoint in the ProtectedController:");
            Console.WriteLine(JToken.Parse(response));
        }
    }
}
