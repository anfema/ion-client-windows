using System;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;



namespace Anfema.Amp.Caching
{
    public class FilePaths
    {
        public static string getFileName( string url )
        {
            // Define MD5 as algorithm
            string strAlgName = HashAlgorithmNames.Md5;

            // Convert the message string to binary data.
            IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(url, BinaryStringEncoding.Utf8);

            // Create a HashAlgorithmProvider object.
            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(strAlgName);

            // Hash the message.
            IBuffer buffHash = objAlgProv.HashData(buffUtf8Msg);

            // Verify that the hash length equals the length specified for the algorithm.
            if (buffHash.Length != objAlgProv.HashLength)
            {
                throw new Exception("FilePaths: There was an error creating the hash");
            }

            // Convert the hash to a string (for display).
            string strHashBase64 = CryptographicBuffer.EncodeToHexString(buffHash);

            // Return the encoded string
            return strHashBase64;
        }
    }
}
