using Anfema.Amp.Caching;
using Anfema.Amp.DataModel;
using Anfema.Amp.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.mediafiles
{
    public class AmpFilesWithCaching
    {
        private AmpConfig _config;
        private HttpClient _client = new HttpClient();

        public AmpFilesWithCaching( AmpConfig config )
        {
            try
            {
                _config = config;
                _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = _config.authenticationHeader;
            }
            catch ( Exception e )
            {
                Debug.WriteLine( "Error in configuring the mediafile client: " + e.Message );
            }
        }

        public async Task<MemoryStream> Request( String url, String checksum )
        {
            return await Request( url, checksum, false, null );
        }

        public async Task<MemoryStream> Request( String url, String checksum, Boolean ignoreCaching, String inTargetFile )
        {
            String targetFile = GetTargetFilePath( url, inTargetFile );
            if ( ignoreCaching )
            {
                return await PerformRequest( url );
            }

            // fetch file from local storage or download it?
            if ( await FileUtils.Exists( targetFile ) && await IsFileUpToDate( url, checksum ) )
            {
                // retrieve current version from cache
                return await FileUtils.ReadFromFile( targetFile );
            }
            else if ( NetworkUtils.isOnline() )
            {
                // download media file
                MemoryStream responseStream = await PerformRequest( url );

                // save data to file
                using ( MemoryStream saveStream = new MemoryStream() )
                {
                    responseStream.CopyTo( saveStream );
                    saveStream.Position = 0;
                    await FileUtils.WriteToFile( saveStream, targetFile );
                    await FileCacheIndex.save( url, saveStream, _config, checksum );
                }

                responseStream.Position = 0;
                return responseStream;
            }
            else if ( await FileUtils.Exists( targetFile ) )
            {
                // TODO notify app that data might be outdated
                // no network: use old version from cache (even if no cache index entry exists)
                return await FileUtils.ReadFromFile( targetFile );
            }
            else
            {
                throw new Exception( "Media file " + url + " is not in cache and no internet connection is available." );
            }
        }

        private async Task<MemoryStream> PerformRequest( String url )
        {
            Stream stream = await _client.GetStreamAsync( url );
            var memStream = new MemoryStream();
            await stream.CopyToAsync( memStream );
            memStream.Position = 0;
            return memStream;
        }

        private String GetTargetFilePath( String url, String targetFile )
        {
            if ( targetFile == null )
            {
                targetFile = FilePaths.GetMediaFilePath( url, _config );
            }
            return targetFile;
        }

        private async Task<bool> IsFileUpToDate( String url, String checksum )
        {
            FileCacheIndex fileCacheIndex = await FileCacheIndex.retrieve( url, _config.collectionIdentifier );
            if ( fileCacheIndex == null )
            {
                return false;
            }

            if ( checksum != null )
            {
                // check with file's checksum
                return !fileCacheIndex.IsOutdated( checksum );
            }
            else
            {
                // check with collection's last_modified (previewPage.last_changed would be slightly more precise)
                CollectionCacheIndex collectionCacheIndex = await CollectionCacheIndex.retrieve( _config );
                DateTime collectionLastModified = collectionCacheIndex == null ? default( DateTime )  : collectionCacheIndex.lastModified;
                return collectionLastModified != null && fileCacheIndex.lastUpdated != null && !( collectionLastModified.CompareTo( fileCacheIndex.lastUpdated ) > 0 );
            }
        }
    }
}
