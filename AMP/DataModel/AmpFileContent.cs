using Anfema.Amp.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.DataModel
{
    public class AmpFileContent : AmpContent, INotifyPropertyChanged
    {
        private string _mimeType;
        private string _name;
        private int _file_size;
        private string _checksum;
        private string _fileURL;


        private object _fileContent;

        private bool _fileFetched = false;

        // Declare the PropertyChanged event.
        public event PropertyChangedEventHandler PropertyChanged;


        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            _mimeType = contentNode.mime_type;
            _name = contentNode.name;
            _file_size = contentNode.file_size;
            _checksum = contentNode.checksum;
            _fileURL = contentNode.file;
        }


        public string fileURL
        {
            get { return _fileURL; }
        }

        public string mimeType
        {
            get { return _mimeType; }
        }

        public string name
        {
            get { return _name; }
        }

        public string checksum
        {
            get { return _checksum; }
        }

        public int fileSize
        {
            get { return _file_size; }
        }

        public object file
        {
            get
            {
                // Get the file from the server, if it isn't loaded yet
                if (!_fileFetched)
                {
                    // Get the file with NO await. The file property will be automatically be updated with the propertyChenged event
                    getFileFromServer();
                }
                return _fileContent;
            }
        }


        // Gets the file from the given adress
        private async Task getFileFromServer()
        {
            // Generate new httpClient to retrieve the file
            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync( new Uri( _fileURL ) );

            // Handle different file types
            switch(_mimeType)
            {
                case "text/plain":
                    {
                        _fileContent = await response.Content.ReadAsStringAsync();
                        break;
                    }
                default:
                    {
                        _fileContent = await response.Content.ReadAsByteArrayAsync();
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
