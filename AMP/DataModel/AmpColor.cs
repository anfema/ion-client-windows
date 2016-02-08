using Anfema.Amp.Parsing;
using Newtonsoft.Json;
using Windows.UI;

namespace Anfema.Amp.DataModel
{
    public class AmpColorContent : AmpContent
    {
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
        public int a { get; set; }

        public override void init(ContentNodeRaw contentNode)
        {
            // Init the basic parameters
            base.init(contentNode);
            
            r = contentNode.r.GetValueOrDefault(0);
            g = contentNode.g.GetValueOrDefault(0);
            b = contentNode.b.GetValueOrDefault(0);
            a = contentNode.a.GetValueOrDefault(255); // Set fully opaque if the alpha value is missing
        }

        // Return a color object from the stored values
        [JsonIgnore]
        public Color color
        {
            get { return new Color { R = (byte)r, G = (byte)g, B = (byte)b, A = (byte)a }; }
        }
    }
}
