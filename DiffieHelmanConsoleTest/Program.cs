namespace InformationSecurityConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Alice и Bob открыто договорились
            int g = 2;
            int p = 3;

            Random rnd = new Random();

            // Alice и Bob загадали
            int a = rnd.Next(0, 100);
            int b = rnd.Next(0, 100);

            Console.WriteLine($"Alice a: {a}");
            Console.WriteLine($"Bob b: {b}");

            // Alice и Bob вычислили и обменялись
            int A = (int)(Math.Pow(g, a) % p);
            int B = (int)(Math.Pow(g, b) % p);

            // Alice и Bob вычислили
            int kAlice = (int)(Math.Pow(B, a) % p);
            int kBob = (int)(Math.Pow(A, b) % p);

            Console.WriteLine($"Alice K: {kAlice}");
            Console.WriteLine($"Bob K: {kBob}");
        }
    }
}