using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Anfema.Amp.DataModel
{
    public class AmpImageContent : AmpContent
    {
        [JsonProperty("mime_type")]
        public string mimeType { get; set; }

        [JsonProperty("image")]
        public string imageURL { get; set; }

        [JsonProperty("file_size")]
        public int fileSize { get; set; }

        [JsonProperty("original_mime_type")]
        public string originalMimeType { get; set; }

        [JsonProperty("original_image")]
        public string originalImageURL { get; set; }

        [JsonProperty("original_width")]
        public int originalWidth { get; set; }

        [JsonProperty("original_height")]
        public int originalHeight { get; set; }

        [JsonProperty("original_file_size")]
        public int originalFileSize { get; set; }

        [JsonProperty("translation_x")]
        public int translationX { get; set; }

        [JsonProperty("translation_y")]
        public int translationY { get; set; }

        [JsonProperty("original_checksum")]
        public string originalChecksum { get; set; }

        public int width { get; set; }
        public int height { get; set; }
        public double scale { get; set; }

        public BitmapImage bitmap { get; set; }
        

        public async Task<bool> createBitmap( Amp amp )
        {
            bitmap = new BitmapImage();
            bitmap.DecodePixelWidth = this.width;
            bitmap.DecodePixelHeight = this.height;
            if ( this.imageURL == null )
            {
                return false;
            }
            using ( MemoryStream data = await amp.Request( this.imageURL, null ) )
            {
                await bitmap.SetSourceAsync( data.AsRandomAccessStream() );
            }
            return true;
        }
    }
}
