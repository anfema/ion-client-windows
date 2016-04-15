using Anfema.Ion.DataModel;
using Anfema.Ion.Utils;
using System;
using Windows.Storage;

namespace Anfema.Ion.Caching
{
    public class FilePaths
    {
        private static readonly String _ftsDbFolderPath = "ftsDbs";
        private static readonly string _mediaPath = "media";
        private static readonly string _archiveFolder = "archive";
        private static readonly string _fileFolder = "file";
        private static readonly string _tempFolder = "temp";
        private static readonly string _cacheIndicesFolder = "cache_indices";
        private static readonly string _slash = "\\";

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


        //public static string getFileFilePath( string url, IonConfig config, bool tempCollectionFolder = false )
        //{
        //    string fileFolderPath = getFileFolderPath( url, config, tempCollectionFolder );
        //    string filename = getFileName( url );
        //    return fileFolderPath + filename;
        //}


        public static string getArchiveFilePath( string url, IonConfig config, bool tempCollectionFolder = false )
        {
            string archiveFolderPath = getArchiveFolderPath( config, tempCollectionFolder );
            string filename = getFileName( url );
            return archiveFolderPath + filename;
        }
        

        public static String GetFtsDbFilePath( String collectionIdentifier )
        {
            return _ftsDbFolderPath + FileUtils.SLASH + "fts_" + collectionIdentifier + ".sqlite";
        }


        public static string getTempFolderPath( IonConfig config )
        {
            return config.collectionIdentifier + FileUtils.SLASH + _tempFolder;
        }


        /// <summary>
        /// Returns the folder to which the archives are downloaded
        /// </summary>
        /// <param name="url"></param>
        /// <param name="config"></param>
        /// <returns>Archive folder</returns>
        public static string getArchiveFolderPath( IonConfig config, bool tempCollectionFolder )
        {
            return config.collectionIdentifier + FileUtils.SLASH + _archiveFolder + FileUtils.SLASH;
        }


        /// <summary>
        /// Returns the folder to which the files are downloaded
        /// </summary>
        /// <param name="url"></param>
        /// <param name="config"></param>
        /// <returns>File folder</returns>
        //private static string getFileFolderPath( string url, IonConfig config, bool tempCollectionFolder )
        //{
        //    return config.collectionIdentifier + FileUtils.SLASH + _fileFolder + FileUtils.SLASH + AppendTemp( "file", tempCollectionFolder );
        //}


        /// <summary>
        /// Get local file cache folder path for an Ion configuration/collection
        /// </summary>
        /// <param name="config"></param>
        /// <param name="tempCollectionFolder"></param>
        /// <returns></returns>
        public static String getMediaFolderPath( IonConfig config, bool tempCollectionFolder )
        {
            return config.collectionIdentifier + FileUtils.SLASH + _mediaPath + FileUtils.SLASH;
        }


        private static String AppendTemp( String path, bool appendTemp )
        {
            if( appendTemp )
            {
                path += "_temp";
            }
            return path;
        }


        /// <summary>
        /// Folder for cache indices
        /// </summary>
        public static string CACHE_FOLDER_IDENTIFIER
        {
            get
            {
                return _cacheIndicesFolder;
            }
        }


        /// <summary>
        /// Absolut path to the cache indices folder
        /// </summary>
        public static string absolutCacheIndicesFolderPath( IonConfig config )
        {
            return ApplicationData.Current.LocalFolder.Path + _slash + config.collectionIdentifier + _slash + _cacheIndicesFolder;
        }


        /// <summary>
        /// Slash for folder hierarchy
        /// </summary>
        public static string SLASH
        {
            get
            {
                return _slash;
            }
        }
    }
}
