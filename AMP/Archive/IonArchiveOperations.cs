using Anfema.Ion.Caching;
using Anfema.Ion.DataModel;
using Anfema.Ion.MediaFiles;
using Anfema.Ion.Pages;
using Anfema.Ion.Parsing;
using Anfema.Ion.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Anfema.Ion.Archive
{
    public class IonArchiveOperations : IIonArchive
    {
        private OperationLocks _fileLocks = new OperationLocks();
        IonConfig _config;


        public IonArchiveOperations( IonConfig config )
        {
            this._config = config;
        }


        /// <summary>
        /// Updates the used IonConfig
        /// </summary>
        /// <param name="config"></param>
        public void updateConfig( IonConfig config )
        {
            _config = config;
        }


        /// <summary>
        /// Used to load and extract an archive to the caches
        /// </summary>
        /// <param name="ionFiles"></param>
        /// <param name="ionPages"></param>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task loadArchiveAsync( IIonFiles ionFiles, IIonPages ionPages, string url, Action callback = null )
        {
            // Temporary used elements in the isolated storage
            StorageFile archiveFile = null;
            StorageFolder tempFolder = null;

            // Get temp-folder path
            string tempFolderPath = FilePaths.getTempFolderPath( _config );

            // Lock all used elements
            using( await _fileLocks.ObtainLock( url ).LockAsync().ConfigureAwait( false ) )
            using( await _fileLocks.ObtainLock( tempFolderPath ).LockAsync().ConfigureAwait( false ) )
            {
                try
                {
                    // Request archive file
                    archiveFile = await ionFiles.requestArchiveFileAsync( url );

                    // Get tempFolder for extraction
                    tempFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync( tempFolderPath, CreationCollisionOption.ReplaceExisting );

                    // Generate fileStream from archiveFile
                    using( Stream stream = (Stream)await archiveFile.OpenStreamForReadAsync() )
                    {
                        // Extract file to tempFolder
                        await TarUtils.ExtractTarArchiveAsync( stream, tempFolder ).ConfigureAwait( false );
                    }

                    // Get all elements listed in the index file
                    string indexFileString = await FileIO.ReadTextAsync( await tempFolder.GetFileAsync( "index.json" ) );
                    List<ArchiveElement> elementsList = JsonConvert.DeserializeObject<List<ArchiveElement>>( indexFileString );

                    // Handle each element of the index.json
                    for( int i = 0; i < elementsList.Count; i++ )
                    {
                        IonRequestInfo requestInfo = PagesURLs.analyze( elementsList[i].url, _config );

                        // Treat every element regarding its type
                        switch( requestInfo.requestType )
                        {
                            case IonRequestType.MEDIA:
                                {
                                    // Get all the needed folder- and file names
                                    StorageFolder mediaFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync( FilePaths.getMediaFolderPath( _config, false ), CreationCollisionOption.OpenIfExists );
                                    string sourcePath = tempFolder.Path + IonConstants.BackSlash + elementsList[i].name.Replace( '/', (char)IonConstants.BackSlash[0] );
                                    string destinationPath = mediaFolder.Path + IonConstants.BackSlash + FilePaths.getFileName( elementsList[i].url );

                                    // Delete a possible existing file
                                    if( File.Exists( destinationPath ) )
                                    {
                                        File.Delete( destinationPath );
                                    }

                                    // Move the file from the temp to the media folder
                                    File.Move( sourcePath, destinationPath );

                                    // Create index for file
                                    FileCacheIndex index = new FileCacheIndex( elementsList[i].url, elementsList[i].checksum, DateTimeUtils.now() );

                                    // Save file index
                                    await CacheIndexStore.save( elementsList[i].url, index, _config ).ConfigureAwait( false );

                                    break;
                                }

                            case IonRequestType.PAGE:
                                {
                                    // Extract the page json from the file
                                    string pageString = await FileIO.ReadTextAsync( await tempFolder.GetFileAsync( elementsList[i].name.Replace( '/', (char)IonConstants.BackSlash[0] ) ) );

                                    // Parse the new page
                                    IonPage page = DataParser.parsePage( pageString );

                                    // Save the page to the caches
                                    await ionPages.savePageToCachesAsync( page, _config ).ConfigureAwait( false );

                                    break;
                                }

                            default:
                                {
                                    IonLogging.log( "Object " + elementsList[i].url + " could not be parsed from archive.", IonLogMessageTypes.ERROR );
                                    break;
                                }
                        }
                    }
                }

                catch( Exception e )
                {
                    IonLogging.log( "Error at the archive download: " + e.Message, IonLogMessageTypes.ERROR );
                }

                finally
                {
                    // Clean up all temperary used elements
                    if( archiveFile != null )
                    {
                        // Delete index file
                        string indexFilePath = FilePaths.getCacheIndicesFolderPath( _config ) + IonConstants.BackSlash + archiveFile.Name + IonConstants.JsonFileExtension;

                        if( File.Exists( indexFilePath ) )
                        {
                            File.Delete( indexFilePath );
                        }

                        // Delete archive file
                        await archiveFile.DeleteAsync();
                    }

                    if( tempFolder != null )
                    {
                        await tempFolder.DeleteAsync();
                    }

                    // Call the callback action if set before
                    if( callback != null )
                    {
                        callback();
                    }
                }
            }

            // Release the file locks
            _fileLocks.ReleaseLock( url );
            _fileLocks.ReleaseLock( tempFolderPath );
        }
    }
}
