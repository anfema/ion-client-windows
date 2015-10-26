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
        private bool _enabled;


        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            _enabled = contentNode.is_enabled.GetValueOrDefault( false ); // gets the value or false, if the value isn't set
        }


        public bool enabled
        {
            get { return _enabled; }
        }
    }
}
