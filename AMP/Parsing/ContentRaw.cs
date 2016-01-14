using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.Parsing
{
    public class ContentRaw
    {
        public string type { get; set; }
        public string variation { get; set; }
        public string outlet { get; set; }
        public List<ContentNodeRaw> children { get; set; }
    }
}
