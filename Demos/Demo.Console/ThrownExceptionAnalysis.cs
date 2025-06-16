using System;

namespace Demo.Console
{
    internal class ThrownExceptionAnalysis
    {
        internal static void Execute()
        {
            var readLine = System.Console.ReadLine();
            var number = int.Parse(readLine ?? "0");
            bool isPrime = IsPrimeNumber(number);
            System.Console.WriteLine($"{number} is {(isPrime ? "a" : "not a")} prime number.");
        }

        private static bool IsPrimeNumber(int number)
        {
            if (number <= 1) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }
    }
}
