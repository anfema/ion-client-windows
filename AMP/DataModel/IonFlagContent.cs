﻿using Anfema.Ion.Utils;
using Newtonsoft.Json;


namespace Anfema.Ion.DataModel
{
    public class IonFlagContent : IonContent
    {
        [JsonProperty( "is_enabled" )]
        public bool enabled { get; set; }


        public override bool Equals( object obj )
        {
            // Basic IonContent equality check
            if( !base.Equals( obj ) )
            {
                return false;
            }

            try
            {
                // Try to parse
                IonFlagContent content = (IonFlagContent)obj;

                return enabled == content.enabled;
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
            return EqualsUtils.calcHashCode( outlet, enabled, type );
        }
    }
}
