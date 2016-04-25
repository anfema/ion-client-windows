using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Anfema.Ion.DataModel
{
    public class IonConnectionContent : IonContent
    {
        [JsonProperty( "connection_string" )]
        public string connectionString
        {
            get
            {
                return _connectionString;
            }

            set
            {
                _connectionString = value;
                initialize();
            }
        }

        // Public properties to access the generated parameters from the connection string
        public string scheme { get { return _scheme; } }
        public string collectionIdentifier { get { return _collectionIdentifier; } }
        public List<string> pageIdentifierPath { get { return _pageIdentifierPath; } }
        public string pageIdentifier { get { return _pageIdentifier; } }
        public string contentIdentifier { get { return _contentIdentifier; } }

        // Original connection string
        private string _connectionString = "";

        // This parameters are generated from 
        private string _scheme = "";
        private string _collectionIdentifier = "";
        private List<string> _pageIdentifierPath = new List<string>();
        private string _pageIdentifier = "";
        private string _contentIdentifier = "";


        /// <summary>
        /// Inits all parameters with the values extracted from the connectionString
        /// </summary>
        private void initialize()
        {
            // Validate uri
            Uri uri;
            bool validUri = Uri.TryCreate( _connectionString, UriKind.Absolute, out uri );

            // If uri is valid then parse the desired content from it
            if( validUri )
            {
                _scheme = uri.Scheme;
                _collectionIdentifier = uri.Host;

                for( int i = 0; i < uri.Segments.Length; i++ )
                {
                    _pageIdentifierPath.Add( uri.Segments[i] );
                }

                if( _pageIdentifierPath != null && _pageIdentifierPath.Count != 0 )
                {
                    _pageIdentifier = _pageIdentifierPath[_pageIdentifierPath.Count - 1];
                }

                _contentIdentifier = uri.Fragment;
            }
        }


        /// <summary>
        /// Used to get a string representation of all parameters of this connection content
        /// </summary>
        /// <returns>String containing all informations for this connection content</returns>
        public override string ToString()
        {
            return base.ToString() + " + [scheme = " + scheme + ", collection = " + collectionIdentifier + ", page = " + pageIdentifier + ", content = " + collectionIdentifier + "]";
        }


        /// <summary>
        /// Check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if equal and false otherwise</returns>
        public override bool Equals( object obj )
        {
            // Basic IonContent equality check
            if( !base.Equals( obj ) )
            {
                return false;
            }

            try
            {
                // Try to cast to this type
                IonConnectionContent content = (IonConnectionContent)obj;

                return connectionString.Equals( content.connectionString )
                    && scheme.Equals( content.scheme )
                    && collectionIdentifier.Equals( content.collectionIdentifier )
                    && pageIdentifierPath.Equals( content.pageIdentifierPath )
                    && pageIdentifier.Equals( content.pageIdentifier )
                    && contentIdentifier.Equals( content.contentIdentifier );
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
