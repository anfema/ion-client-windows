using Anfema.Amp.DataModel;
using System.Collections.Generic;

namespace Anfema.Amp.Parsing
{
    /// <summary>
    /// Class that symbolizes the root of the Amp-page-request
    /// </summary>
    public class PagesRootRaw
    {
        public Meta meta { get; set; }

        public List<AmpPageRaw> page { get; set; }
    }
}
