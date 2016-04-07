using Anfema.Ion.DataModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Anfema.Ion.Pages
{
    public interface IIonPages : IIonConfigUpdateable
    {
        Task<IonCollection> getCollectionAsync();
        Task<IonPage> getPageAsync( string pageIdentifier );
        Task<List<string>> getAllPagesIdentifierAsync();
        Task<List<IonPage>> getPagesAsync( Predicate<IonPagePreview> filter );
        Task<List<IonPagePreview>> getPagePreviewsAsync( Predicate<IonPagePreview> filter );
    }
}
