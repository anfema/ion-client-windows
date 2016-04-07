using Anfema.Ion.Parsing;
using System;
using System.Collections.Generic;


namespace Anfema.Ion.DataModel
{
    public class IonPage
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



        public IonPage()
        {
            children = new List<string>();
        }


        /// <summary>
        /// Used to get the complete content of a page
        /// </summary>
        /// <returns>List of IonContent</returns>
        public List<IonContent> getContent()
        {
            return contents[0].children;
        }
    }
}