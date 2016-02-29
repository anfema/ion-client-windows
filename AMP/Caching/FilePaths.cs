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
        public static string GetFileName( string url )
        {
            return HashUtils.GetMD5Hash( url );
        }
        
        public static String GetMediaFilePath( String url, AmpConfig config )
        {
            return GetMediaFilePath( url, config, false );
        }

        public static String GetMediaFilePath( String url, AmpConfig config, bool tempCollectionFolder )
        {
            String mediaFolderPath = GetMediaFolderPath( config, tempCollectionFolder );
            String filename = GetFileName( url );
            return mediaFolderPath + filename;
        }

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
