﻿using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Storage;


namespace Anfema.Ion.DataModel
{
    public class IonImageContent : IonContent
    {
        [JsonProperty( "mime_type" )]
        public string mimeType { get; set; }

        [JsonProperty( "image" )]
        public string imageURL { get; set; }

        [JsonProperty( "file_size" )]
        public int fileSize { get; set; }

        [JsonProperty( "original_mime_type" )]
        public string originalMimeType { get; set; }

        [JsonProperty( "original_image" )]
        public string originalImageURL { get; set; }

        [JsonProperty( "original_width" )]
        public int originalWidth { get; set; }

        [JsonProperty( "original_height" )]
        public int originalHeight { get; set; }

        [JsonProperty( "original_file_size" )]
        public int originalFileSize { get; set; }

        [JsonProperty( "translation_x" )]
        public int translationX { get; set; }

        [JsonProperty( "translation_y" )]
        public int translationY { get; set; }

        [JsonProperty( "original_checksum" )]
        public string originalChecksum { get; set; }

        public int width { get; set; }
        public int height { get; set; }
        public double scale { get; set; }
        public string checksum { get; set; }

        public StorageFile storageFile { get; set; }


        public async Task loadImage( Ion amp )
        {
            this.storageFile = await amp.Request( this.imageURL, checksum, this );
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
                IonImageContent content = (IonImageContent)obj;

                // Compare all elements
                return mimeType.Equals( content.mimeType )
                    && imageURL.Equals( content.imageURL )
                    && fileSize == content.fileSize
                    && originalMimeType.Equals( content.originalMimeType )
                    && originalImageURL.Equals( content.originalImageURL )
                    && originalWidth == content.originalWidth
                    && originalHeight == content.originalHeight
                    && originalFileSize == content.originalFileSize
                    && translationX == content.translationX
                    && translationY == content.translationY
                    && originalChecksum.Equals( content.originalChecksum )
                    && width == content.width
                    && height == content.height
                    && scale == content.scale
                    && checksum.Equals( content.checksum )
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
