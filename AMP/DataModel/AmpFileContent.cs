using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;

namespace Anfema.Amp.DataModel
{
    public class AmpFileContent : AmpContent
    {
        [JsonProperty("mime_type")]
        public string mimeType { get; set; }

        public string name { get; set; }

        [JsonProperty("file_size")]
        public int fileSize { get; set; }

        public string checksum { get; set; }

        [JsonProperty("file")]
        public string fileURL { get; set; }

        public StorageFile storageFile { get; set; }

        public async Task loadFile(Amp amp)
        {
            this.storageFile = await amp.Request(this.fileURL, null);
        }
    }
}
