using Anfema.Ion.DataModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anfema.Ion.FullTextSearch
{
    public interface IIonFts : IIonConfigUpdateable
    {
        Task<String> DownloadSearchDatabase();
        Task<List<SearchResult>> FullTextSearch( String searchTerm, String locale, String pageLayout = null );
    }
}
