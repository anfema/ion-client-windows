using System.Net.Http.Headers;

namespace Anfema.Amp.DataModel
{
    public class AmpConfig
    {
        public string baseUrl { get; set; }
        public string collectionIdentifier { get; set; }
        public AuthenticationHeaderValue authenticationHeader { get; set; }
        public string locale { get; set; }
        public int minutesUntilCollectionRefresh { get; set; }
        public int pagesMemCacheSize { get; set; }
        public bool archiveDownloads { get; set; }



        public AmpConfig( string baseUrl, string locale, string collectionIdentifier, AuthenticationHeaderValue authenticationHeader, int minutesUntilCollectionRefresh, int pagesMemCacheSize, bool archiveDownloads)
        {
            this.baseUrl = baseUrl;
            this.collectionIdentifier = collectionIdentifier;
            this.locale = locale;
            this.authenticationHeader = authenticationHeader;
            this.minutesUntilCollectionRefresh = minutesUntilCollectionRefresh;
            this.pagesMemCacheSize = pagesMemCacheSize;
            this.archiveDownloads = archiveDownloads;
        }
    }
}
