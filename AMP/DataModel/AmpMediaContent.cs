using Newtonsoft.Json;
using System;

namespace Anfema.Amp.DataModel
{
    public class AmpMediaContent : AmpContent
    {
        [JsonProperty("mime_type")]
        public string mimeType { get; set; }

        [JsonProperty("original_mime_type")]
        public string originalMimeType { get; set; }

        [JsonProperty("file")]
        public string fileURL { get; set; }

        [JsonProperty("original_file")]
        public string originalFileURL { get; set; }

        [JsonProperty("original_width")]
        public int originalWidth { get; set; }

        [JsonProperty("original_height")]
        public int originalHeight { get; set; }

        [JsonProperty("file_size")]
        public int fileSize { get; set; }

        [JsonProperty("original_file_size")]
        public int originalFileSize { get; set; }

        [JsonProperty("original_checksum")]
        public string originalChecksum { get; set; }

        [JsonProperty("original_length")]
        public int originalLength { get; set; }

        public int width { get; set; }
        public int height { get; set; }
        public string checksum { get; set; }
        public int length { get; set; }
        
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