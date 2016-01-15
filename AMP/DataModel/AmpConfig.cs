using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.DataModel
{
    public class AmpConfig
    {
        public string baseUrl { get; set; }
        public string collectionIdentifier { get; set; }
        public string authorizationHeaderValue { get; set; }


        public AmpConfig( string baseUrl, string collectionIdentifier, string authorizationHeaderValue )
        {
            this.baseUrl = baseUrl;
            this.collectionIdentifier = collectionIdentifier;
            this.authorizationHeaderValue = authorizationHeaderValue;
        }
    }
}
