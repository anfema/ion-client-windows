using Anfema.Amp.mediafiles;
using Anfema.Amp.Parsing;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Anfema.Amp.DataModel
{
    public class AmpImageContent : AmpContent
    {
        public string mimeType { get; set; }
        public string imageURL { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int fileSize { get; set; }
        public string originalMimeType { get; set; }
        public string originalImageURL { get; set; }
        public int originalWidth { get; set; }
        public int originalHeight { get; set; }
        public int originalFileSize { get; set; }
        public int translationX { get; set; }
        public int translationY { get; set; }
        public double scale { get; set; }

        public BitmapImage bitmap { get; set; }
        
        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            mimeType = contentNode.mime_type;
            originalMimeType = contentNode.original_mime_type;
            imageURL = contentNode.image;
            originalImageURL = contentNode.original_image;
            width = contentNode.width;
            height = contentNode.height;
            originalWidth = contentNode.original_width;
            originalHeight = contentNode.original_height;
            fileSize = contentNode.file_size;
            originalFileSize = contentNode.original_file_size;
            scale = contentNode.scale.GetValueOrDefault(1.0);
            translationX = contentNode.translation_x.GetValueOrDefault( 0 );
            translationY = contentNode.translation_y.GetValueOrDefault( 0 );
        }

        public async Task<bool> createBitmap( AmpFilesWithCaching ampFilesWithCaching )
        {
            bitmap = new BitmapImage();
            bitmap.DecodePixelWidth = this.width;
            bitmap.DecodePixelHeight = this.height;
            if ( this.imageURL == null )
            {
                return false;
            }
            using ( MemoryStream data = await ampFilesWithCaching.Request( this.imageURL, null ) )
            {
                await bitmap.SetSourceAsync( data.AsRandomAccessStream() );
            }
            return true;
        }
    }
}
