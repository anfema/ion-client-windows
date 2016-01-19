using System.Net.Http.Headers;

namespace Anfema.Amp.DataModel
{
    public class AmpConfig
    {
        public string baseUrl { get; set; }
        public string collectionIdentifier { get; set; }
        public AuthenticationHeaderValue authenticationHeader { get; set; }
        public string locale { get; set; }


        public AmpConfig( string baseUrl, string collectionIdentifier, string locale, AuthenticationHeaderValue authenticationHeader)
        {
            this.baseUrl = baseUrl;
            this.collectionIdentifier = collectionIdentifier;
            this.locale = locale;
            this.authenticationHeader = authenticationHeader;
        }
    }
}
