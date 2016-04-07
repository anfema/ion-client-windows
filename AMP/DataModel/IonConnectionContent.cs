using Anfema.Ion.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Ion.DataModel
{
    public class IonConnectionContent : IonContent
    {
        [JsonProperty( "connection_string" )]
        public string connectionString { get; set; }
    }
}
