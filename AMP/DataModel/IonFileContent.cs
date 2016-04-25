using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;

namespace Anfema.Ion.DataModel
{
    public class IonFileContent : IonContent
    {
        [JsonProperty( "mime_type" )]
        public string mimeType { get; set; }

        public string name { get; set; }

        [JsonProperty( "file_size" )]
        public int fileSize { get; set; }

        public string checksum { get; set; }

        [JsonProperty( "file" )]
        public string fileURL { get; set; }

        public StorageFile storageFile { get; set; }




        public async Task loadFile( Ion amp )
        {
            this.storageFile = await amp.Request( this.fileURL, checksum, this );
        }


        /// <summary>
        /// Checks for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals( object obj )
        {
            // Basic IonContent equality check
            if( !base.Equals( obj ) )
            {
                return false;
            }

            try
            {
                // Try to cast
                IonFileContent content = (IonFileContent)obj;

                return mimeType.Equals( content.mimeType )
                    && name.Equals( content.name )
                    && fileSize == content.fileSize
                    && checksum.Equals( content.checksum )
                    && fileURL.Equals( content.fileURL )
                    && storageFile.Equals( content.storageFile );
            }

            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Returns the exact hashCode that the base class would do
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
