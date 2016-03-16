using Newtonsoft.Json;

namespace Anfema.Amp.DataModel
{
    public class AmpOptionContent : AmpContent
    {
        public string value { get; set; }


        [JsonIgnore]
        public string selectedOption
        {
            get { return value; }
        }
    }
}