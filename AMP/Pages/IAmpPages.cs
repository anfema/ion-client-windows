using Anfema.Amp.DataModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Anfema.Amp.Pages
{
    public interface IAmpPages
    {
        Task<AmpCollection> getCollectionAsync();
        Task<AmpPage> getPageAsync(string pageIdentifier);
        Task<List<string>> getAllPagesIdentifierAsync();
        Task<List<AmpPage>> getPagesAsync(Predicate<PagePreview> filter);
        Task<List<PagePreview>> getPagePreviewsAsync(Predicate<PagePreview> filter);
    }
}
