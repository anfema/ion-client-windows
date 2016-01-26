using Anfema.Amp.DataModel;
using Anfema.Amp.Parsing;
using Newtonsoft.Json;
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
     

        /// <summary>
        /// Constructor with config file for initialization
        /// </summary>
        /// <param name="config"></param>
        public DataClient( AmpConfig config )
        {
            try
            {
                _config = config;
                _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = _config.authenticationHeader;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in configuring the data client: " + e.Message);
            }
        }


        /// <summary>
        /// Gets data for specific data content
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns>String recieved from the server</returns>
        public async Task<string> getDataAsync(string requestString)
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


        /// <summary>
        /// Get a collection for a given identifier
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <returns>AmpCollection with the desired identifier</returns>
        public async Task<AmpCollection> getCollectionAsync( string collectionIdentifier )
        {
            string collectionsString = await getDataAsync("collections/" + collectionIdentifier);
            CollectionRoot collectionsRoot = JsonConvert.DeserializeObject<CollectionRoot>(collectionsString);
            return collectionsRoot.collection[0];
        }


        /// <summary>
        /// Used to get a page with a given identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>Already parsed AmpPage</returns>
        public async Task<AmpPage> getPageAsync( string identifier )
        {
            string pageString = await _client.GetStringAsync(_config.baseUrl + "pages/" + _config.collectionIdentifier + "/" + identifier + "?locale=" + _config.locale);
            PageRootRaw pageRootRaw = JsonConvert.DeserializeObject<PageRootRaw>(pageString);            

            return DataParser.parsePage(pageRootRaw.page[0]);
        }
    }
}
