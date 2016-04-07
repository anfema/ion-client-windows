using Anfema.Ion.DataModel;


namespace Anfema.Ion.Pages
{
    public class PagesURLs
    {
        public static string getCollectionURL( IonConfig config )
        {
            return config.baseUrl + config.locale + "/" + config.collectionIdentifier;
        }


        public static string getPageURL( IonConfig config, string pageIdentifier )
        {
            return config.baseUrl + config.locale + "/" + config.collectionIdentifier + "/" + pageIdentifier;
        }
    }
}
