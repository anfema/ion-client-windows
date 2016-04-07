using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Ion.DataModel
{
    public class IonPageContent
    {
        public string type { get; set; }
        public string variation { get; set; }
        public string outlet { get; set; }
        public List<IonPageObservableCollection> children { get; set; }

        public IonPageContent()
        {
            children = new List<IonPageObservableCollection>();
        }
    }
}
