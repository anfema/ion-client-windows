using Anfema.Amp.DataModel;
using Anfema.Amp.mediafiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace AMP_Test
{
    public class ImageDataProvider
    {
        AmpFilesWithCaching _ampFilesWithCaching;

        public ImageDataProvider( AmpFilesWithCaching ampFilesWithCaching )
        {
            this._ampFilesWithCaching = ampFilesWithCaching;
        }

        public async Task<BitmapImage> getBitmap( AmpImageContent ampImageContent )
        {
            return await ampImageContent.getBitmap( _ampFilesWithCaching );
        }
    }
}
