using System.Collections.Generic;

namespace Anfema.Amp.DataModel
{
    public class AmpPage
    {
        public string parent { get; set; }
        public string identifier { get; set; }
        public string collection { get; set; }
        public List<AmpPageTranslation> translations { get; set; }
        public List<string> children { get; set; }

        public AmpPage()
        {
            translations = new List<AmpPageTranslation>();
            children = new List<string>();
        }
    }
}