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
        private DateTime _dateTime;


        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            _dateTime = DateTime.ParseExact( contentNode.datetime, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture );
        }

        // Returns the stored dateTime
        public DateTime dateTime
        {
            get { return _dateTime; }
        }
    }
}
