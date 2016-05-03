using Anfema.Ion.Utils;
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
            catch
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
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Checks a given pagePreview for equality with this pagePreview
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if they both are equal, false otherwise</returns>
        public override bool Equals( object obj )
        {
            // If the object is the exact this object
            if( obj == this )
            {
                return true;
            }

            // If the given object is null then it can't be equal too
            if( obj == null )
            {
                return false;
            }

            // If the object's type is not equal to this type
            if( GetType() != obj.GetType() )
            {
                return false;
            }

            try
            {
                // Try to cast the object
                IonPagePreview content = (IonPagePreview)obj;

                // Special treatment for parent, which can be null
                bool parentEqual = false;
                if( parent == null )
                {
                    if( content.parent == null )
                    {
                        parentEqual = true;
                    }
                    else
                    {
                        return false;
                    }                        
                }
                else
                {
                    parentEqual = parent.Equals( content.parent );
                }

                // Check all elements for equality
                return identifier.Equals( content.identifier )
                    && collection_identifier.Equals( content.collection_identifier )
                    && version_number == content.version_number
                    && lastChanged == content.lastChanged
                    && layout.Equals( content.layout )
                    && locale.Equals( content.locale )
                    && parentEqual
                    && meta.ToString().Equals( content.meta.ToString() );
            }

            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Calculates the hashCode for this pagePreview based on his identifier
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return EqualsUtils.calcHashCode( identifier, collection_identifier, locale );
        }
    }
}