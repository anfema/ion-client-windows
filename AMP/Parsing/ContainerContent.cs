using Anfema.Ion.DataModel;
using Anfema.Ion.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace Anfema.Ion.Parsing
{
    public class ContainerContent
    {
        [JsonProperty( Order = 1 )]
        public string outlet { get; set; }

        [JsonProperty( Order = 2 )]
        public string type { get; set; }

        [JsonProperty( Order = 3 )]
        public List<IonContent> children { get; set; }

        [JsonProperty( Order = 4 )]
        public string variation { get; set; }


        /// <summary>
        /// Checks for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if ContainerContents are equal, false otherwise</returns>
        public override bool Equals( object obj )
        {
            if( obj == this )
            {
                return true;
            }

            if( obj == null )
            {
                return false;
            }

            try
            {
                // Try to cast
                ContainerContent content = (ContainerContent)obj;

                // Compare every value
                return outlet.Equals( content.outlet )
                    && type.Equals( content.type )
                    && EqualsUtils.UnorderedEqual( children, content.children )
                    && variation.Equals( content.variation );
            }

            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Returns the hashCode that is computed by its member values
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return ( outlet + type + variation ).GetHashCode();
        }
    }
}
