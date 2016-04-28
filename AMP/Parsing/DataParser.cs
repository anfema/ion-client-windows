using Anfema.Ion.DataModel;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;


namespace Anfema.Ion.Parsing
{
    public static class DataParser
    {
        /// <summary>
        /// Parses a given raw json to a IonPage
        /// </summary>
        /// <param name="pageRaw"></param>
        /// <returns>IonPage without the generic data types</returns>
        public static async Task<IonPage> parsePageAsync(HttpResponseMessage response)
        {
            // Extract the json string from the content of the response message
            string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return parsePage( responseString );
        }


        /// <summary>
        /// Parses a given pageString to a IonPage
        /// </summary>
        /// <param name="pageString"></param>
        /// <returns>Parsed IonPage</returns>
        public static IonPage parsePage( string pageString )
        {
            // Parse the page to a raw page container
            IonPage pageParsed = new IonPage();

            try
            {
                // Deserialize page
                IonPageRoot pageParsedNew = JsonConvert.DeserializeObject<IonPageRoot>( pageString );

                // Set the first element of the root to be the page
                pageParsed = pageParsedNew.page[0];

                // Sort out empty content
                pageParsed.sortOutEmptyContent();
            }
            catch( Exception e )
            {
                Debug.WriteLine( "Error deserializing page json: " + e.Message );
            }

            return pageParsed;
        }


        /// <summary>
        /// Parses the content of a HttpResponseMessage as a collection and returns the first collection in the array
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<IonCollection> parseCollectionAsync( HttpResponseMessage response )
        {
            return JsonConvert.DeserializeObject<CollectionRoot>( await response.Content.ReadAsStringAsync().ConfigureAwait(false) ).collection[0];
        }


        /// <summary>
        /// Parses the content of a given string as a collection and returns the first collection in the array
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static IonCollection parseCollection( string collectionString )
        {
            return JsonConvert.DeserializeObject<CollectionRoot>( collectionString ).collection[0];
        }
    }
}
