using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Anfema.Amp.Utils
{
    public class NetworkUtils
    {
        // Checks if the device is online
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
