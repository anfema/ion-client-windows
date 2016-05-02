using System;
using System.Net.Http.Headers;
using System.Text;


namespace Anfema.Ion.Authorization
{
    public class BasicAuth
    {
        // Generate base64 encoded header from username and password
        public static AuthenticationHeaderValue GetAuthHeaderValue( string username, string password )
        {
            try
            {
                string credentials = username + ':' + password;
                string credentialsBase64 = Convert.ToBase64String( Encoding.UTF8.GetBytes( credentials ) );
                return new AuthenticationHeaderValue( "Basic", credentialsBase64 );
            }
            catch
            {
                return null;
            }
        }
    }
}
