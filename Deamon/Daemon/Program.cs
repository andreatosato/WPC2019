using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Daemon
{
    class Program
    {
        private const string ClientSecrets = "A-U==RY4mEgJZ0tRUY6[p4fxj6ShvmpU";
        public const string TenantId = "97877fab-d3d2-4424-bbb9-2423276e5f58";
        private const string ClientId = "f2699a0e-73f6-4ca0-adf5-f543038279c3";
        const string Resource = "https://login.microsoftonline.com/";
        static string Authority => $"{Resource}{TenantId}";        
        static string[] Scopes = new string[] { "https://graph.microsoft.com/.default" };
        static AuthenticationResult _identity;

        static async Task Main(string[] args)
        {
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(ClientId)
                                         .WithClientSecret(ClientSecrets)
                                         .WithAuthority(new Uri(Authority))
                                         .Build();

            
            try
            {
                _identity = await app
                    .AcquireTokenForClient(Scopes)
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                // Log
            }
            catch (MsalServiceException ex)
            {
                // Log
            }
            catch (MsalClientException ex)
            {
                // Log
            }


            var client = new GraphServiceClient(
                  new DelegateAuthenticationProvider(
                      async (requestMessage) =>
                      {
                          //TODO: token cache
                          requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", _identity.AccessToken);
                      }));

            var me = await client.Users["andrea.tosato@rnd4ward.it"].Request().GetAsync();
            var options = new JsonSerializerOptions
            {
                
                WriteIndented = true, 
            };

            Console.WriteLine(JsonSerializer.Serialize(
                new 
                {
                    DisplayName = me.DisplayName,
                    AboutMe = me.AboutMe,
                    Birthday = me.Birthday,
                    JobTitle = me.JobTitle,
                    Mail = me.Mail,
                    MailNickname = me.MailNickname,
                    PreferredName = me.PreferredName,
                    Photo = me.Photo
                }, options
            ));

            Console.ReadLine();




            var me2 = await client.Users["alberto.gardini@rnd4ward.it"].Request().GetAsync();
            Console.WriteLine(JsonSerializer.Serialize(
                new
                {
                    DisplayName = me2.DisplayName,
                    AboutMe = me2.AboutMe,
                    Birthday = me2.Birthday,
                    JobTitle = me2.JobTitle,
                    Mail = me2.Mail,
                    MailNickname = me2.MailNickname,
                    PreferredName = me2.PreferredName,
                    Photo = me2.Photo
                }, options
            ));
            Console.ReadLine();

        }
    }
}
