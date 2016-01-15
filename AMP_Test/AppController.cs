using Anfema.Amp.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMP_Test
{
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


        public AppController()
        {
            _ampConfig = new AmpConfig("http://Ampdev2.anfema.com/client/v1/", "test", "");
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
