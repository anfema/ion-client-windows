using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;


namespace Anfema.Ion.Utils
{
    public class IonLogHelper
    {
        private static Queue<string> _logMessages = new Queue<string>();
        private static Task _writeToFileTask;
        private static StorageFolder _currentFolder = ApplicationData.Current.LocalFolder;

        /// <summary>
        /// Used to log a message
        /// </summary>
        /// <param name="logMessage"></param>
        public static void log( string logMessage )
        {
            // Add log message to queue
            _logMessages.Enqueue( logMessage );

            // Start task to save the queue to file
            if( _writeToFileTask == null || _writeToFileTask.IsCompleted )
            {
                _writeToFileTask = Task.Factory.StartNew( saveToFile );
            }
        }


        /// <summary>
        /// Saves the complete queue to file
        /// </summary>
        private static async Task saveToFile()
        {
            try
            {
                // Create filePath from compontents
                StorageFile file = await _currentFolder.CreateFileAsync( IonConstants.LogFileIdentifier, CreationCollisionOption.OpenIfExists );

                // Write the whole log queue to the log-file
                while( _logMessages.Count > 0 )
                {
                    await FileIO.AppendTextAsync( file, DateTimeUtils.now().ToString() + ": " + _logMessages.Dequeue() + Environment.NewLine );
                }
            }

            catch( Exception e )
            {
                Debug.WriteLine( "Error writing log message to file: " + e.Message );
            }
        }
    }
}
