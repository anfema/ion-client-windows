using Anfema.Ion.DataModel;
using Anfema.Ion.Utils;
using System;


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


        /// <summary>
        /// Retrieves the path to the archive file for a given url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="config"></param>
        /// <param name="tempCollectionFolder"></param>
        /// <returns>File path</returns>
        public static string getArchiveFilePath( string url, IonConfig config, bool tempCollectionFolder = false )
        {
            string archiveFolderPath = getArchiveFolderPath( config, tempCollectionFolder );
            string filename = getFileName( url );
            return archiveFolderPath + filename;
        }
        

        public static String GetFtsDbFilePath( String collectionIdentifier )
        {
            return IonConstants.FtsDbFolderIdentifier + IonConstants.BackSlash + "fts_" + collectionIdentifier + ".sqlite";
        }


        /// <summary>
        /// Used to get the folder path for the temp folder
        /// </summary>
        /// <param name="config"></param>
        /// <returns>File path</returns>
        public static string getTempFolderPath( IonConfig config )
        {
            return getBasicCollectionFolderPath( config ) + IonConstants.TempFolderIdentifier + IonConstants.BackSlash;
        }


        /// <summary>
        /// Returns the folder to which the archives are downloaded
        /// </summary>
        /// <param name="url"></param>
        /// <param name="config"></param>
        /// <returns>Archive folder</returns>
        public static string getArchiveFolderPath( IonConfig config, bool tempCollectionFolder )
        {
            return getCollectionFolderPath( config ) + IonConstants.ArchiveFolderIdentifier + IonConstants.BackSlash;
        }


        /// <summary>
        /// Get local file cache folder path for an Ion configuration/collection
        /// </summary>
        /// <param name="config"></param>
        /// <param name="tempCollectionFolder"></param>
        /// <returns></returns>
        public static String getMediaFolderPath( IonConfig config, bool tempCollectionFolder )
        {
            return getBasicCollectionFolderPath( config ) + IonConstants.MediaFolderIdentifier + IonConstants.BackSlash;
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
        public static string getCacheIndicesFolderPath( IonConfig config )
        {
            return getCollectionFolderPath( config ) + IonConstants.CacheIndicesFolderIdentifier + IonConstants.BackSlash;
        }


        /// <summary>
        /// Used to get the basic collection folder path without any deeper hierarchy
        /// </summary>
        /// <param name="config"></param>
        /// <returns>FilePath</returns>
        public static string getBasicCollectionFolderPath( IonConfig config )
        {
            return config.collectionIdentifier + IonConstants.BackSlash;
        }


        /// <summary>
        /// Used to get the folder path for this IonConfig including hierarchy info for locale and variation
        /// </summary>
        /// <param name="config"></param>
        /// <returns>FilePath including locale and variation</returns>
        public static string getCollectionFolderPath( IonConfig config )
        {
            return getBasicCollectionFolderPath( config ) + config.locale + IonConstants.BackSlash + config.variation + IonConstants.BackSlash;
        }


        /// <summary>
        /// Used to get the folder path for this configs pages
        /// </summary>
        /// <param name="config"></param>
        /// <returns>FilePath to the pages of this config</returns>
        public static string getPagesFolderPath( IonConfig config )
        {
            return getCollectionFolderPath( config ) + IonConstants.PageFolderIdentifier + IonConstants.BackSlash;
        }
    }
}
