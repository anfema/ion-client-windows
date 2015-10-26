using Anfema.Amp.Parsing;
using Windows.UI;

namespace Anfema.Amp.DataModel
{
    public class AmpColorContent : AmpContent
    {
        private int _r;
        private int _g;
        private int _b;
        private int _a;

        public override void init(ContentNodeRaw contentNode)
        {
            // Init the basic parameters
            base.init(contentNode);
            
            _r = contentNode.r.GetValueOrDefault(0);
            _g = contentNode.g.GetValueOrDefault(0);
            _b = contentNode.b.GetValueOrDefault(0);
            _a = contentNode.a.GetValueOrDefault(255); // Set fully opaque if the alpha value is missing
        }

        // Return a color object from the stored values
        public Color color
        {
            get { return new Color { R = (byte)_r, G = (byte)_g, B = (byte)_b, A = (byte)_a }; }
        }
    }
}
