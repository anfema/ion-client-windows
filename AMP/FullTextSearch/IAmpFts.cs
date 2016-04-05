using Anfema.Amp.DataModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anfema.Amp.FullTextSearch
{
    public interface IAmpFts : IAmpConfigUpdateable
    {
        Task<String> DownloadSearchDatabase();
        Task<List<SearchResult>> FullTextSearch( String searchTerm, String locale, String pageLayout = null );
    }
}
