using Anfema.Ion.DataModel;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace Anfema.Ion.Parsing
{
    public class ContainerContent
    {
        [JsonProperty( Order = 1 )]
        public string outlet { get; set; }

        [JsonProperty( Order = 2 )]
        public string type { get; set; }

        [JsonProperty( Order = 3 )]
        public List<IonContent> children { get; set; }

        [JsonProperty( Order = 4 )]
        public string variation { get; set; }


    }
}
