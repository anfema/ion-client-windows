using System;
using System.IO;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Anfema.Amp.Utils
{
    public class HashUtils
    {
        public static string GetMD5Hash( string input )
        {
            return GetHash( input, HashAlgorithmNames.Md5 );
        }

        public static string GetMD5Hash( Stream input )
        {
            return GetHash( input, HashAlgorithmNames.Md5 );
        }

        public static string GetSHA256Hash( string input )
        {
            return GetHash( input, HashAlgorithmNames.Sha256 );
        }

        public static string GetSHA256Hash( Stream input )
        {
            return GetHash( input, HashAlgorithmNames.Sha256 );
        }

        private static string GetHash( string input, string algName )
        {
            // Convert the message string to binary data.
            IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);
            return GetHash( buffUtf8Msg.AsStream(), algName );
        }

        private static string GetHash( Stream input, string algName )
        {
            // Create a HashAlgorithmProvider object.
            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(algName);

            CryptographicHash cryptoHash = objAlgProv.CreateHash();

            using ( input )
            {
                byte[] b = new byte[2048];
                int r;
                while ( ( r = input.Read( b, 0, b.Length ) ) > 0 )
                {
                    cryptoHash.Append( b.AsBuffer( 0, r ) );
                }
            }

            // Hash the message.
            IBuffer buffHash = cryptoHash.GetValueAndReset();
            
            // Convert the hash to a string (for display).
            string strHashBase64 = CryptographicBuffer.EncodeToHexString(buffHash);

            // Return the encoded string
            return strHashBase64;
        }
    }
}
