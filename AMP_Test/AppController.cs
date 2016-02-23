using Anfema.Amp.Authorization;
using Anfema.Amp.DataModel;
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
                if (_instance != null)
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
            if (!_loggedIn)
            {
                AuthenticationHeaderValue authenticationHeader = await new AmpTokenAuthorization().LoginAsync("admin@anfe.ma", "test", "http://Ampdev2.anfema.com/client/v1/login");

                AmpConfig config = new AmpConfig("http://Ampdev2.anfema.com/client/v1/", "de_DE", "test", authenticationHeader, 5, 100, false);

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
