using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Ion.Authorization
{
    public class BasicAuth
    {
        // generate base64 encoded header from username and password
        public static AuthenticationHeaderValue GetAuthHeaderValue( string username, string password )
        {
            try
            {
                String credentials = username + ':' + password;
                String credentialsBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
                return new AuthenticationHeaderValue( "Basic", credentialsBase64 );
            }
            catch ( Exception e )
            {
                return null;
            }
        }
    }
}
