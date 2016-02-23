using Anfema.Amp.Parsing;
using Newtonsoft.Json;

namespace Anfema.Amp.DataModel
{
    public class AmpOptionContent : AmpContent
    {
        public string value { get; set; }

        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            value = contentNode.value;
        }
        

        [JsonIgnore]
        public string selectedOption
        {
            get { return value; }
        }
    }
}