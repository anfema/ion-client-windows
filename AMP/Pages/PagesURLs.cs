using Anfema.Amp.DataModel;


namespace Anfema.Amp.Pages
{
    public class PagesURLs
    {
        public static string getCollectionURL( AmpConfig config )
        {
            return config.baseUrl + config.locale + "/" + config.collectionIdentifier;
        }


        public static string getPageURL( AmpConfig config, string pageIdentifier )
        {
            return config.baseUrl + config.locale + "/" + config.collectionIdentifier + "/" + pageIdentifier;
        }
    }
}
