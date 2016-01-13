using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.DataModel
{
    public class AmpPageContent
    {
        public string type { get; set; }
        public string variation { get; set; }
        public string outlet { get; set; }
        public List<AmpPageObservableCollection> children { get; set; }

        public AmpPageContent()
        {
            children = new List<AmpPageObservableCollection>();
        }
    }
}
