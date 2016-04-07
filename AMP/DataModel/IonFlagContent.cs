using Newtonsoft.Json;


namespace Anfema.Ion.DataModel
{
    public class IonFlagContent : IonContent
    {
        [JsonProperty( "is_enabled" )]
        public bool enabled { get; set; }
    }
}
