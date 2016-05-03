using Anfema.Ion.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Anfema.Ion.DataModel
{
    public class IonCollection
    {
        [JsonProperty( Order = 1 )]
        public string identifier { get; set; }

        [JsonProperty( Order = 2 )]
        public string default_locale { get; set; }

        [JsonProperty( Order = 3 )]
        public DateTime last_changed { get; set; }

        [JsonProperty( Order = 4 )]
        public Uri fts_db { get; set; }

        [JsonProperty( Order = 5 )]
        public Uri archive { get; set; }

        [JsonProperty( Order = 6 )]
        public List<IonPagePreview> pages { get; set; }



        public DateTime getPageLastChanged( string pageIdentifier )
        {
            return pages.Find( x => x.identifier.Equals( pageIdentifier ) ).lastChanged;
        }


        /// <summary>
        /// Checks a given collection's property for equality with this collection
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if collections are equal, false otherwise</returns>
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
                IonCollection content = (IonCollection)obj;

                // Check all elements for equality
                return identifier.Equals( content.identifier )
                    && default_locale.Equals( content.default_locale )
                    && last_changed == content.last_changed
                    && fts_db.Equals( content.fts_db )
                    && archive.Equals( content.archive )
                    && EqualsUtils.UnorderedEqual( pages, content.pages );
            }

            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Calculates the hashCode of this collection by its unique identifier
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return EqualsUtils.calcHashCode( identifier, default_locale );
        }
    }
}
