using Anfema.Ion.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Anfema.Ion.DataModel
{
    public class IonPage
    {
        [JsonProperty( Order = 1 )]
        public string parent { get; set; }

        [JsonProperty( Order = 2 )]
        public string identifier { get; set; }

        [JsonProperty( Order = 3 )]
        public string collection { get; set; }

        [JsonProperty( Order = 4 )]
        public DateTime last_changed { get; set; }

        [JsonProperty( Order = 5 )]
        public Uri archive { get; set; }

        [JsonProperty( Order = 6 )]
        public List<ContainerContent> contents { get; set; }

        [JsonProperty( Order = 7 )]
        public List<string> children { get; set; }

        [JsonProperty( Order = 8 )]
        public string locale { get; set; }

        [JsonProperty( Order = 9 )]
        public string layout { get; set; }

        [JsonProperty( Order = 10 )]
        public int position { get; set; }

        [JsonProperty( Order = 11 )]
        public List<IonContent> emptyContent { get; set; }




        public IonPage()
        {
            children = new List<string>();
            emptyContent = new List<IonContent>();
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


        /// <summary>
        /// Sorts out empty content and moves it to the emptyContent list
        /// </summary>
        public void sortOutEmptyContent()
        {
            for( int i = 0; i < contents[0].children.Count; i++ )
            {
                if( !contents[0].children[i].isAvailable )
                {
                    // Add empty content to list of empty contents
                    emptyContent.Add( contents[0].children[i] );

                    // Remove the empty content from the content list
                    contents[0].children.RemoveAt( i );

                    // Decrease the counter, because we removed an item from the list we're working on
                    --i;
                }
            }


            foreach( IonContent content in contents[0].children )
            {
                if( !content.isAvailable )
                {
                    // Add empty content to list of empty contents
                    emptyContent.Add( content );

                    // Remove the empty content from the content list
                    contents[0].children.Remove( content );
                }
            }
        }


        /// <summary>
        /// Checks for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if pages are equal, false otherwise</returns>
        public override bool Equals( object obj )
        {
            if( obj == this )
            {
                return true;
            }

            if( obj == null )
            {
                return false;
            }

            try
            {
                // Try to cast
                IonPage page = (IonPage)obj;

                // Compare each value
                return parent.Equals( page.parent )
                    && identifier.Equals( page.identifier )
                    && collection.Equals( page.collection )
                    && last_changed == page.last_changed
                    && archive.Equals( page.archive )
                    && contents.Equals( page.contents )
                    && children.Equals( page.children )
                    && locale.Equals( page.locale )
                    && layout.Equals( page.layout )
                    && position == page.position;
            }

            catch
            {
                return false;
            }

        }


        /// <summary>
        /// Returns the exact hashCode that the base class would do
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}