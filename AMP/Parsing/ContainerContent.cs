using Anfema.Amp.DataModel;
using System.Collections.Generic;


namespace Anfema.Amp.Parsing
{
    public class ContainerContent
    {
        public string type { get; set; }
        public string variation { get; set; }
        public string outlet { get; set; }
        public List<AmpContent> children { get; set; }
    }
}
