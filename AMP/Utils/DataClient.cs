using Anfema.Ion.DataModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;


namespace Anfema.Ion.Utils
{
    public class DataClient
    {
        // Client for all REST communication
        private HttpClient _client;

        // Holds the neccessary data for data retrieval
        private IonConfig _config;        
     

        /// <summary>
        /// Constructor with config file for initialization
        /// </summary>
        /// <param name="config"></param>
        public DataClient( IonConfig config )
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
        /// Get a collection for a given identifier
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <returns>IonCollection with the desired identifier</returns>
        public async Task<HttpResponseMessage> getCollectionAsync( string collectionIdentifier )
        {
            return await getCollectionAsync(collectionIdentifier, DateTime.MinValue).ConfigureAwait(false);
        }


        /// <summary>
        /// Used to get a page with a given identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>Already parsed IonPage</returns>
        public async Task<HttpResponseMessage> getPageAsync( string identifier )
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_config.baseUrl + _config.locale + "/" + _config.collectionIdentifier + "/" + identifier).ConfigureAwait(false);
                return response;
            }
            catch( Exception e )
            {
                Debug.WriteLine("Error getting page response from server! " + e.Message );
                return null;
            }
        }


        /// <summary>
        /// Used to get a collection that is newer than the given date. Otherwise the content is empty
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="lastModified"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> getCollectionAsync( string identifier, DateTime lastModified )
        {
            try
            {
                // Construct request string
                string requestString = _config.baseUrl + _config.locale + "/" + _config.collectionIdentifier;

                // Add the last-modified-header
                _client.DefaultRequestHeaders.Add("If-Modified-Since", lastModified.ToString("r") );

                // Recieve the response
                HttpResponseMessage response = await _client.GetAsync(requestString).ConfigureAwait(false);

                // Remove the last-modified-header
                _client.DefaultRequestHeaders.Remove("If-Modified-Since");

                return response;
            }

            catch( Exception e )
            {
                Debug.WriteLine("Error getting collection from server: " + e.Message);
                return null;
            }                
        }


        public async Task<MemoryStream> PerformRequest( Uri uri )
        {
            Stream stream = await _client.GetStreamAsync( uri ).ConfigureAwait(false);
            var memStream = new MemoryStream();
            await stream.CopyToAsync( memStream ).ConfigureAwait(false);
            memStream.Position = 0;
            return memStream;
        }
    }
}
