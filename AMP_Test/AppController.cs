﻿using Anfema.Amp.Authorization;
using Anfema.Amp.Caching;
using Anfema.Amp.DataModel;
using Anfema.Amp.FullTextSearch;
using Anfema.Amp.MediaFiles;
using Anfema.Amp.Utils;
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

        private AmpConfig _ampConfig;

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
        

        public async Task<AmpConfig> loginAsync()
        {
            if ( !_loggedIn )
            {
                String collectionIdentifier = "seoul-november-2014";

                AuthenticationHeaderValue authorizationHeader = CollectionAuthStore.Get( collectionIdentifier );

                if ( authorizationHeader == null )
                {
                    authorizationHeader = await TokenAuthorization.GetAuthHeaderValue( "admin@anfe.ma", "test", "http://bireise-dev.anfema.com/client/v1/login" );
                    // Or using BasicAuth
                    //AuthenticationHeaderValue authenticationHeader = BasicAuth.GetAuthHeaderValue("admin@anfe.ma", "test");
                }

                // Store authorization header
                CollectionAuthStore.Set( collectionIdentifier, authorizationHeader );

                AmpConfig config = new AmpConfig("http://bireise-dev.anfema.com/client/v1/", "de_DE", collectionIdentifier, authorizationHeader, 120, 100, false);
                
                // Only testing purpose TODO: remove
                _ampConfig = config;
                _loggedIn = true;
                return config;
            }

            return _ampConfig;
        }


        public AmpConfig ampConfig
        {
            get
            {
                return _ampConfig;
            }
        }
    }
}
