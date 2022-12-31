using Security.Cryptography;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Net.Security;

namespace Security.Cryptography
{
    class program
    {

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

           

            RSA Test = new RSA();
            Test.RSA_taoKhoa();
            Test.RSA_maHoa();
            Test.RSA_giaiMa();
            ////test sha256
            //string plaindata = "test hash";
            //console.writeline("raw data: {0}", plaindata);
            //string hasheddata = computesha256hash(plaindata);
            //console.writeline("hash {0}", hasheddata);
            //console.writeline(computesha256hash("mahesh"));



            Console.ReadKey();
        }
    }

}




