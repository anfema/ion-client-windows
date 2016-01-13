using System.Collections.Generic;

namespace Anfema.Amp.DataModel
{
    public class PageAllTranslationsModel
    {
        public string name { get; set; }
        public List<string> translations { get; set; }

        public PageAllTranslationsModel()
        {
            translations = new List<string>();
        }
    }
}
