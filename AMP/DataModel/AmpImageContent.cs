using Anfema.Amp.Parsing;
using System;

namespace Anfema.Amp.DataModel
{
    public class AmpImageContent : AmpContent
    {
        private string _mimeType;
        private string _imageURL;
        private int _width;
        private int _height;
        private int _fileSize;
        private string _originalMimeType;
        private string _originalImageURL;
        private int _originalWidth;
        private int _originalHeight;
        private int _originalFileSize;
        private int _translationX;
        private int _translationY;
        private double _scale;


        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            _mimeType = contentNode.mime_type;
            _originalMimeType = contentNode.original_mime_type;
            _imageURL = contentNode.image;
            _originalImageURL = contentNode.original_image;
            _width = contentNode.width;
            _height = contentNode.height;
            _originalWidth = contentNode.original_width;
            _originalHeight = contentNode.original_height;
            _fileSize = contentNode.file_size;
            _originalFileSize = contentNode.original_file_size;
            _scale = contentNode.scale.GetValueOrDefault(1.0);
            _translationX = contentNode.translation_x.GetValueOrDefault( 0 );
            _translationY = contentNode.translation_y.GetValueOrDefault( 0 );
        }


        public Uri imageURI
        {
            get { return new Uri(_imageURL); }
        }
    }
}
