using System;
using System.Collections.Generic;

namespace Anfema.Ion.DataModel
{
    public class IonCollection
    {
        public string identifier { get; set; }
        public string default_locale { get; set; }
        public Uri fts_db { get; set; }
        public Uri archive { get; set; }
        public List<IonPagePreview> pages { get; set; }
        public DateTime last_changed { get; set; }


        public DateTime getPageLastChanged( string pageIdentifier )
        {
            return pages.Find( x => x.identifier.Equals( pageIdentifier ) ).lastChanged;
        }
    }
}
