using Anfema.Amp.DataModel;
using Anfema.Amp.Utils;
using System;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;



namespace Anfema.Amp.Caching
{
    public class FilePaths
    {
        /// <summary>
        /// Get filename for a url as its MD5 hash
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFileName( string url )
        {
            return HashUtils.GetMD5Hash( url );
        }
        
        /// <summary>
        /// Get local file cache path for media url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static String GetMediaFilePath( String url, AmpConfig config )
        {
            return GetMediaFilePath( url, config, false );
        }

        /// <summary>
        /// Get local file cache path for media url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="config"></param>
        /// <param name="tempCollectionFolder"></param>
        /// <returns></returns>
        public static String GetMediaFilePath( String url, AmpConfig config, bool tempCollectionFolder )
        {
            String mediaFolderPath = GetMediaFolderPath( config, tempCollectionFolder );
            String filename = GetFileName( url );
            return mediaFolderPath + filename;
        }

        /// <summary>
        /// Get local file cache folder path for an amp configuration/collection
        /// </summary>
        /// <param name="config"></param>
        /// <param name="tempCollectionFolder"></param>
        /// <returns></returns>
        public static String GetMediaFolderPath( AmpConfig config, bool tempCollectionFolder )
        {
            return config.collectionIdentifier + FileUtils.SLASH + AppendTemp( "media", tempCollectionFolder );
        }

        private static String AppendTemp( String path, bool appendTemp )
        {
            if ( appendTemp )
            {
                path += "_temp";
            }
            return path;
        }
    }
}
