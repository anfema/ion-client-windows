using Anfema.Ion.DataModel;
using Anfema.Ion.Utils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Anfema.Ion.Caching
{
    class FileCacheIndex : CacheIndex
    {
        public String checksum { get; set; }        // Used to store the checksum of the file
        public DateTime lastUpdated { get; set; }   // Used to store the last time, the file was checked for modification

        public FileCacheIndex() { }

        public FileCacheIndex( String filename, String checksum, DateTime lastUpdated ) : base( filename )
        {
            this.checksum = checksum;
            this.lastUpdated = lastUpdated;
        }

        public FileCacheIndex( Uri requestUrl, String checksum, DateTime lastUpdated ) : base( requestUrl )
        {
            this.checksum = checksum;
            this.lastUpdated = lastUpdated;
        }

        /// <summary>
        /// Checks if the passed checksum is equal to the cache items checksum
        /// </summary>
        /// <param name="serverChecksum"></param>
        /// <returns></returns>
        public bool IsOutdated( String serverChecksum )
        {
            return !String.Equals( checksum, serverChecksum );
        }

        /// <summary>
        /// Retrieves a FileCacheIndex from cache
        /// </summary>
        /// <param name="requestURL"></param>
        /// <param name="collectionIdentifier"></param>
        /// <returns>FileCacheIndex object or null, if the index isn't found</returns>
        public static async Task<FileCacheIndex> retrieve( string requestUrl, IonConfig config )
        {
            return await CacheIndexStore.retrieve<FileCacheIndex>( requestUrl, config ).ConfigureAwait( false );
        }

        /// <summary>
        /// Saves a FileCacheIndex object to the cache
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="file"></param>
        /// <param name="config"></param>
        /// <param name="checksum"></param>
        public static async Task save( string requestUrl, Stream file, IonConfig config, string checksum )
        {
            if( file == null )
            {
                return;
            }

            if( checksum == null )
            {
                checksum = "sha256:" + HashUtils.GetSHA256Hash( file );
            }

            FileCacheIndex cacheIndex = new FileCacheIndex( requestUrl, checksum, DateTimeUtils.now() );
            await CacheIndexStore.save( requestUrl, cacheIndex, config ).ConfigureAwait( false );
        }
    }
}
