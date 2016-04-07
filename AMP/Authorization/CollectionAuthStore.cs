using Anfema.Ion.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Anfema.Ion.Authorization
{
    public class CollectionAuthStore
    {
        private static Dictionary<String, AuthenticationHeaderValue> authenticationHeaders = new Dictionary<String, AuthenticationHeaderValue>();

        public static AuthenticationHeaderValue Get( String collectionIdentifier )
        {
            // Create authentication headers dictionary if not present
            if ( authenticationHeaders == null )
            {
                authenticationHeaders = new Dictionary<String, AuthenticationHeaderValue>();
            }

            // Try to get authentication header from dictionary
            if ( authenticationHeaders.ContainsKey( collectionIdentifier ) )
            {
                return authenticationHeaders[collectionIdentifier];
            }

            // Try to get authentication header from app storage
            String scheme = StorageUtils.GetValue<String>( collectionIdentifier + "_scheme" );
            String parameter = StorageUtils.GetValue<String>( collectionIdentifier + "_parameter" );

            if ( scheme == null || parameter == null )
            {
                return null;
            }
            
            AuthenticationHeaderValue authenticationHeader = new AuthenticationHeaderValue( scheme, parameter );

            // Add authentication header to dictionary
            authenticationHeaders[collectionIdentifier] = authenticationHeader;

            return authenticationHeader;
        }

        public static void Set( String collectionIdentifier, AuthenticationHeaderValue authenticationHeaderValue )
        {
            // Create authentication headers dictionary if not present
            if ( authenticationHeaders == null )
            {
                authenticationHeaders = new Dictionary<String, AuthenticationHeaderValue>();
            }

            // Add authentication header to dictionary
            authenticationHeaders[collectionIdentifier] = authenticationHeaderValue;

            // Save to app storage
            StorageUtils.SetValue( collectionIdentifier + "_scheme", authenticationHeaderValue.Scheme );
            StorageUtils.SetValue( collectionIdentifier + "_parameter", authenticationHeaderValue.Parameter );
        }

        public static void Set( String collectionIdentifier, String username, String password )
        {
            Set( collectionIdentifier, BasicAuth.GetAuthHeaderValue( username, password ) );
        }
    }
}
