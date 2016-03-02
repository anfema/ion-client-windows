using Anfema.Amp.Parsing;
using Newtonsoft.Json;
using System;

namespace Anfema.Amp.DataModel
{
    public class AmpMediaContent : AmpContent
    {
        public string mimeType { get; set; }
        public string originalMimeType { get; set; }
        public string fileURL { get; set; }
        public string originalFileURL { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int originalWidth { get; set; }
        public int originalHeight { get; set; }
        public int fileSize { get; set; }
        public int originalFileSize { get; set; }
        public string checksum { get; set; }
        public string originalChecksum { get; set; }
        public int length { get; set; }
        public int originalLength { get; set; }


        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            // TODO: this must be done because of a bug in AMP!!
            if( contentNode.mime_type == null)
            {
                mimeType = contentNode.original_mime_type;
            }
            else
            {
                mimeType = contentNode.mime_type;
            }
            //mimeType = contentNode.mime_type;
            originalMimeType = contentNode.original_mime_type;
            fileURL = contentNode.file;
            originalFileURL = contentNode.original_file;
            width = contentNode.width;
            height = contentNode.height;
            originalWidth = contentNode.original_width;
            originalHeight = contentNode.original_height;
            fileSize = contentNode.file_size;
            originalFileSize = contentNode.original_file_size;
            checksum = contentNode.checksum;
            originalChecksum = contentNode.original_checksum;
            length = contentNode.length;
            originalLength = contentNode.original_length;
        }        

        
        // Returns the media uri   TODO: currently some failsave stuff because of a Amp error
        [JsonIgnore]
        public Uri mediaURI
        {
            get
            {
                if (fileURL == null)
                {
                    if ( originalFileURL != null )
                    {
                        return new Uri( originalFileURL );
                    }
                    else return null;
                }
                else
                {
                    return new Uri(fileURL);
                }
            }
        }
    }
}