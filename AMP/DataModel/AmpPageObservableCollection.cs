using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.DataModel
{
    // Class for all the data content parsed from the server
    public class AmpPageObservableCollection
    {
        public ObservableCollection<AmpColorContent> colorContent { get; set; }
        public ObservableCollection<AmpDateTimeContent> dateTimeContent { get; set; }
        public ObservableCollection<AmpFileContent> fileContent { get; set; }
        public ObservableCollection<AmpFlagContent> flagContent { get; set; }
        public ObservableCollection<AmpImageContent> imageContent { get; set; }
        public ObservableCollection<AmpMediaContent> mediaContent { get; set; }
        public ObservableCollection<AmpOptionContent> optionContent { get; set; }
        public ObservableCollection<AmpTextContent> textContent { get; set; }
        public ObservableCollection<AmpConnectionContent> connectionContent { get; set; }


        public AmpPageObservableCollection()
        {
            colorContent = new ObservableCollection<AmpColorContent>();
            dateTimeContent = new ObservableCollection<AmpDateTimeContent>();
            fileContent = new ObservableCollection<AmpFileContent>();
            flagContent = new ObservableCollection<AmpFlagContent>();
            imageContent = new ObservableCollection<AmpImageContent>();
            mediaContent = new ObservableCollection<AmpMediaContent>();
            optionContent = new ObservableCollection<AmpOptionContent>();
            textContent = new ObservableCollection<AmpTextContent>();
            connectionContent = new ObservableCollection<AmpConnectionContent>();
        }
    }
}
