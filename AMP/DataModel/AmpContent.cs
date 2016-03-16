﻿using Anfema.Amp.Parsing;
using Newtonsoft.Json;

namespace Anfema.Amp.DataModel
{
    /// <summary>
    /// Content base class that all content classes should inheritate from
    /// </summary> 
    [JsonConverter( typeof(AmpContentConverter))]
    public class AmpContent
    {
        public string variation { get; set; }
        public string outlet { get; set; }

        [JsonProperty("is_searchable")]
        public bool isSearchable { get; set; }

        public int position { get; set; }
        public string type { get; set; }

        [JsonProperty("is_available")]
        public bool isAvailable { get; set; } = true;
    }
}
