using Anfema.Ion.DataModel;
using Anfema.Ion.Utils;
using System;


namespace Anfema.Ion.Caching
{
    public class FilePaths
    {
        private static readonly String _ftsDbFolderPath = "ftsDbs";
        private static readonly string _mediaPath = "media";
        private static readonly string _archiveFolder = "archive";
        private static readonly string _fileFolder = "file";

        /// <summary>
        /// Get filename for a url as its MD5 hash
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string getFileName( string url )
        {
            return HashUtils.GetMD5Hash( url );
        }


        /// <summary>
        /// Get local file cache path for media url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="config"></param>
        /// <param name="tempCollectionFolder"></param>
        /// <returns></returns>
        public static String getMediaFilePath( String url, IonConfig config, bool tempCollectionFolder = false )
        {
            String mediaFolderPath = getMediaFolderPath( config, tempCollectionFolder );
            String filename = getFileName( url );
            return mediaFolderPath + filename;
        }


        public static string getFileFilePath( string url, IonConfig config, bool tempCollectionFolder = false )
        {
            string fileFolderPath = getFileFolderPath( url, config, tempCollectionFolder );
            string filename = getFileName( url );
            return fileFolderPath + filename;
        }


        public static string getArchiveFilePath( string url, IonConfig config, bool tempCollectionFolder = false )
        {
            string archiveFolderPath = getArchiveFolderPath( url, config, tempCollectionFolder );
            string filename = getFileName( url );
            return archiveFolderPath + filename;
        }


        /// <summary>
        /// Get local file cache folder path for an Ion configuration/collection
        /// </summary>
        /// <param name="config"></param>
        /// <param name="tempCollectionFolder"></param>
        /// <returns></returns>
        private static String getMediaFolderPath( IonConfig config, bool tempCollectionFolder )
        {
            return config.collectionIdentifier + FileUtils.SLASH + _mediaPath + FileUtils.SLASH + AppendTemp( "media", tempCollectionFolder );
        }

        private static String AppendTemp( String path, bool appendTemp )
        {
            if( appendTemp )
            {
                path += "_temp";
            }
            return path;
        }

        public static String GetFtsDbFilePath( String collectionIdentifier )
        {
            return _ftsDbFolderPath + FileUtils.SLASH + "fts_" + collectionIdentifier + ".sqlite";
        }


        /// <summary>
        /// Returns the folder to which the archives are downloaded
        /// </summary>
        /// <param name="url"></param>
        /// <param name="config"></param>
        /// <returns>Archive folder</returns>
        private static string getArchiveFolderPath( string url, IonConfig config, bool tempCollectionFolder )
        {
            return config.collectionIdentifier + FileUtils.SLASH + _archiveFolder + FileUtils.SLASH + AppendTemp( "archive", tempCollectionFolder );
        }


        /// <summary>
        /// Returns the folder to which the files are downloaded
        /// </summary>
        /// <param name="url"></param>
        /// <param name="config"></param>
        /// <returns>File folder</returns>
        private static string getFileFolderPath( string url, IonConfig config, bool tempCollectionFolder )
        {
            return config.collectionIdentifier + FileUtils.SLASH + _fileFolder + FileUtils.SLASH + AppendTemp( "file", tempCollectionFolder );
        }
    }
}
