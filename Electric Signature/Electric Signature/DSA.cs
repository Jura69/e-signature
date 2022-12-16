using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;

namespace Security.Cryptography
{
    class DSA
    {

        static void Main()
        {
            try
            {
                //Create a new instance of DSA.
                DSA DSA = DSA.Create();

                //The hash to sign.
                byte[] Hash = { 59, 4, 248, 102, 77, 97, 142, 201, 210, 12, 224, 93, 25, 41, 100, 197, 213, 134, 130, 135 };

                //Create an DSASignatureFormatter object and pass it the 
                //DSA instance to transfer the key information.
                DSASignatureFormatter DSAFormatter = new DSASignatureFormatter(DSA);

                //Set the hash algorithm to SHA1.
                DSAFormatter.SetHashAlgorithm("SHA1");

                //Create a signature for HashValue and return it.
                byte[] SignedHash = DSAFormatter.CreateSignature(Hash);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
