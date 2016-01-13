using System;

namespace Anfema.Amp.Parsing
{
    public class CollectionPage
    {
        public string identifier { get; set; }
        public string collection_identifier { get; set; }
        public int version_number { get; set; }
        public DateTime last_changed { get; set; }
        public int position { get; set; }
        public string layout { get; set; }
        public string locale { get; set; }
        public object parent { get; set; }
        public CollectionPageMeta meta { get; set; }
    }
}