using Anfema.Ion.DataModel;
using Anfema.Ion.Utils;
using System;
using Windows.Storage;

namespace Anfema.Ion.Caching
{
    public class FilePaths
    {
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
            return IonConstants.FtsDbFolderIdentifier + IonConstants.Slash + "fts_" + collectionIdentifier + ".sqlite";
        }


        public static string getTempFolderPath( IonConfig config )
        {
            return config.collectionIdentifier + IonConstants.Slash + IonConstants.TempFolderIdentifier;
        }


        /// <summary>
        /// Returns the folder to which the archives are downloaded
        /// </summary>
        /// <param name="url"></param>
        /// <param name="config"></param>
        /// <returns>Archive folder</returns>
        public static string getArchiveFolderPath( IonConfig config, bool tempCollectionFolder )
        {
            return config.collectionIdentifier + IonConstants.Slash + IonConstants.ArchiveFolderIdentifier + IonConstants.Slash;
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
            return config.collectionIdentifier + IonConstants.Slash + IonConstants.MediaFolderIdentifier + IonConstants.Slash;
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
        /// Absolut path to the cache indices folder
        /// </summary>
        public static string absolutCacheIndicesFolderPath( IonConfig config )
        {
            return ApplicationData.Current.LocalFolder.Path + IonConstants.Slash + config.collectionIdentifier + IonConstants.Slash + IonConstants.CacheIndicesFolderIdentifier;
        }
    }
}
