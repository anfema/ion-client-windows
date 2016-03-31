using Anfema.Amp.Caching;
using Anfema.Amp.DataModel;
using Anfema.Amp.MediaFiles;
using Anfema.Amp.Pages;
using Anfema.Amp.Parsing;
using Anfema.Amp.Utils;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Anfema.Amp.FullTextSearch
{
    public class AmpFtsImpl : IAmpFts
    {
        // Config associated with this collection of pages
        private AmpConfig _config;

        // Data client that will be used to get the data from the server
        private DataClient _dataClient;

        private IAmpPages _ampPages;
        private IAmpFiles _ampFiles;

        SQLiteAsyncConnection _connection;
        private String _ftsQuery;

        public AmpFtsImpl( IAmpPages ampPages, IAmpFiles ampFiles, AmpConfig config )
        {
            this._ampPages = ampPages;
            this._ampFiles = ampFiles;
            this._config = config;

            // Init the data client
            _dataClient = new DataClient( config );

            _ftsQuery = new ResourceLoader("Amp/Resources").GetString("ftsQuery");
            String dbFilePath = Path.Combine( Windows.Storage.ApplicationData.Current.LocalFolder.Path, FilePaths.GetFtsDbFilePath( _config.collectionIdentifier ) );
            _connection = new SQLiteAsyncConnection( () =>
                                                   new SQLiteConnectionWithLock( new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(),
                                                                                    new SQLiteConnectionString( dbFilePath, false,
                                                                                        openFlags: SQLite.Net.Interop.SQLiteOpenFlags.ReadOnly ) ) );
        }

        public async Task<String> DownloadSearchDatabase()
        {
            String dbTargetFile = FilePaths.GetFtsDbFilePath( _config.collectionIdentifier );
            AmpCollection ampCollection = await _ampPages.getCollectionAsync().ConfigureAwait(false);
            
            // Load fts db from server and save data to file
            using ( MemoryStream responseStream = await _dataClient.PerformRequest( ampCollection.fts_db ).ConfigureAwait(false) )
            {
                await FileUtils.WriteToFile( responseStream, dbTargetFile ).ConfigureAwait(false);
            }
            
            return dbTargetFile;
        }

        public async Task<List<SearchResult>> FullTextSearch( String searchTerm, String locale, String pageLayout )
        {
            searchTerm = PrepareSearchTerm( searchTerm );

            List<String> args = new List<string>();
            args.Add( locale );

            String additionalFilters = String.Empty;
            bool searchTermFilter = !( searchTerm == String.Empty );
            if ( searchTermFilter )
            {
                additionalFilters += "AND s.search MATCH ?2 ";
                args.Add( searchTerm );
            }

            if ( pageLayout != null )
            {
                if ( searchTermFilter )
                {
                    additionalFilters += "AND c.layout = ?3 ";
                }
                else
                {
                    additionalFilters += "AND c.layout = ?2 ";
                }
                
                args.Add( pageLayout );
            }

            String query = _ftsQuery.Replace( "<additional_filters>", additionalFilters );
            
            return await _connection.QueryAsync<SearchResult>( query, args.ToArray() ).ConfigureAwait(false);
        }
        
        public String PrepareSearchTerm( String searchTerm )
        {
            if ( searchTerm == null || searchTerm == String.Empty )
            {
                return String.Empty;
            }

            Array words = searchTerm.Split(' ');
            String searchTermModified = String.Empty;
            foreach ( String word in words )
            {
                searchTermModified += word + "* ";
            }

            return searchTermModified;
        }
    }
}
