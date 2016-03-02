using Anfema.Amp.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.DataModel
{
    public class AmpDateTimeContent : AmpContent
    {
        public DateTime dateTime { get; set; }


        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            if ( contentNode.datetime != null )
            {
                dateTime = DateTime.ParseExact( contentNode.datetime, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture );
            }
        }
    }
}
