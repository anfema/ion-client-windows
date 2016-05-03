using Anfema.Ion.Authorization;
using Anfema.Ion.Caching;
using Anfema.Ion.DataModel;
using Anfema.Ion.FullTextSearch;
using Anfema.Ion.MediaFiles;
using Anfema.Ion.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AMP_Test
{
    /*** Simulates a real app that uses the AMP-Client ***/
    public class AppController
    {
        private static AppController _instance;

        private IonConfig _ampConfig;

        private bool _loggedIn;

        public static AppController instance
        {
            get
            {
                if ( _instance != null )
                {
                    return _instance;
                }

                _instance = new AppController();
                return _instance;
            }
        }


        public AppController()
        {
            _loggedIn = false;
        }
        

        public async Task<IonConfig> loginAsync()
        {
            if ( !_loggedIn )
            {
                String collectionIdentifier = "lookbook";

                AuthenticationHeaderValue authorizationHeader = await TokenAuthorization.GetAuthHeaderValue( "knauf-translator@anfe.ma", "Jdt9y9qHt3", "https://lookbook-dev.anfema.com/client/v1/login" );
                // Or using BasicAuth
                //AuthenticationHeaderValue authorizationHeader = BasicAuth.GetAuthHeaderValue( "knauf-translator@anfe.ma", "Jdt9y9qHt3" );

                IonConfig config = new IonConfig( "https://lookbook-dev.anfema.com/client/v1/", "de_DE", collectionIdentifier, "default", authorizationHeader, 120, 100, false);
                
                // Only testing purpose TODO: remove
                _ampConfig = config;

                // Set the loggedIn bool
                _loggedIn = authorizationHeader != null;

                return config;
            }

            return _ampConfig;
        }


        public IonConfig ampConfig
        {
            get
            {
                return _ampConfig;
            }
        }
    }
}
