using Anfema.Amp.Parsing;
using System;

namespace Anfema.Amp.DataModel
{
    public class AmpMediaContent : AmpContent
    {
        private string _mimeType;
        private string _originalMimeType;
        private string _fileURL;
        private string _originalFileURL;
        private int _width;
        private int _height;
        private int _originalWidth;
        private int _originalHeight;
        private int _fileSize;
        private int _originalFileSize;
        private string _checksum;
        private string _originalChecksum;
        private int _lenght;
        private int _originalLength;
        
        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            _mimeType = contentNode.mime_type;
            _originalMimeType = contentNode.original_mime_type;
            _fileURL = contentNode.file;
            _originalFileURL = contentNode.original_file;
            _width = contentNode.width;
            _height = contentNode.height;
            _originalWidth = contentNode.original_width;
            _originalHeight = contentNode.original_height;
            _fileSize = contentNode.file_size;
            _originalFileSize = contentNode.original_file_size;
            _checksum = contentNode.checksum;
            _originalChecksum = contentNode.original_checksum;
            _lenght = contentNode.length;
            _originalLength = contentNode.original_length;
        }
        
        // Returns the mime type   TODO: currently some failsave stuff because of a Amp error
        public string mimeType
        {
            get
            {
                if (_mimeType == null)
                {
                    return _originalMimeType;
                }
                else
                {
                    return _mimeType;
                }
            }
        }
        
        // Returns the media uri   TODO: currently some failsave stuff because of a Amp error
        public Uri mediaURI
        {
            get
            {
                if (_fileURL == null)
                {
                    return new Uri(_originalFileURL);
                }
                else
                {
                    return new Uri(_fileURL);
                }
            }
        }
    }
}