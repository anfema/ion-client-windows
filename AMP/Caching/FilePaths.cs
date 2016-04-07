using Anfema.Ion.DataModel;
using Anfema.Ion.Utils;
using System;


namespace Anfema.Ion.Caching
{
    public class FilePaths
    {
        private static readonly String _ftsDbFolderPath = "ftsDbs";
        private static readonly string _mediaPath = "media";

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
        public static String GetMediaFilePath( String url, IonConfig config )
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
        public static String GetMediaFilePath( String url, IonConfig config, bool tempCollectionFolder )
        {
            String mediaFolderPath = GetMediaFolderPath( config, tempCollectionFolder );
            String filename = GetFileName( url );
            return mediaFolderPath + filename;
        }

        /// <summary>
        /// Get local file cache folder path for an Ion configuration/collection
        /// </summary>
        /// <param name="config"></param>
        /// <param name="tempCollectionFolder"></param>
        /// <returns></returns>
        public static String GetMediaFolderPath( IonConfig config, bool tempCollectionFolder )
        {
            return config.collectionIdentifier + FileUtils.SLASH + _mediaPath + FileUtils.SLASH + AppendTemp( "media", tempCollectionFolder );
        }

        private static String AppendTemp( String path, bool appendTemp )
        {
            if ( appendTemp )
            {
                path += "_temp";
            }
            return path;
        }

        public static String GetFtsDbFilePath( String collectionIdentifier )
        {
            return _ftsDbFolderPath + FileUtils.SLASH + "fts_" + collectionIdentifier + ".sqlite";
        }
    }
}
