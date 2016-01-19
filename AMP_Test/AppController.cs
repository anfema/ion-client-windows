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


        public async Task<AmpConfig> loginAsync()
        {
            AuthenticationHeaderValue authenticationHeader = await new AmpTokenAuthorization().LoginAsync("admin@anfe.ma", "test", "http://Ampdev2.anfema.com/client/v1/login");

            AmpConfig config = new AmpConfig("http://Ampdev2.anfema.com/client/v1/", "test", "de_DE", authenticationHeader);

            // Only testing purpose TODO: remove
            _ampConfig = config;

            return config;
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
