using Security.Cryptography;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;

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
            RSACryptography RSA = new RSACryptography();
            string publicKey, privateKey;

            // Generate RSA key pair
            RSA.GenerateKeys(out publicKey, out privateKey);

            string plainText = Console.ReadLine();// Guid.NewGuid().ToString();

            // Encrypt
            string encryptedText = RSA.Encrypt(publicKey, plainText);

            // Decrypt
            string decryptedText = RSA.Decrypt(privateKey, encryptedText);

            string file = publicKey;

            //test RSA
            Console.WriteLine(plainText);
            Console.WriteLine("PublicKey");
            Console.WriteLine(publicKey);
            Console.WriteLine("PrivateKey");
            Console.WriteLine(privateKey);
            Console.WriteLine("encryptedText");
            Console.WriteLine(encryptedText);
            Console.WriteLine("decrytedText");
            Console.WriteLine(decryptedText);

            //test SHA256
            string plainData = "Mahesh";
            Console.WriteLine("Raw data: {0}", plainData);
            string hashedData = ComputeSha256Hash(plainData);
            Console.WriteLine("Hash {0}", hashedData);
            Console.WriteLine(ComputeSha256Hash("Mahesh"));

            Console.ReadKey();
        }
    }

}




