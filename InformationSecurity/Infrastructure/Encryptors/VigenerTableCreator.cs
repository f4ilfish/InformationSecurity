using System.Collections.Generic;

namespace InformationSecurity.Infrastructure.Encryptors
{
    /// <summary>
    /// VigenerTableCreator class
    /// </summary>
    internal static class VigenerTableCreator
    {
        /// <summary>
        /// EncryptionAlphabet const
        /// </summary>
        const string EncryptionAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                          "abcdifghijklmnopqrstuvwxyz" +
                                          "АБВГДЕЖЗИЙКЛМНОПРСТФХЦЧШЩЪЫЬЭЮЯ" +
                                          "абвгдежзиклмнопрстуфхцчшщъыьэюя" +
                                          "0123456789";

        /// <summary>
        /// Get Vigener table method
        /// </summary>
        /// <returns></returns>
        public static List<string> GetVigenerTable()
        {
            var vigenerTable = new List<string>
            {
                EncryptionAlphabet
            };

            foreach (char ch in EncryptionAlphabet)
            {
                var vigenerTableRow = string.Empty;
                vigenerTableRow += ch;

                foreach (char keyCh in EncryptionAlphabet)
                {
                    vigenerTableRow += CharEncryptor.GetVigenerEncryptedChar(keyCh, ch, false);
                }

                vigenerTable.Add(vigenerTableRow);
            }

            return vigenerTable;
        }
    }
}
