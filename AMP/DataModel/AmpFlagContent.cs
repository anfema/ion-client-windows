using Anfema.Amp.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.DataModel
{
    public class AmpFlagContent : AmpContent
    {
        public bool enabled { get; set; }

        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            enabled = contentNode.is_enabled.GetValueOrDefault( false ); // gets the value or false, if the value isn't set
        }
    }
}
