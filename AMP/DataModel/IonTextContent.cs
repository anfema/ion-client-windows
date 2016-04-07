using Newtonsoft.Json;


namespace Anfema.Ion.DataModel
{
    public class IonTextContent : IonContent
    {
        [JsonProperty( "mime_type" )]
        public string mimeType { get; set; }

        [JsonProperty( "is_multiline" )]
        public bool multiLine { get; set; }

        public string text { get; set; }


        [JsonIgnore]
        public bool isMultiLine
        {
            get { return multiLine; }
        }
    }
}
