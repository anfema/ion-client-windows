using Anfema.Amp.DataModel;
using System.Collections.Generic;

namespace Anfema.Amp.Parsing
{
    public class CollectionRoot
    {
        public CollectionMeta meta { get; set; }
        public List<AmpCollection> collection { get; set; }
    }
}
