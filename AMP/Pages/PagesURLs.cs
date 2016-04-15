using Anfema.Ion.Caching;
using Anfema.Ion.DataModel;
using Anfema.Ion.Exceptions;
using Anfema.Ion.Utils;

namespace Anfema.Ion.Pages
{
    public class PagesURLs
    {
        /// <summary>
        /// Composes a collection URL from a given config
        /// </summary>
        /// <param name="config"></param>
        /// <returns>String representing the link to the collection</returns>
        public static string getCollectionURL( IonConfig config )
        {
            return config.baseUrl + config.locale + "/" + config.collectionIdentifier;
        }


        /// <summary>
        /// Composes a URL to a specific page
        /// </summary>
        /// <param name="config"></param>
        /// <param name="pageIdentifier"></param>
        /// <returns>String representing the URL to the desired page</returns>
        public static string getPageURL( IonConfig config, string pageIdentifier )
        {
            return config.baseUrl + config.locale + "/" + config.collectionIdentifier + "/" + pageIdentifier;
        }


        public static IonRequestInfo analyze( string url, IonConfig config )
        {
            if( isMediaRequestUrl( url ) )
            {
                return new IonRequestInfo( IonRequestType.MEDIA, null, null, null, null );
            }
            else
            {
                string relativeUrlPath = url.Replace( config.baseUrl, "" );
                string[] urlPathSegments = relativeUrlPath.Split( '/' );

                if( urlPathSegments.Length < 2 || urlPathSegments.Length > 3 )
                {
                    throw new NoIonPagesRequestException( url );
                }
                
                string[] idPlusVariation = urlPathSegments[urlPathSegments.Length - 1].Split( '?' );
                if( idPlusVariation.Length > 2 )
                {
                    throw new NoIonPagesRequestException( url );
                }

                if( idPlusVariation.Length == 1 )
                {
                    idPlusVariation = new string[] { idPlusVariation[0], "default" }; // TODO make this a global defined parameter "default"
                }

                string locale = urlPathSegments[0];
                string variation = idPlusVariation[1];
                string collectionIdentifier;

                if( urlPathSegments.Length == 2 )
                {
                    collectionIdentifier = idPlusVariation[0];
                    return new IonRequestInfo( IonRequestType.COLLECTION, locale, variation, collectionIdentifier, null );
                }
                else // urlPathSegments.length == 3
                {
                    collectionIdentifier = urlPathSegments[1];
                    string pageIdentifier = idPlusVariation[0];
                    return new IonRequestInfo( IonRequestType.PAGE, locale, variation, collectionIdentifier, pageIdentifier );
                }
            }
        }


        /// <summary>
        /// Analyzes a url to be a media request URL or not
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool isMediaRequestUrl( string url )
        {
            for( int i = 0; i < IonConstants.MediaUrlIndicators.Length; i++ )
            {
                if( url.Contains( IonConstants.MediaUrlIndicators[i] ) )
                {
                    return true;
                }
            }
            return false;
        }
    }
}
