using Anfema.Amp.Parsing;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Anfema.Amp.DataModel
{
    public class AmpKeyValueContent : AmpContent
    {
        public List<KeyValueItem> keyValueList { get; set; } = new List<KeyValueItem>();

        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            // Generate the keyValueItems for the given array and try to determine their value type
            for (int i = 0; i < contentNode.values.Count; i++ )
            {
                keyValueList.Add(new KeyValueItem(contentNode.values[i]));
            }
        }
        
        [JsonIgnore]
        public List<KeyValueItem> items
        {
            get { return keyValueList; }
        }
    }
}