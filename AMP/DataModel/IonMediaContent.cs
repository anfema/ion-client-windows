using Newtonsoft.Json;
using System;

namespace Anfema.Ion.DataModel
{
    public class IonMediaContent : IonContent
    {
        [JsonProperty( "mime_type" )]
        public string mimeType { get; set; }

        [JsonProperty( "original_mime_type" )]
        public string originalMimeType { get; set; }

        [JsonProperty( "file" )]
        public string fileURL { get; set; }

        [JsonProperty( "original_file" )]
        public string originalFileURL { get; set; }

        [JsonProperty( "original_width" )]
        public int originalWidth { get; set; }

        [JsonProperty( "original_height" )]
        public int originalHeight { get; set; }

        [JsonProperty( "file_size" )]
        public int fileSize { get; set; }

        [JsonProperty( "original_file_size" )]
        public int originalFileSize { get; set; }

        [JsonProperty( "original_checksum" )]
        public string originalChecksum { get; set; }

        [JsonProperty( "original_length" )]
        public int originalLength { get; set; }

        public int width { get; set; }
        public int height { get; set; }
        public string checksum { get; set; }
        public int length { get; set; }



        // Returns the media uri   TODO: currently some failsave stuff because of a Ion error
        [JsonIgnore]
        public Uri mediaURI
        {
            get
            {
                if( fileURL == null )
                {
                    if( originalFileURL != null )
                    {
                        return new Uri( originalFileURL );
                    }
                    else return null;
                }
                else
                {
                    return new Uri( fileURL );
                }
            }
        }


        /// <summary>
        /// Checks for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if all elements are equal, false otherwise</returns>
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
                IonMediaContent content = (IonMediaContent)obj;

                // Check each parameter for equality
                return mimeType.Equals( content.mimeType )
                    && originalMimeType.Equals( content.originalMimeType )
                    && fileURL.Equals( content.fileURL )
                    && originalFileURL.Equals( content.originalFileURL )
                    && originalWidth == content.originalWidth
                    && originalHeight == content.originalHeight
                    && fileSize == content.fileSize
                    && originalFileSize == content.originalFileSize
                    && originalChecksum.Equals( content.originalChecksum )
                    && originalLength == content.originalLength
                    && width == content.width
                    && height == content.height
                    && checksum.Equals( content.checksum )
                    && length == content.length;
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