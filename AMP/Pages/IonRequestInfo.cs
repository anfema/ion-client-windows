using Anfema.Ion.Pages;

namespace Anfema.Ion.Pages
{
    public class IonRequestInfo
    {
        public IonRequestType requestType;
        public string locale;
        public string variation;
        public string collectionIdentifier;
        public string pageIdentifier;

        public IonRequestInfo( IonRequestType requestType, string locale, string variation, string collectionIdentifier, string pageIdentifier )
        {
            this.requestType = requestType;
            this.locale = locale;
            this.variation = variation;
            this.collectionIdentifier = collectionIdentifier;
            this.pageIdentifier = pageIdentifier;
        }
    }
}
