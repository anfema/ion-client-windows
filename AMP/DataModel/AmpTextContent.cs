using Anfema.Amp.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.DataModel
{
    public class AmpTextContent : AmpContent
    {
        private string _mimeType;
        private bool _multiLine;
        private string _text;

        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            _mimeType = contentNode.mime_type;
            _multiLine = contentNode.is_multiline;
            _text = contentNode.text;
        }


        public string text
        {
            get { return _text; }
        }


        public bool isMultiLine
        {
            get { return _multiLine; }
        }
    }
}
