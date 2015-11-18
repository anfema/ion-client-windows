using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.DataModel
{
    public class AmpPageTranslationContent
    {
        public string type { get; set; }
        public string variation { get; set; }
        public string outlet { get; set; }
        public List<AmpPageObservableCollection> children { get; set; }

        public AmpPageTranslationContent()
        {
            children = new List<AmpPageObservableCollection>();
        }
    }
}
