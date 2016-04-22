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
    }
}
