using Anfema.Amp.DataModel;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;


namespace Anfema.Amp.Parsing
{
    public static class DataParser
    {
        /// <summary>
        /// Parses a given raw json to a AmpPage
        /// </summary>
        /// <param name="pageRaw"></param>
        /// <returns>AmpPage without the generic data types</returns>
        public static async Task<AmpPage> parsePage(HttpResponseMessage response)
        {
            // Extract the json string from the content of the response message
            string responseString = await response.Content.ReadAsStringAsync();

            // Parse the page to a raw page container
            AmpPage pageParsed = new AmpPage();

            try
            {
                AMPPageRoot pageParsedNew = JsonConvert.DeserializeObject<AMPPageRoot>(responseString);
                pageParsed = pageParsedNew.page[0];
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error deserializing page json: " + e.Message);
            }

            return pageParsed;
        }


        /// <summary>
        /// Parses the content of a HttpResponseMessage as a collection and returns the first collection in the array
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<AmpCollection> parseCollection( HttpResponseMessage response )
        {
            return JsonConvert.DeserializeObject<CollectionRoot>( await response.Content.ReadAsStringAsync() ).collection[0];
        }
    }
}
