using Anfema.Amp.Parsing;
using Newtonsoft.Json;
using System;
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

        public List<ContainerContent> contents { get; set; }

        public List<AmpPageContent> content { get; set; }
        public DateTime last_changed { get; set; }
        public int position { get; set; }
        public Uri archive { get; set; }
        
        // TODO: include missing properties (layout, position)



        public AmpPage()
        {
            content = new List<AmpPageContent>();
            children = new List<string>();
        }
    }


    public class ContainerContent
    {
        public string type { get; set; }
        public string variation { get; set; }
        public string outlet { get; set; }
        public List<AmpContent> children { get; set; }
    }


    public class AMPPageRoot
    {
        public List<AmpPage> page { get; set; }
    }
}