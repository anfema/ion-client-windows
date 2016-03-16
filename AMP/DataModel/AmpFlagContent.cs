using Newtonsoft.Json;


namespace Anfema.Amp.DataModel
{
    public class AmpFlagContent : AmpContent
    {
        [JsonProperty("is_enabled")]
        public bool enabled { get; set; }
    }
}
