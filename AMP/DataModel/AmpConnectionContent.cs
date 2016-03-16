using Anfema.Amp.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.DataModel
{
    public class AmpConnectionContent : AmpContent
    {
        [JsonProperty("connection_string")]
        public string connectionString { get; set; }
    }
}
