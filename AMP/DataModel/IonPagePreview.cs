using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace Anfema.Ion.DataModel
{
    public class IonPagePreview
    {
        public string identifier { get; set; }
        public string collection_identifier { get; set; }
        public int version_number { get; set; }

        [JsonProperty( "last_changed" )]
        public DateTime lastChanged { get; set; }

        public int position { get; set; }
        public string layout { get; set; }
        public string locale { get; set; }
        public string parent { get; set; }
        public JObject meta { get; set; }


        /// <summary>
        /// Used to get a string property of the meta for this pagepreview
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>String of the desired property</returns>
        public string getMetaString( string propertyName )
        {
            // Try to get the desired object from the meta json
            JToken result;
            meta.TryGetValue( propertyName, out result );

            if( result != null )
            {
                return result.ToString();
            }
            else
            {
                throw new JsonSerializationException( "The property " + propertyName + " could not be found in meta of pagePreview " + identifier + "." );
            }
        }


        /// <summary>
        /// Gets a list property if strings
        /// </summary>
        /// <param name="listName"></param>
        /// <returns>List of strings</returns>
        public List<string> getMetaList( string listName )
        {
            // Search list in properties
            JToken jObject = meta[listName];

            // If object wasn't found in meta
            if( jObject == null )
            {
                throw new JsonSerializationException( "The list property " + listName + " could not be found in meta of pagePreview " + identifier + "." );
            }

            // Try to parse the object to a list of strings
            try
            {
                string listString = jObject.ToString();
                List<string> result = JsonConvert.DeserializeObject<List<string>>( listString );
                return result;
            }
            catch
            {
                throw new JsonSerializationException( "The list property " + listName + " in pagePreview " + identifier + " could not be serialized." ); ;
            }
        }


        /// <summary>
        /// Used to get a metaString or null, if the metaString with the given key wasn't found
        /// </summary>
        /// <param name="metaKey"></param>
        /// <returns>MetaString or null</returns>
        public string getMetaStringOrNull( string metaKey )
        {
            try
            {
                return getMetaString( metaKey );
            }
            catch( Exception e )
            {
                return null;
            }
        }


        /// <summary>
        /// Used to get a metaString or an empty string, if the metaString with the given key wasn't found
        /// </summary>
        /// <param name="metaKey"></param>
        /// <returns>MetaString or empty string</returns>
        public string getMetaStringOrEmpty( string metaKey )
        {
            try
            {
                return getMetaString( metaKey );
            }
            catch( Exception e )
            {
                return "";
            }
        }
    }
}