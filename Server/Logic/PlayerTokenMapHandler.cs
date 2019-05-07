using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Server.Logic
{
    public class PlayerTokenMap
    {
        private byte[] symetricEncKey;
        private byte[] IV;
        private Dictionary<Guid,int> map=null;

        public PlayerTokenMap(Guid symetricEncKey,Guid IV,Dictionary<Guid,int> initialMap):this(symetricEncKey,IV){
            map=initialMap;
        }
        public PlayerTokenMap(Guid symetricEncKey,Guid IV){
            this.symetricEncKey=symetricEncKey.ToByteArray();
            this.IV=IV.ToByteArray();
        }
        //these two shamelessly adapted from 
        //https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=netframework-4.8
        public void SetToString(string encryptedRep)
        {
            if(encryptedRep==null) 
                throw new ArgumentNullException(nameof(encryptedRep));
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize=128;
                aesAlg.Key = symetricEncKey;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedRep)))
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
            map=JsonConvert.DeserializeObject<Dictionary<Guid,int>>(plaintext);
        }
        public string AsEncryptedJson()
        {
            var plainText=JsonConvert.SerializeObject(map);
            byte[] encrypted;
            
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize=128;
                aesAlg.Key = symetricEncKey;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
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

        public ICollection<Guid> Keys => ((IDictionary<Guid, int>)map).Keys;
        public ICollection<int> Values => ((IDictionary<Guid, int>)map).Values;
        public int Count => ((IDictionary<Guid, int>)map).Count;
        public int this[Guid key] { get => ((IDictionary<Guid, int>)map)[key]; }
        public bool ContainsKey(Guid key)
        {
            return ((IDictionary<Guid, int>)map).ContainsKey(key);
        }
        public bool TryGetValue(Guid key, out int value)
        {
            return ((IDictionary<Guid, int>)map).TryGetValue(key, out value);
        }

        public bool Contains(KeyValuePair<Guid, int> item)
        {
            return ((IDictionary<Guid, int>)map).Contains(item);
        }
        public IEnumerator<KeyValuePair<Guid, int>> GetEnumerator()
        {
            return ((IDictionary<Guid, int>)map).GetEnumerator();
        }
    }
}

