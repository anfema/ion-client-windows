using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;

namespace Anfema.Amp.Utils
{
    public class FileUtils
    {
        private static OperationLocks fileLocks = new OperationLocks();
        private static readonly StorageFolder _localFolder = ApplicationData.Current.LocalFolder;
        public static readonly String SLASH = "\\";
        
        /// <summary>
        /// Write MemoryStream to file
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="targetFilePath"></param>
        /// <returns></returns>
        public static async Task<bool> WriteToFile( MemoryStream inputStream, String targetFilePath )
        {
            using ( await fileLocks.ObtainLock( targetFilePath ).LockAsync().ConfigureAwait(false))
            {
                StorageFile file = await _localFolder.CreateFileAsync(targetFilePath, CreationCollisionOption.ReplaceExisting);
                using ( Stream outputStream = await file.OpenStreamForWriteAsync().ConfigureAwait(false))
                {
                    await inputStream.CopyToAsync( outputStream ).ConfigureAwait(false);
                }
            }
            fileLocks.ReleaseLock( targetFilePath );
            return true;
        }
        
        /// <summary>
        /// Read file into MemoryStream
        /// </summary>
        /// <param name="targetFilePath"></param>
        /// <returns></returns>
        public static async Task<MemoryStream> ReadFromFile( String targetFilePath )
        {
            MemoryStream outputStream = new MemoryStream();
            using ( await fileLocks.ObtainLock( targetFilePath ).LockAsync().ConfigureAwait(false))
            {
                StorageFile file = await _localFolder.CreateFileAsync(targetFilePath, CreationCollisionOption.OpenIfExists);
                using ( Stream inputStream = await file.OpenStreamForReadAsync().ConfigureAwait(false))
                {
                    await inputStream.CopyToAsync( outputStream ).ConfigureAwait(false);
                    outputStream.Position = 0;
                }
            }
            fileLocks.ReleaseLock( targetFilePath );
            return outputStream;
        }

        /// <summary>
        /// Write string to file
        /// </summary>
        /// <param name="text"></param>
        /// <param name="targetFilePath"></param>
        public static async void WriteTextToFile( String text, String targetFilePath )
        {
            using ( await fileLocks.ObtainLock( targetFilePath ).LockAsync().ConfigureAwait(false))
            {
                StorageFile file = await _localFolder.CreateFileAsync(targetFilePath, CreationCollisionOption.ReplaceExisting);
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
            using ( await fileLocks.ObtainLock( targetFilePath ).LockAsync().ConfigureAwait(false))
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
                file = await ApplicationData.Current.LocalFolder.GetFileAsync( filePath );
                fileStream = await file.OpenStreamForReadAsync().ConfigureAwait(false);
                fileStream.Dispose();
            }
            catch ( FileNotFoundException )
            {
                // If the file dosn't exits it throws an exception, make fileExists false in this case 
                fileExists = false;
            }
            finally
            {
                if ( fileStream != null )
                {
                    fileStream.Dispose();
                }
            }

            return fileExists;
        }
    }
}
