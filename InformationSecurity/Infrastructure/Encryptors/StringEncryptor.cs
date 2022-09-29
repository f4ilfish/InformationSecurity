namespace InformationSecurity.Infrastructure.Encryptors
{
    /// <summary>
    /// String encryptor class
    /// </summary>
    internal class StringEncryptor
    {
        /// <summary>
        /// Get encrypted string by Ceaser alg
        /// </summary>
        /// <param name="stringToEncrypt">String to encrypt</param>
        /// <param name="keyInt">Int shift key</param>
        /// <param name="decryption">Is decryption</param>
        /// <returns></returns>
        public static string GetCeaserEncryptedString(string stringToEncrypt, int keyInt, bool decryption)
        {
            var charArrayToEncrypt = stringToEncrypt.ToCharArray();
            var encryptedCharArray = new char[charArrayToEncrypt.Length];

            for (int i = 0; i < encryptedCharArray.Length; i++)
            {
                encryptedCharArray[i] = CharEncryptor.
                                        GetCeasarEncryptedChar(charArrayToEncrypt[i],
                                                                             keyInt,
                                                                             decryption);
            }

            return new string(encryptedCharArray);
        }

        /// <summary>
        /// Get encrypted string by Vigener alg 
        /// </summary>
        /// <param name="stringToEncrypt"></param>
        /// <param name="keyString"></param>
        /// <param name="decryption"></param>
        /// <returns></returns>
        public static string GetVigenerEncyptedString(string stringToEncrypt, string keyString, bool decryption)
        {
            var charArrayToEncrypt = stringToEncrypt.ToCharArray();
            var keyCharArray = keyString.ToCharArray();

            string encryptedString = string.Empty;
            int keyLength = keyCharArray.Length;
            int keyIndex = 0;

            foreach (char c in charArrayToEncrypt)
            {
                encryptedString += CharEncryptor.GetVigenerEncryptedChar(c, keyCharArray[keyIndex], decryption);
                
                if (char.IsLetterOrDigit(c)) keyIndex++;

                if (keyIndex == keyLength) keyIndex = 0;
            }

            return encryptedString;
        }
    }
}
