using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.FullTextSearch
{
    public interface IAmpFts
    {
        Task<String> DownloadSearchDatabase();
        Task<List<SearchResult>> FullTextSearch( String searchTerm, String locale, String pageLayout = null );
    }
}
