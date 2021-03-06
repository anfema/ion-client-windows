﻿using SharpCompress.Reader.Tar;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Anfema.Ion.Utils
{
    public class TarUtils
    {
        public async static Task ExtractTarArchiveAsync( Stream archiveStream, StorageFolder destinationFolder )
        {
            using( TarReader reader = TarReader.Open( archiveStream ) )
            {
                while( reader.MoveToNextEntry() )
                {
                    if( !reader.Entry.IsDirectory )
                    {
                        using( var entryStream = reader.OpenEntryStream() )
                        {
                            string fileName = Path.GetFileName( reader.Entry.Key );
                            string folderName = Path.GetDirectoryName( reader.Entry.Key );

                            StorageFolder folder = destinationFolder;
                            if( string.IsNullOrWhiteSpace( folderName ) == false )
                            {
                                folder = await destinationFolder.CreateFolderAsync( folderName, CreationCollisionOption.OpenIfExists );
                            }

                            StorageFile file = await folder.CreateFileAsync( fileName, CreationCollisionOption.OpenIfExists );
                            using( Stream fileStream = await file.OpenStreamForWriteAsync().ConfigureAwait( false ) )
                            {
                                await entryStream.CopyToAsync( fileStream );
                            }
                        }
                    }
                }
            }
        }
    }
}