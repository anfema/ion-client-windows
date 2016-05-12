using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;


namespace Anfema.Ion.Utils
{
    public class IonLogging
    {
        private static Queue<IonLogMessage> _logMessages = new Queue<IonLogMessage>();
        private static Task _writeToFileTask;
        private static StorageFolder _currentFolder = ApplicationData.Current.LocalFolder;

        /// <summary>
        /// Used to log a message
        /// </summary>
        /// <param name="logMessage"></param>
        public static void log( string message, IonLogMessageTypes messageType )
        {
            IonLogMessage logMessage = new IonLogMessage( message, messageType );
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
                // Get file or create file in the designated filePath
                StorageFile file = await _currentFolder.CreateFileAsync( IonConstants.LogFileIdentifier, CreationCollisionOption.OpenIfExists );

                // Write the whole log queue to the log-file
                while( _logMessages.Count > 0 )
                {
                    // Dequeue one messe and write it to file
                    IonLogMessage actualMessage = _logMessages.Dequeue();
                    await FileIO.AppendTextAsync( file, DateTimeUtils.now().ToString() + ": " + actualMessage.type + " - " + actualMessage.message + Environment.NewLine );

#if SHOW_LOGS
                    Debug.WriteLine( DateTimeUtils.now().ToString() + ": " + actualMessage.type + " - " + actualMessage.message );
#endif
                }
            }

            catch( Exception e )
            {
                Debug.WriteLine( "Error writing log message to file: " + e.Message );
            }
        }
    }
}