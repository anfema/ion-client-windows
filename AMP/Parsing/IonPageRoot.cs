using Anfema.Ion.DataModel;
using System.Collections.Generic;


namespace Anfema.Ion.Parsing
{
    public class IonPageRoot
    {
        public List<IonPage> page { get; set; }


        public IonPageRoot()
        {
            page = new List<IonPage>();
        }
    }
}
