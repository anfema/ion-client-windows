using Newtonsoft.Json;

namespace Anfema.Ion.DataModel
{
    public class IonOptionContent : IonContent
    {
        public string value { get; set; }


        [JsonIgnore]
        public string selectedOption
        {
            get { return value; }
        }
    }
}