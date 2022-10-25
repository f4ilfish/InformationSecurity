using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InformationSecurity.Infrastructure.Encryptors
{
    internal class RSAEncryptor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
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
        /// 
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="qValue"></param>
        /// <returns></returns>
        public static int GetNValue(int pValue, int qValue)
        {
            return pValue * qValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="qValue"></param>
        /// <returns></returns>
        public static int GetKValue(int pValue, int qValue)
        {
            return (pValue - 1) * (qValue - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nValue"></param>
        /// <param name="kValue"></param>
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
        /// 
        /// </summary>
        /// <param name="upperBound"></param>
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
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
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
        /// 
        /// </summary>
        /// <param name="dValue"></param>
        /// <param name="kValue"></param>
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
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="eValue"></param>
        /// <param name="nValue"></param>
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
        /// 
        /// </summary>
        /// <param name="encryptedTextAsNumbers"></param>
        /// <param name="dValue"></param>
        /// <param name="nValue"></param>
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
