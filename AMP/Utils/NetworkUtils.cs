using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Anfema.Amp.Utils
{
    public class NetworkUtils
    {
        /// <summary>
        /// Used to check if the device is online
        /// </summary>
        /// <returns>Online state of the device: true if it is only and false if not</returns>
        public static bool isOnline()
        {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            bool isOnline = (connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
            return isOnline;
        }


        // Returns the type of internet access. May be "none" if no internet connection it available
        public static ConnectionProfile isOnlineType()
        {
            return NetworkInformation.GetInternetConnectionProfile();
        }
    }
}
