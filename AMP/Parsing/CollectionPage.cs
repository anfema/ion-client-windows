using System;

namespace Anfema.Amp.Parsing
{
    public class CollectionPage
    {
        public string identifier { get; set; }
        public object parent { get; set; }
        public DateTime last_changed { get; set; }
        public string title { get; set; }
        public Uri thumbnail { get; set; }
        public string layout { get; set; }
    }
}