using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.Parsing
{
    public class PageRaw
    {
        public string parent { get; set; }
        public string identifier { get; set; }
        public string collection { get; set; }
        public DateTime last_changed { get; set; }
        public Uri archive { get; set; }
        public List<ContentRaw> contents { get; set; }
        public List<string> children { get; set; }
        public string locale { get; set; }
        public string layout { get; set; }
    }
}
