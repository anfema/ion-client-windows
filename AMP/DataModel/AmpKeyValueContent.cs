using Anfema.Amp.Parsing;
using System.Collections.Generic;

namespace Anfema.Amp.DataModel
{
    public class AmpKeyValueContent : AmpContent
    {
        private List<KeyValueItem> _keyValueList = new List<KeyValueItem>();
        
        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            // Generate the keyValueItems for the given array and try to determine their value type
            for (int i = 0; i < contentNode.values.Count; i++ )
            {
                _keyValueList.Add(new KeyValueItem(contentNode.values[i]));
            }
        }
        
        public List<KeyValueItem> items
        {
            get { return _keyValueList; }
        }
    }
}