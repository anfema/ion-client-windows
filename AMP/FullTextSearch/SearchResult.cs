using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.FullTextSearch
{
    public class SearchResult
    {
        public string page { get; set; }
        public string text { get; set; }
        public string layout { get; set; }
    }
}
