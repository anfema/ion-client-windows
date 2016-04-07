using Anfema.Ion.DataModel;
using System.Collections.Generic;


namespace Anfema.Ion.Parsing
{
    public class ContainerContent
    {
        public string type { get; set; }
        public string variation { get; set; }
        public string outlet { get; set; }
        public List<IonContent> children { get; set; }
    }
}
