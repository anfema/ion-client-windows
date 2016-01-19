using Anfema.Amp.DataModel;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;


namespace Anfema.Amp
{
    public class DataClient
    {
        // Client for all REST communication
        private HttpClient _client;

        // Holds the neccessary data for data retrieval
        private AmpConfig _config;        
     

        // Configures the data client with the parameters included in the config file
        public bool configureDataClient( AmpConfig config )
        {
            try
            {
                _config = config;
                _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = _config.authenticationHeader;

                return true;
            }
            catch( Exception e)
            {
                Debug.WriteLine("Error in configuring the data client: " + e.Message);
                return false;
            }
        }


        // Gets data for specific data content
        public async Task<string> getData(string requestString)
        {
            string response = "";

            try
            {
                response = await _client.GetStringAsync(_config.baseUrl + requestString + "?locale=" + _config.locale);

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
                response = await _client.GetStringAsync(_config.baseUrl + "pages/" + collectionIdentifier + "/" + pageIdentifier + "?locale=" + _config.locale );
            }

            catch (HttpRequestException e)
            {
                Debug.WriteLine("getData exception: " + e.Message);
            }

            return response;
        }
    }
}
