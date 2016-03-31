using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace Anfema.Amp.DataModel
{
    public class AmpFileContent : AmpContent, INotifyPropertyChanged
    {
        [JsonProperty("mime_type")]
        public string mimeType { get; set; }

        public string name { get; set; }

        [JsonProperty("file_size")]
        public int fileSize { get; set; }

        public string checksum { get; set; }

        [JsonProperty("file")]
        public string fileURL { get; set; }


        [JsonIgnore]
        private object _fileContent;

        [JsonIgnore]
        private bool _fileFetched = false;

        // Declare the PropertyChanged event.
        public event PropertyChangedEventHandler PropertyChanged;


        [JsonIgnore]
        public object file
        {
            get
            {
                // Get the file from the server, if it isn't loaded yet
                if (!_fileFetched)
                {
                    // Get the file with NO await. The file property will be automatically be updated with the propertyChenged event
                    if ( this.fileURL != null )
                    {
                        getFileFromServer();
                    }
                }
                return _fileContent;
            }
        }


        // Gets the file from the given adress
        private async Task getFileFromServer()
        {
            // Generate new httpClient to retrieve the file
            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync( new Uri( fileURL ) ).ConfigureAwait(false);

            // Handle different file types
            switch(mimeType)
            {
                case "text/plain":
                    {
                        _fileContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        break;
                    }
                default:
                    {
                        _fileContent = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                        break;
                    }
            }

            // mark the file as fetched
            _fileFetched = true;

            // Send the event to refresh the file property
            NotifyPropertyChanged("file");
        }


        // Notify the property, that the file was changed/loaded
        public void NotifyPropertyChanged( string propertyName )
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
