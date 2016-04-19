using System;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;

namespace Anfema.Ion.Utils
{
    public class FileUtils
    {
        private static OperationLocks fileLocks = new OperationLocks();
        private static readonly StorageFolder _localFolder = ApplicationData.Current.LocalFolder;

        /// <summary>
        /// Write MemoryStream to file
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="targetFilePath"></param>
        /// <returns></returns>
        public static async Task<StorageFile> WriteToFile( MemoryStream inputStream, String targetFilePath )
        {
            StorageFile file = null;
            using( await fileLocks.ObtainLock( targetFilePath ).LockAsync() )
            {
                file = await _localFolder.CreateFileAsync( targetFilePath, CreationCollisionOption.ReplaceExisting );
                using( Stream outputStream = await file.OpenStreamForWriteAsync() )
                {
                    await inputStream.CopyToAsync( outputStream );
                }
            }
            fileLocks.ReleaseLock( targetFilePath );
            return file;
        }

        /// <summary>
        /// Read file into MemoryStream
        /// </summary>
        /// <param name="targetFilePath"></param>
        /// <returns></returns>
        public static async Task<StorageFile> ReadFromFile( String targetFilePath )
        {
            StorageFile file = null;
            using( await fileLocks.ObtainLock( targetFilePath ).LockAsync() )
            {
                file = await _localFolder.CreateFileAsync( targetFilePath, CreationCollisionOption.OpenIfExists );
            }
            fileLocks.ReleaseLock( targetFilePath );
            return file;
        }

        /// <summary>
        /// Write string to file
        /// </summary>
        /// <param name="text"></param>
        /// <param name="targetFilePath"></param>
        public static async void WriteTextToFile( String text, String targetFilePath )
        {
            using( await fileLocks.ObtainLock( targetFilePath ).LockAsync() )
            {
                StorageFile file = await _localFolder.CreateFileAsync( targetFilePath, CreationCollisionOption.ReplaceExisting );
                await FileIO.WriteTextAsync( file, text );
            }
            fileLocks.ReleaseLock( targetFilePath );
        }

        /// <summary>
        /// Read file into string
        /// </summary>
        /// <param name="targetFilePath"></param>
        /// <returns></returns>
        public static async Task<String> ReadTextFromFile( String targetFilePath )
        {
            String value = String.Empty;
            using( await fileLocks.ObtainLock( targetFilePath ).LockAsync() )
            {
                StorageFile file = await _localFolder.GetFileAsync( targetFilePath );
                value = await FileIO.ReadTextAsync( file );
            }
            fileLocks.ReleaseLock( targetFilePath );
            return value;
        }

        // TODO: Implement missing functions: move, reset, createDir, deleteRecursive

        /// <summary>
        /// Check if file exists
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async static Task<bool> Exists( String filePath )
        {
            bool fileExists = true;
            Stream fileStream = null;
            StorageFile file = null;

            try
            {
                //ApplicationData.Current.LocalFolder.
                file = await ApplicationData.Current.LocalFolder.GetFileAsync( filePath );
                fileStream = await file.OpenStreamForReadAsync();
                fileStream.Dispose();
            }
            catch( FileNotFoundException )
            {
                // If the file dosn't exits it throws an exception, make fiqleExists false in this case 
                fileExists = false;
            }
            finally
            {
                if( fileStream != null )
                {
                    fileStream.Dispose();
                }
            }

            return fileExists;
        }
    }
}
