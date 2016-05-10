#ION Windows 10 Universal Client

This code represents the current state of the ION-Clients for Windows 10 Universal. 

##Comments:

Work in progress <br>
Found bugs or new requests should be either written down into new ticket or directly mailed to me (Peter H.). 


##First steps:

1) First of all the user must create a valid authorizationHeader. In order to do this there are currently two methods:
> Basic Authorization <br>
Token Authorization

    async Task<AuthenticationHeaderValue> GetAuthHeaderValue( string username, string password, string loginAdress )

2) After that a IonConfig must be created with the following parameters:
>Base-URL <br>
locale <br>
CollectionIdentifier <br>
Variation <br>
The in 1) generated autorizationHeader <br>
Count of minutes to a refresh of the collection <br>
Size of memory cache

    IonConfig IonConfig( string baseUrl, string locale, string collectionIdentifier, string variation, authenticationHeaderValue authenticationHeader, int minutesUntilCollectionRefresh, int pagesMemCacheSize, bool archiveDownloads )
;


3) Now this config can be used to fetch a instance of ION, which can be used to call all available methods:

    Ion getInstance( IonConfig config );




##Basic API:


Get page

    async Task<IonPage> getPageAsync( string name, Action callback = null )


Get pages

    async Task<List<IonPage>> getPagesAsync( Predicate<IonPagePreview> filter = null, Action callback = null )

Get pagePreview

    async Task<IonPagePreview> getPagePreviewAsync( string identifier, Action callback = null )

Get pagePreviews

    async Task<List<IonPagePreview>> getPagePreviewsAsync( Predicate<IonPagePreview> filter = null, Action callback = null )

Load archive and save to cache

    async Task loadArchive( string url, Action callback = null )

Update config

    void updateConfig( IonConfig config )

