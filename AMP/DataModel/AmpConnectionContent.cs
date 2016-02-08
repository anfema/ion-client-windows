using Anfema.Amp.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.DataModel
{
    public class AmpConnectionContent : AmpContent
    {
        public string connection_string { get; set; }


        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            connection_string = contentNode.connection_string;
        }
    }
}
