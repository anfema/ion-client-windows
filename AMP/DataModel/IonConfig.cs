using System.Net.Http.Headers;

namespace Anfema.Ion.DataModel
{
    public class IonConfig
    {
        public string baseUrl { get; set; }
        public string collectionIdentifier { get; set; }
        public AuthenticationHeaderValue authenticationHeader { get; set; }
        public string locale { get; set; }
        public int minutesUntilCollectionRefresh { get; set; }
        public int pagesMemCacheSize { get; set; }
        public bool archiveDownloads { get; set; }
        public string variation { get; set; }


        /// <summary>
        /// Constructor with all parameters set
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="locale"></param>
        /// <param name="collectionIdentifier"></param>
        /// <param name="variation"></param>
        /// <param name="authenticationHeader"></param>
        /// <param name="minutesUntilCollectionRefresh"></param>
        /// <param name="pagesMemCacheSize"></param>
        /// <param name="archiveDownloads"></param>
        public IonConfig( string baseUrl, string locale, string collectionIdentifier, string variation, AuthenticationHeaderValue authenticationHeader, int minutesUntilCollectionRefresh, int pagesMemCacheSize, bool archiveDownloads )
        {
            this.baseUrl = baseUrl;
            this.collectionIdentifier = collectionIdentifier;
            this.locale = locale;
            this.variation = variation;
            this.authenticationHeader = authenticationHeader;
            this.minutesUntilCollectionRefresh = minutesUntilCollectionRefresh;
            this.pagesMemCacheSize = pagesMemCacheSize;
            this.archiveDownloads = archiveDownloads;
        }


        /// <summary>
        /// Constructor with all parameters except "variation§
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="locale"></param>
        /// <param name="collectionIdentifier"></param>
        /// <param name="authenticationHeader"></param>
        /// <param name="minutesUntilCollectionRefresh"></param>
        /// <param name="pagesMemCacheSize"></param>
        /// <param name="archiveDownloads"></param>
        public IonConfig( string baseUrl, string locale, string collectionIdentifier, AuthenticationHeaderValue authenticationHeader, int minutesUntilCollectionRefresh, int pagesMemCacheSize, bool archiveDownloads )
            : this( baseUrl, locale, collectionIdentifier, "default", authenticationHeader, minutesUntilCollectionRefresh, pagesMemCacheSize, archiveDownloads )
        { }


        /// <summary>
        /// Checks the given Ampconfig for equality based uppon the content of both AmpConfigs
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if the content is equal and false otherwise</returns>
        public override bool Equals( object obj )
        {
            if( obj == this )
            {
                return true;
            }

            if( obj == null )
            {
                return false;
            }

            try
            {
                // Cast to IonConfig for accessing the elements
                IonConfig config = (IonConfig)obj;

                return config.baseUrl.Equals( baseUrl )
                    && config.collectionIdentifier.Equals( collectionIdentifier )
                    && config.locale.Equals( locale )
                    && config.variation.Equals( variation );
            }

            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Returns the exact hashCode that the base class would do
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
