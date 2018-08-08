using Microsoft.Crm.Sdk.Samples.HelperCode;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Security;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace ConsoleApp1
{
    public class Class1
    {
        private HttpClient httpClient;
        private Configuration _config;
        private AuthenticationContext _context;
        //Start with base version and update with actual version later.
        private Version webAPIVersion = new Version(8, 0);

        public SecureString Password { get; private set; }

        private string getVersionedWebAPIPath()
        {
            return string.Format("v{0}/", webAPIVersion.ToString(2));
        }

        public async Task getWebAPIVersion()
        {

            HttpRequestMessage RetrieveVersionRequest =
              new HttpRequestMessage(HttpMethod.Get, getVersionedWebAPIPath() + "RetrieveVersion");

            HttpResponseMessage RetrieveVersionResponse =
                await httpClient.SendAsync(RetrieveVersionRequest);
            if (RetrieveVersionResponse.StatusCode == HttpStatusCode.OK)  //200
            {
                JObject RetrievedVersion = JsonConvert.DeserializeObject<JObject>(
                    await RetrieveVersionResponse.Content.ReadAsStringAsync());
                //Capture the actual version available in this organization
                webAPIVersion = Version.Parse((string)RetrievedVersion.GetValue("Version"));
                Console.WriteLine(webAPIVersion);
            }
            else
            {
                Console.WriteLine("Failed to retrieve the version for reason: {0}",
                    RetrieveVersionResponse.ReasonPhrase);
                throw new CrmHttpResponseException(RetrieveVersionResponse.Content);
            }

        }

        public void RunAll()
        {
            Class1 app = new Class1();
            app.ConnectToCRM();
            Task.WaitAll(Task.Run(async () => await app.getWebAPIVersion()));
            Task.WaitAll(Task.Run(async () => await app.retrieveProductsAsync()));
            Console.ReadKey();
        }

        private void ConnectToCRM()
        {
            //Create a helper object to read app.config for service URL and application 
            // registration settings.
            Configuration _config = null;
            _config = new FileConfiguration();

            _config.ClientId = "1c739198-aca7-45f9-95ee-58751062b9cd";
            _config.Username = "robin.goos@scapta.com";
            string pw = "Welcome@Scapta";
            Password = new SecureString();
            foreach (char c in pw) Password.AppendChar(c);
            _config.RedirectUrl = "http://localhost/HoloDynamics";
            _config.ServiceUrl = "https://scapta.crm4.dynamics.com/";
            
            AuthenticationContext authContext = new AuthenticationContext("https://login.windows.net/common", false);
            AuthenticationResult result = this.AcquireToken();
            AuthenticationResult resultCred = this.AcquireToken(_config.Username, _config.Password);

            //Console.WriteLine(config.ClientId, config.Domain, config.Password, config.RedirectUrl, config.ServiceUrl, config.Username);
            ////Create a helper object to authenticate the user with this connection info.
            //Authentication auth = new Authentication(config);
            //Next use a HttpClient object to connect to specified CRM Web service.
            //httpClient = new HttpClient(auth.ClientHandler, true);
            //Define the Web API base address, the max period of execute time, the
            // default OData version, and the default response payload format.
            //httpClient.BaseAddress = new Uri(config.ServiceUrl + "api/data/");
            //httpClient.Timeout = new TimeSpan(0, 2, 0);
            //httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            //httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            //httpClient.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task retrieveProductsAsync()
        {
            string queryOptions;
            HttpRequestMessage req;
            HttpResponseMessage resp;

            Console.WriteLine("--Query--");
            queryOptions = "?$select=scp_holoproduct";

            resp = await SendCrmRequestAsync(HttpMethod.Get, "https://scapta.crm4.dynamics.com/api/data/v9.0.2.758/scp_holoproducts?$select=scp_name", true);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                JObject product = JsonConvert.DeserializeObject<JObject>(resp.Content.ReadAsStringAsync().Result);
                Console.WriteLine(product.Count);
            }
        }

        private async Task<HttpResponseMessage> SendCrmRequestAsync(
            HttpMethod method, string query, Boolean formatted = false, int maxPageSize = 10)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, query);
            request.Headers.Add("Prefer", "odata.maxpagesize=" + maxPageSize.ToString());
            if (formatted)
                request.Headers.Add("Prefer",
                    "odata.include-annotations=OData.Community.Display.V1.FormattedValue");
            return await httpClient.SendAsync(request);
        }
    }
}
