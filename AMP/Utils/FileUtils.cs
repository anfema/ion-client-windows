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
        private static readonly StorageFolder _localFolder = ApplicationData.Current.LocalFolder;
        public static readonly String SLASH = "\\";
        
        private static readonly object syncLock = new object();

        class LockWithCounter
        {
            public int counter { get; set; } = 0;
            public AsyncLock asyncLock { get; } = new AsyncLock();
        }

        private static Dictionary<String, LockWithCounter> ongoingWriteOperations = new Dictionary<string, LockWithCounter>();

        private static void ReleaseLock( String filePath )
        {
            lock ( syncLock )
            {
                LockWithCounter lockWithCounter = null;
                if ( ongoingWriteOperations.TryGetValue( filePath, out lockWithCounter ) )
                {
                    lockWithCounter.counter--;

                    if ( lockWithCounter.counter <= 0 )
                    {
                        ongoingWriteOperations.Remove( filePath );
                    }
                }
            }
        }

        private static AsyncLock ObtainLock( String filePath )
        {
            lock ( syncLock )
            {
                LockWithCounter lockWithCounter = null;
                if ( !ongoingWriteOperations.TryGetValue( filePath, out lockWithCounter ) )
                {
                    lockWithCounter = new LockWithCounter();
                    ongoingWriteOperations.Add( filePath, lockWithCounter );
                }
                lockWithCounter.counter++;
                return lockWithCounter.asyncLock;
            }
        }

        public static async Task<bool> WriteToFile( MemoryStream inputStream, String targetFilePath )
        {
            using ( await ObtainLock( targetFilePath ).LockAsync() )
            {
                StorageFile file = await _localFolder.CreateFileAsync(targetFilePath, CreationCollisionOption.ReplaceExisting);
                using ( Stream outputStream = await file.OpenStreamForWriteAsync() )
                {
                    await inputStream.CopyToAsync( outputStream );
                }
            }
            ReleaseLock( targetFilePath );
            return true;
        }

        public static async Task<MemoryStream> ReadFromFile( String targetFilePath )
        {
            MemoryStream outputStream = new MemoryStream();
            using ( await ObtainLock( targetFilePath ).LockAsync() )
            {
                StorageFile file = await _localFolder.CreateFileAsync(targetFilePath, CreationCollisionOption.OpenIfExists);
                using ( Stream inputStream = await file.OpenStreamForReadAsync() )
                {
                    await inputStream.CopyToAsync( outputStream );
                    outputStream.Position = 0;
                }
            }
            ReleaseLock( targetFilePath );
            return outputStream;
        }

        public static async void WriteTextToFile( String text, String targetFilePath )
        {
            using ( await ObtainLock( targetFilePath ).LockAsync() )
            {
                StorageFile file = await _localFolder.CreateFileAsync(targetFilePath, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync( file, text );
            }
            ReleaseLock( targetFilePath );
        }

        public static async Task<String> ReadTextFromFile( String targetFilePath )
        {
            String value = String.Empty;
            using ( await ObtainLock( targetFilePath ).LockAsync() )
            {
                StorageFile file = await _localFolder.GetFileAsync( targetFilePath );
                value = await FileIO.ReadTextAsync( file );
            }
            ReleaseLock( targetFilePath );
            return value;
        }

        // TODO: Implement missing functions: move, reset, createDir, deleteRecursive

        public async static Task<bool> Exists( String filePath )
        {
            bool fileExists = true;
            Stream fileStream = null;
            StorageFile file = null;

            try
            {
                file = await ApplicationData.Current.LocalFolder.GetFileAsync( filePath );
                fileStream = await file.OpenStreamForReadAsync();
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
