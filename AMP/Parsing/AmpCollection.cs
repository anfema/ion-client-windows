using System.Collections.Generic;

namespace Anfema.Amp.Parsing
{
    public class AmpCollection
    {
        public string id { get; set; }
        public string identifier { get; set; }
        public string name { get; set; }
        public string default_locale { get; set; }
        public List<CollectionPage> pages { get; set; }
    }
}
