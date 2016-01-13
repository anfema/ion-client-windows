using System.Collections.Generic;

namespace Anfema.Amp.Parsing
{
    public class AmpCollection
    {
        public string identifier { get; set; }
        public string default_locale { get; set; }
        public string fts_db { get; set; }
        public string archive { get; set; }
        public List<CollectionPage> pages { get; set; }
    }
}
