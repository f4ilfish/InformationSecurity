using System;

namespace InformationSecurity.Infrastructure.Encryptors
{
    /// <summary>
    /// Char encryptor class
    /// </summary>
    internal class CharEncryptor
    {
        /// <summary>
        /// Is latin char meth
        /// </summary>
        /// <param name="ch">Char</param>
        /// <returns>Is latin char</returns>
        private static bool IsLatinChar(char ch)
        {
            return (ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z');
        }

        /// <summary>
        /// Is cyrillic char meth
        /// </summary>
        /// <param name="ch">Char</param>
        /// <returns>Is cyrillic char</returns>
        private static bool IsCyrillicChar(char ch)
        {
            return (ch >= 'А' && ch <= 'Я' || ch >= 'а' && ch <= 'я');
        }

        /// <summary>
        /// Get shifted char meth
        /// </summary>
        /// <param name="ch">Char to shift</param>
        /// <param name="key">Shift key</param>
        /// <param name="startChar">Start (or the end) alphabet char</param>
        /// <param name="alphabetLength">Length of alphabet</param>
        /// <returns>Char shifted by key</returns>
        private static char GetShiftedChar(char ch, int key, char startChar, int alphabetLength)
        {
            return (char)(startChar + ((ch + key - startChar) % alphabetLength));
        }

        /// <summary>
        /// Get Ceaser encrypted char
        /// </summary>
        /// <param name="ch">Char to encrypt</param>
        /// <param name="keyInt">Int shift key</param>
        /// <param name="decryption">Is decryption</param>
        /// <returns>Encrypted char by Ceaser alg</returns>
        public static char GetCeasarEncryptedChar(char ch, int keyInt, bool decryption)
        {
            if (char.IsLetter(ch))
            {
                if (IsLatinChar(ch))
                {
                    var latinAlphabetLenght = 26;
                    char startChar;

                    var key = decryption ? -keyInt : keyInt;

                    if (key >= 0)
                    {
                        startChar = char.IsUpper(ch) ? 'A' : 'a';
                    }
                    else
                    {
                        startChar = char.IsUpper(ch) ? 'Z' : 'z';
                    }

                    return GetShiftedChar(ch, key, startChar, latinAlphabetLenght);
                }
                else if (IsCyrillicChar(ch))
                {
                    var cyrillicAlphabetLenght = 32;
                    char startChar;

                    var key = decryption ? -keyInt : keyInt;

                    if (key >= 0)
                    {
                        startChar = char.IsUpper(ch) ? 'А' : 'а';
                    }
                    else
                    {
                        startChar = char.IsUpper(ch) ? 'Я' : 'я';
                    }

                    return GetShiftedChar(ch, key, startChar, cyrillicAlphabetLenght);
                }
                else
                {
                    return ch;
                }
            }
            else if (char.IsDigit(ch))
            {
                var numericAlphabetLenght = 10;
                char startChar;

                var key = decryption ? -keyInt : keyInt;

                if (key >= 0)
                {
                    startChar = '0';
                }
                else
                {
                    startChar = '9';
                }

                return GetShiftedChar(ch, key, startChar, numericAlphabetLenght);
            }
            else
            {
                return ch;
            }
        }

        /// <summary>
        /// Get shift key from shift key char
        /// </summary>
        /// <param name="keyCh">Shift key char</param>
        /// <returns>Shift key by Vigener alg</returns>
        /// <exception cref="ArgumentException"></exception>
        private static int GetKey(char keyCh)
        {
            if (char.IsLetter(keyCh))
            {
                if (IsLatinChar(keyCh))
                {
                    return char.IsUpper(keyCh) ? (keyCh - 'A') : (keyCh - 'a');
                }
                else if (IsCyrillicChar(keyCh))
                {
                    return char.IsUpper(keyCh) ? (keyCh - 'А') : (keyCh - 'а');
                }
                else
                {
                    throw new ArgumentException("keyCh is not Latin or Cyrillic char");
                }
            }
            else if (char.IsDigit(keyCh))
            {
                return keyCh - '0';
            }
            else
            {
                throw new ArgumentException("keyCh is not letter or digit char");
            }
        }

        /// <summary>
        /// Get Vigener encrypted char
        /// </summary>
        /// <param name="ch">Char to encrypt</param>
        /// <param name="keyCh">Shift key char</param>
        /// <param name="decryption">Is decryption</param>
        /// <returns>Encrypted char by Vigener alg</returns>
        public static char GetVigenerEncryptedChar(char ch, char keyCh, bool decryption)
        {
            if (char.IsLetterOrDigit(keyCh))
            {
                if (char.IsLetter(ch))
                {
                    if (IsLatinChar(ch))
                    {
                        var latinAlphabetLenght = 26;
                        char startChar;

                        var key = decryption ? -(GetKey(keyCh) % latinAlphabetLenght)
                                                : (GetKey(keyCh) % latinAlphabetLenght);

                        if (key >= 0)
                        {
                            startChar = char.IsUpper(ch) ? 'A' : 'a';
                        }
                        else
                        {
                            startChar = char.IsUpper(ch) ? 'Z' : 'z';
                        }

                        return GetShiftedChar(ch, key, startChar, latinAlphabetLenght);
                    }
                    else if (IsCyrillicChar(ch))
                    {
                        var cyrillicAlphabetLenght = 32;
                        char startChar;

                        var key = decryption ? -(GetKey(keyCh) % cyrillicAlphabetLenght)
                                                : (GetKey(keyCh) % cyrillicAlphabetLenght);

                        if (key >= 0)
                        {
                            startChar = char.IsUpper(ch) ? 'А' : 'а';
                        }
                        else
                        {
                            startChar = char.IsUpper(ch) ? 'Я' : 'я';
                        }

                        return GetShiftedChar(ch, key, startChar, cyrillicAlphabetLenght);
                    }
                    else
                    {
                        return ch;
                    }
                }
                else if (char.IsDigit(ch))
                {
                    var numericAlphabetLenght = 10;
                    char startChar;

                    var key = decryption ? -(GetKey(keyCh) % numericAlphabetLenght)
                                           : (GetKey(keyCh) % numericAlphabetLenght);

                    if (key >= 0)
                    {
                        startChar = '0';
                    }
                    else
                    {
                        startChar = '9';
                    }

                    return GetShiftedChar(ch, key, startChar, numericAlphabetLenght);
                }
                else
                {
                    return ch;
                }
            }
            else
            {
                return ch;
            }
        }
    }
}
