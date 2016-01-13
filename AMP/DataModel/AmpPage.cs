using System.Collections.Generic;

namespace Anfema.Amp.DataModel
{
    public class AmpPage
    {
        public string parent { get; set; }
        public string identifier { get; set; }
        public string collection { get; set; }
        public string locale { get; set; }
        public List<string> children { get; set; }
        public List<AmpPageContent> contents { get; set; }


        public AmpPage()
        {
            contents = new List<AmpPageContent>();
            children = new List<string>();
        }
    }
}