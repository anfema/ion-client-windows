using Anfema.Ion.DataModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anfema.Ion.FullTextSearch
{
    public interface IIonFts : IIonConfigUpdateable
    {
        Task<string> DownloadSearchDatabaseAsync();
        Task<List<SearchResult>> FullTextSearchAsync( string searchTerm, string locale, string pageLayout = null );
    }
}
