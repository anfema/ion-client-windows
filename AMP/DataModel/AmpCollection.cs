using System;
using System.Collections.Generic;

namespace Anfema.Amp.Parsing
{
    public class AmpCollection
    {
        public string identifier { get; set; }
        public string default_locale { get; set; }
        public Uri fts_db { get; set; }
        public Uri archive { get; set; }
        public List<PagePreview> pages { get; set; }
        public DateTime last_changed { get; set; }


        public DateTime getPageLastChanged( string pageIdentifier )
        {
            return pages.Find(x => x.identifier.Equals(pageIdentifier)).lastChanged;
        }
    }
}
