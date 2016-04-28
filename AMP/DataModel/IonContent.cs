using Anfema.Ion.Parsing;
using Newtonsoft.Json;
using System;

namespace Anfema.Ion.DataModel
{
    /// <summary>
    /// Content base class that all content classes should inheritate from
    /// </summary> 
    [JsonConverter( typeof( IonContentConverter ) )]
    public class IonContent
    {
        public string variation { get; set; }
        public string outlet { get; set; }

        [JsonProperty( "is_searchable" )]
        public bool isSearchable { get; set; }

        public int position { get; set; }
        public string type { get; set; }

        [JsonProperty( "is_available" )]
        public bool isAvailable { get; set; } = true;


        /// <summary>
        /// Checks for equality of the given object with the current one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if both objects are equal and false otherwise</returns>
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

            try
            {
                // Try to cast the object
                IonContent content = (IonContent)obj;

                // Check all elements for equality
                return variation.Equals( content.variation )
                    && outlet.Equals( content.outlet )
                    && isSearchable.Equals( content.isSearchable )
                    && position == content.position
                    && type.Equals( content.type )
                    && isAvailable == content.isAvailable;
            }

            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Returns the hascode computed by its elements
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return ( variation + outlet + isSearchable.ToString() + position.ToString() + type + isAvailable.ToString() ).GetHashCode();
        }
    }
}
