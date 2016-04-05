using Anfema.Amp.Caching;
using Anfema.Amp.DataModel;
using Anfema.Amp.Utils;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Anfema.Amp.MediaFiles
{
    public class AmpFilesWithCaching : IAmpFiles
    {
        // Config associated with this collection of pages
        private AmpConfig _config;

        // Data client that will be used to get the data from the server
        private DataClient _dataClient;

        private OperationLocks downloadLocks = new OperationLocks();

        public AmpFilesWithCaching(AmpConfig config)
        {
            _config = config;

            // Init the data client
            _dataClient = new DataClient(config);
        }

        /// <summary>
        /// Request file from url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="checksum"></param>
        /// <param name="ignoreCaching"></param>
        /// <param name="inTargetFile"></param>
        /// <returns></returns>
        public async Task<StorageFile> Request(String url, String checksum, Boolean ignoreCaching)
        {
            String targetFile = FilePaths.GetMediaFilePath(url, _config);
            //if ( ignoreCaching )
            //{
            //    return await _dataClient.PerformRequest( new Uri( url ) );
            //}

            StorageFile returnFile = null;
            using (await downloadLocks.ObtainLock(url).LockAsync().ConfigureAwait(false))
            {
                // fetch file from local storage or download it?
                if (await FileUtils.Exists(targetFile) && await IsFileUpToDate(url, checksum).ConfigureAwait(false))
                {
                    // retrieve current version from cache
                    returnFile = await FileUtils.ReadFromFile(targetFile).ConfigureAwait(false);
                }
                else if (NetworkUtils.isOnline())
                {
                    // download media file
                    MemoryStream saveStream = await _dataClient.PerformRequest(new Uri(url)).ConfigureAwait(false);

                    // save data to file
                    returnFile = await FileUtils.WriteToFile(saveStream, targetFile).ConfigureAwait(false);
                    await FileCacheIndex.save(url, saveStream, _config, checksum).ConfigureAwait(false);
                }
                else if (await FileUtils.Exists(targetFile).ConfigureAwait(false))
                {
                    // TODO notify app that data might be outdated
                    // no network: use old version from cache (even if no cache index entry exists)
                    returnFile = await FileUtils.ReadFromFile(targetFile);
                }
            }
            downloadLocks.ReleaseLock(url);
            if (returnFile == null)
            {
                throw new Exception("Media file " + url + " is not in cache and no internet connection is available.");
            }
            return returnFile;
        }


        /// <summary>
        /// Updates the AmpConfig file
        /// </summary>
        /// <param name="config"></param>
        public void updateConfig( AmpConfig config )
        {
            _config = config;
        }


        private async Task<bool> IsFileUpToDate(String url, String checksum)
        {
            FileCacheIndex fileCacheIndex = await FileCacheIndex.retrieve(url, _config.collectionIdentifier).ConfigureAwait(false);
            if (fileCacheIndex == null)
            {
                return false;
            }

            if (checksum != null)
            {
                // check with file's checksum
                return !fileCacheIndex.IsOutdated(checksum);
            }
            else
            {
                // check with collection's last_modified (previewPage.last_changed would be slightly more precise)
                CollectionCacheIndex collectionCacheIndex = await CollectionCacheIndex.retrieve(_config).ConfigureAwait(false);
                DateTime collectionLastModified = collectionCacheIndex == null ? default(DateTime) : collectionCacheIndex.lastModified;
                return collectionLastModified != null && fileCacheIndex.lastUpdated != null && !(collectionLastModified.CompareTo(fileCacheIndex.lastUpdated) > 0);
            }
        }
    }
}
