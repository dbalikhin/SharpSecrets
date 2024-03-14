using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace secret_synth.Pages
{
    public class EncryptionModel : PageModel
    {
        private readonly ILogger<EncryptionModel> _logger;


        public string ClientData { get; set; }

        public EncryptionModel(ILogger<EncryptionModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string encryptedGameState = "badone", int version = 1)
        {
            if (version == 1)
            {
                ClientData = DecryptString(encryptedGameState, _gameSecret);
            }
            else
            {
                ClientData = DecryptString(encryptedGameState, _gameSecret2);
            }

            var dosomethingwiththis = Base64UrlEncoder.Decode(Base64EncodedNotASecret);
        }

        private static string Base64EncodedNotASecret = "bW9kdWxlIEFwcE1vZHVsZQogIGNsYXNzIEFwcGxpY2F0aW9uIDwgUmFpbHM6OkFwcGxpY2F0aW9uCiAgICBkZWYgY3JlZGVudGlhbHMKICAgICAgaWYgUmFpbHMuZW52LnByb2R1Y3Rpb24/CiAgICAgICAgc3VwZXIKICAgICAgZWxzZQogICAgICAgIGVuY3J5cHRlZCgKICAgICAgICAgICJjb25maWcvY3JlZGVudGlhbHMuI3tSYWlscy5lbnZ9LnltbC5lbmMiLAogICAgICAgICAga2V5X3BhdGg6ICJjb25maWcvI3tSYWlscy5lbnZ9LmtleSIKICAgICAgICApCiAgICAgIGVuZAogICAgZW5kCiAgZW5kCmVuZA==";
/*1*/   private static string _staticIV = "0000-0000-0000-0000"; // hard-coded IV

/*2*/   private static string _gameSecret = "dGhpc2lzdmVyeXN0cm9uZ3NlY3JldGZvcnN1cmViZWNhdXNlaXRpc2xvbmch"; // base64 secret
/*3*/   private static string _gameSecret2 = "FD,B0BleH/Ed;_JEc5i.F(Jj%ATVO&EcZADAR]4\\@<HX&Bln'1Ci=3(+T"; // base85 secret because why not

        // encrypt a string with aes-cbc and return the encrypted string
        public string EncryptString(string plainText, string key)
        {
            // check arguments
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(_staticIV);
                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }

        //decrypt a string with aes-cbc and return the decrypted string
        public string DecryptString(string cipherText, string key)
        {
            // check arguments
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(_staticIV);
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}