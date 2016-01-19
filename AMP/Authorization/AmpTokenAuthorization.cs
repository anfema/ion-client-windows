using Anfema.Amp.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace Anfema.Amp.Authorization
{
    public class AmpTokenAuthorization : AmpLoginAPI
    {
        // login to get access token
        public async Task<AuthenticationHeaderValue> LoginAsync(string username, string password, string loginAdress)
        {
            try
            {
                // Temporary httpClient for the login process
                HttpClient loginClient = new HttpClient();

                // Login data object
                Login loginData;

                // Generate POST request
                Dictionary<string, string> request = new Dictionary<string, string>();
                request.Add("username", username);
                request.Add("password", password);

                FormUrlEncodedContent requestContent = new FormUrlEncodedContent(request);

                // Post request to server
                HttpResponseMessage response = await loginClient.PostAsync(new Uri(loginAdress), requestContent);

                // Process and return the acces token
                string jsonResult = response.Content.ReadAsStringAsync().Result;

                // Generate the model and try to parse the answer from the server
                LoginRootObject lro = new LoginRootObject();

                loginData = JsonConvert.DeserializeObject<LoginRootObject>(jsonResult).login;

                // TODO remove because of strange Amp behaviour
                loginData.api_url = "http://Ampdev2.anfema.com/client/v1/";

                // Create the http authentication header
                return new AuthenticationHeaderValue("Token", loginData.token);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
