using Anfema.Amp.DataModel;
using Anfema.Amp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.Caching
{
    class FileCacheIndex : CacheIndex
    {
        public String checksum { get; set; }
        
        public DateTime lastUpdated { get; set; }

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

        public bool IsOutdated( String serverChecksum )
        {
            return String.Equals( this.checksum, serverChecksum );
        }

        public static async Task<FileCacheIndex> retrieve( String requestUrl, String collectionIdentifier )
        {
            return await CacheIndexStore.retrieve<FileCacheIndex>( requestUrl, collectionIdentifier );
	    }

        public static async Task<bool> save( String requestUrl, Stream file, AmpConfig config, String checksum )
        {
            if ( file == null )
            {
                return false;
            }

            if ( checksum == null )
            {
                checksum = "sha256:" + HashUtils.GetSHA256Hash( file );
            }

            FileCacheIndex cacheIndex = new FileCacheIndex( requestUrl, checksum, DateTime.Now );
            await CacheIndexStore.save<FileCacheIndex>( requestUrl, cacheIndex, config );

            return true;
        }
    }
}
