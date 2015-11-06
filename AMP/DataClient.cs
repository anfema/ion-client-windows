using Anfema.Amp.DataModel;
using Anfema.Amp.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp
{
    public class DataClient
    {
        private string loginAdress = "http://Ampdev2.anfema.com/client/v1/login";
        
        // Client for all REST communication
        private HttpClient _client;

        // Holds the neccessary data for data retrieval
        private Login _loginData;
        
        /// <summary>
        /// The possible api calls
        /// </summary>
        public Dictionary<string, string> ApiCalls { get; private set; }
        
        // login to get access token
        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                // Temporary httpClient for the login process
                HttpClient loginClient = new HttpClient();

                // Generate POST request
                Dictionary<string, string> request = new Dictionary<string, string>();
                request.Add("username", username);
                request.Add("password", password);

                FormUrlEncodedContent requestContent = new FormUrlEncodedContent(request);

                // Post request to server
                HttpResponseMessage response = await loginClient.PostAsync(new Uri(this.loginAdress), requestContent);

                // Process and return the acces token
                string jsonResult = response.Content.ReadAsStringAsync().Result;

                // Generate the model and try to parse the answer from the server
                LoginRootObject lro = new LoginRootObject();

                _loginData = JsonConvert.DeserializeObject<LoginRootObject>(jsonResult).login;

                // TODO remove because of strange Amp behaviour
                _loginData.api_url = "http://Ampdev2.anfema.com/client/v1/";

                // Create the httpClient with a special header
                _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", _loginData.token);

                // Init possible API calls
                this.ApiCalls = JsonConvert.DeserializeObject<Dictionary<string, string>>(await getData(""));

                return true;
            }
            catch(Exception e)
            {
                return false;
            }            
        }


        // Gets data for specific data content
        public async Task<string> getData(string requestString)
        {
            string response = "";

            try
            {
                response = await _client.GetStringAsync(_loginData.api_url + requestString);

                return response;
            }

            catch (HttpRequestException e)
            {
                Debug.WriteLine("getData exception: " + e.Message);
                return "";
            }
        }


        // Gets a complete page in raw json format
        public async Task<string> getPageOfCollection( string pageIdentifier, string collectionIdentifier )
        {
            string response = "";

            try
            {
                response = await _client.GetStringAsync(_loginData.api_url + "pages/" + collectionIdentifier + "/" + pageIdentifier );
            }

            catch (HttpRequestException e)
            {
                Debug.WriteLine("getData exception: " + e.Message);
            }

            return response;
        }
    }
}
