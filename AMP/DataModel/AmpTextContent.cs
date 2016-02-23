using Anfema.Amp.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.DataModel
{
    public class AmpTextContent : AmpContent
    {
        public string mimeType { get; set; }
        public bool multiLine { get; set; }
        public string text { get; set; }


        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            mimeType = contentNode.mime_type;
            multiLine = contentNode.is_multiline;
            text = contentNode.text;
        }


        [JsonIgnore]
        public bool isMultiLine
        {
            get { return multiLine; }
        }
    }
}
