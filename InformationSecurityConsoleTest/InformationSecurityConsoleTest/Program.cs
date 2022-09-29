namespace InformationSecurityConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("--Init--");
            //var inputString = Console.ReadLine();

            //var charArray = inputString.ToCharArray();
            //var encryptedCharArray = new char[charArray.Length];

            //for (int i = 0; i < charArray.Length; i++)
            //{
            //    encryptedCharArray[i] = Encryptor.GetCeasarEncryptedChar(charArray[i], 33);
            //}

            //Console.WriteLine("--Direct--");
            //var outputString = new string(encryptedCharArray);
            //Console.WriteLine(outputString);

            //Console.WriteLine("--Reverse--");

            //var reverseEncryptedCharArray = new char[encryptedCharArray.Length];
            //for (int i = 0; i < encryptedCharArray.Length; i++)
            //{
            //    reverseEncryptedCharArray[i] = Encryptor.GetCeasarEncryptedChar(encryptedCharArray[i], 33, true);
            //}
            //var reverseOutputString = new string(reverseEncryptedCharArray);
            //Console.WriteLine(reverseOutputString);

            var inputString = "1234567890 ABCDIFGHIJKLMNOPQRST abcdifghijklmn АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ абвгдежзиклмнопрстуфхцчшщъыьэюя";
            var keyString = "1 12 '''12 ауауцц core";

            Console.WriteLine(inputString);

            //var inputCharArray = inputString.ToCharArray();
            //var keyCharArray = keyString.ToCharArray();

            //string encryptedString = string.Empty;
            //int keyLength = keyCharArray.Length;
            //int keyIndex = 0;

            //foreach (char c in inputCharArray)
            //{
            //    encryptedString += CharEncryptor.GetVigenerEncryptedChar(c, keyCharArray[keyIndex]);
            //    keyIndex++;
            //    if (keyIndex == keyLength)
            //    {
            //        keyIndex = 0;
            //    }
            //}

            //Console.WriteLine(encryptedString.ToString());

            var encryptedString = StringEncryptor.GetVigenerEncyptedString(inputString, keyString, false);
            var decryptedString = StringEncryptor.GetVigenerEncyptedString(encryptedString, keyString, true);

            Console.WriteLine(encryptedString);
            Console.WriteLine(decryptedString);
        }
    }
}