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
        public DateTime last_changed { get; set; }
        public int position { get; set; }
        public Uri archive { get; set; }
        public string layout { get; set; }



        public AmpPage()
        {
            children = new List<string>();
        }


        /// <summary>
        /// Used to get the complete content of a page
        /// </summary>
        /// <returns>List of AmpContent</returns>
        public List<AmpContent> getContent()
        {
            return contents[0].children;
        }
    }
}