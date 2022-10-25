using System;
using System.Collections.Generic;
using System.Numerics;

namespace InformationSecurity.Infrastructure.Encryptors
{
    /// <summary>
    /// RSAEncryptor class
    /// </summary>
    internal class RSAEncryptor
    {
        /// <summary>
        /// Prime number validation
        /// </summary>
        /// <param name="value">input value</param>
        /// <returns></returns>
        public static bool IsPrimeNumber(int value)
        {
            if (value <= 0) return false;

            for (int i = 2; i < value; i++)
            {
                if (value % i == 0) return false;
            }

            return true;
        }

        /// <summary>
        /// Get common for both keys N value
        /// </summary>
        /// <param name="pValue">general p value</param>
        /// <param name="qValue">general q value</param>
        /// <returns></returns>
        public static int GetNValue(int pValue, int qValue)
        {
            return pValue * qValue;
        }

        /// <summary>
        /// Get general K value
        /// </summary>
        /// <param name="pValue">general p value</param>
        /// <param name="qValue">general q value</param>
        /// <returns></returns>
        public static int GetKValue(int pValue, int qValue)
        {
            return (pValue - 1) * (qValue - 1);
        }

        /// <summary>
        /// Get secret key's D value
        /// </summary>
        /// <param name="nValue">common N value</param>
        /// <param name="kValue">general K value</param>
        /// <returns></returns>
        public static int GetDValue(int nValue, int kValue)
        {
            var primeNumberList = GetPrimeNumberList(nValue);
            var rnd = new Random();
            int dValue;
            do
            {
                dValue = primeNumberList[rnd.Next(primeNumberList.Count)];
            }
            while (!IsPrimeNumber(dValue) | GetGreatestCommonDevisor(dValue, kValue) != 1);

            return dValue;
        }

        /// <summary>
        /// Get list of prime numbers 
        /// </summary>
        /// <param name="upperBound">upper bound value</param>
        /// <returns></returns>
        private static List<int> GetPrimeNumberList(int upperBound)
        {
            var primeNumberList = new List<int>();

            for (int i = 2; i < upperBound; i++)
            {
                if (IsPrimeNumber(i)) primeNumberList.Add(i);
            }

            return primeNumberList;
        }

        /// <summary>
        /// Get greatest common devisor
        /// </summary>
        /// <param name="a">input value</param>
        /// <param name="b">input value</param>
        /// <returns></returns>
        public static int GetGreatestCommonDevisor(int a, int b)
        {
            if (a == b)
                return a;
            else
                if (a > b)
                return GetGreatestCommonDevisor(a - b, b);
            else
                return GetGreatestCommonDevisor(b - a, a);
        }

        /// <summary>
        /// Get open key's E value
        /// </summary>
        /// <param name="dValue">secret key's D value</param>
        /// <param name="kValue">general K value</param>
        /// <returns></returns>
        public static int GetEValue(int dValue, int kValue)
        {
            int eValue = 0;
            do
            {
                eValue++;
            }
            while ((eValue * dValue) % kValue != 1);

            return eValue;
        }

        /// <summary>
        /// Encrypt text by RSA algorithm
        /// </summary>
        /// <param name="text">text to encryption</param>
        /// <param name="eValue">open key's E value</param>
        /// <param name="nValue">common N value</param>
        /// <returns></returns>
        public static List<int> EncryptRSA(string text, int eValue, int nValue)
        {
            var encryptedTextAsNumbers = new List<int>();

            foreach (char ch in text)
            {
                int encryptedCharAsNumber = (int)BigInteger.ModPow((byte)ch, eValue, nValue);
                encryptedTextAsNumbers.Add(encryptedCharAsNumber);
            }

            return encryptedTextAsNumbers;
        }

        /// <summary>
        /// Decrypt list of numbers by RSA algorithm
        /// </summary>
        /// <param name="encryptedTextAsNumbers">list of numbers</param>
        /// <param name="dValue">secret key's D value</param>
        /// <param name="nValue">common N value</param>
        /// <returns></returns>
        public static string DecryptRSA(List<int> encryptedTextAsNumbers, long dValue, long nValue)
        {
            string decryptedText = string.Empty;

            foreach (int number in encryptedTextAsNumbers)
            {
                byte decryptedChar = (byte)BigInteger.ModPow(number, dValue, nValue);
                decryptedText += (char)decryptedChar;
            }

            return decryptedText;
        }
    }
}
