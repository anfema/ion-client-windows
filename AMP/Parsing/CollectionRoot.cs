using Anfema.Ion.DataModel;
using System.Collections.Generic;

namespace Anfema.Ion.Parsing
{
    public class CollectionRoot
    {
        //public CollectionMeta meta { get; set; }
        public List<IonCollection> collection { get; set; }


        public CollectionRoot()
        {
            collection = new List<IonCollection>();
        }
    }
}
