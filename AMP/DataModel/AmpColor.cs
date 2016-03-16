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


        // Return a color object from the stored values
        [JsonIgnore]
        public Color color
        {
            get { return new Color { R = (byte)r, G = (byte)g, B = (byte)b, A = (byte)a }; }
        }
    }
}
