﻿using Anfema.Ion.Parsing;
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
        public List<IonContent> getContents()
        {
            return contents[0].children;
        }


        /// <summary>
        /// Used to find all IonContent of this page matching the given outlet's name
        /// </summary>
        /// <param name="outlet"></param>
        /// <returns>List of IonContent</returns>
        public List<IonContent> getContents( string outlet )
        {
            // Create empty list for IonContent
            List<IonContent> contents = new List<IonContent>();

            if( outlet != null )
            {
                // Search all content for matching the outlet name
                for( int i = 0; i < this.contents[0].children.Count; i++ )
                {
                    if( this.contents[0].children[i].outlet.Equals( outlet ) )
                    {
                        contents.Add( this.contents[0].children[i] );
                    }
                }
            }

            // Return all found IonContent matching the outlet name
            return contents;
        }


        /// <summary>
        /// Searches a specific outlet throughout the content of the page
        /// </summary>
        /// <param name="outlet"></param>
        /// <returns>IonContent with the given name or null, if no IonContent was found</returns>
        public IonContent getContent( string outlet )
        {
            for( int i = 0; i < contents[0].children.Count; i++ )
            {
                if( contents[0].children[i].outlet.Equals( outlet ) )
                {
                    return contents[0].children[i];
                }
            }

            return null;
        }


        /// <summary>
        /// Searches the page for a text content and returns its text or null, if the text outlet wasn't found
        /// </summary>
        /// <param name="outlet"></param>
        /// <returns>String or null</returns>
        public string getTextOrNull( string outlet )
        {
            IonContent content = getContent( outlet );

            if( content != null && content.GetType() == typeof( IonTextContent ) )
            {
                return ( (IonTextContent)content ).text;
            }

            return null;
        }


        /// <summary>
        /// Searches the page for a text content with the given name and returns its text or an empty string, if the outlet wasn't found
        /// </summary>
        /// <param name="outlet"></param>
        /// <returns>String or emptyString</returns>
        public string getTextOrEmpty( string outlet )
        {
            IonContent content = getContent( outlet );

            if( content != null && content.GetType() == typeof( IonTextContent ) )
            {
                return ( (IonTextContent)content ).text;
            }

            return "";
        }
    }
}