using System.Collections.ObjectModel;


namespace Anfema.Ion.DataModel
{
    // Class for all the data content parsed from the server
    public class IonPageObservableCollection
    {
        public ObservableCollection<IonColorContent> colorContent { get; set; }
        public ObservableCollection<IonDateTimeContent> dateTimeContent { get; set; }
        public ObservableCollection<IonFileContent> fileContent { get; set; }
        public ObservableCollection<IonFlagContent> flagContent { get; set; }
        public ObservableCollection<IonImageContent> imageContent { get; set; }
        public ObservableCollection<IonMediaContent> mediaContent { get; set; }
        public ObservableCollection<IonOptionContent> optionContent { get; set; }
        public ObservableCollection<IonTextContent> textContent { get; set; }
        public ObservableCollection<IonConnectionContent> connectionContent { get; set; }
        public ObservableCollection<IonNumberContent> numberContent { get; set; }
        public ObservableCollection<IonChartContent> chartContent { get; set; }


        public IonPageObservableCollection()
        {
            colorContent = new ObservableCollection<IonColorContent>();
            dateTimeContent = new ObservableCollection<IonDateTimeContent>();
            fileContent = new ObservableCollection<IonFileContent>();
            flagContent = new ObservableCollection<IonFlagContent>();
            imageContent = new ObservableCollection<IonImageContent>();
            mediaContent = new ObservableCollection<IonMediaContent>();
            optionContent = new ObservableCollection<IonOptionContent>();
            textContent = new ObservableCollection<IonTextContent>();
            connectionContent = new ObservableCollection<IonConnectionContent>();
            numberContent = new ObservableCollection<IonNumberContent>();
            chartContent = new ObservableCollection<IonChartContent>();
        }
    }
}
