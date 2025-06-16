namespace Demo.Console
{
    internal class PerformanceAnalysis
    {
        internal static void Execute()
        {
            var startingString = "Hello, World!";
            var toAppend = " Welcome to performance analysis.";
            var iterations = 1000000;

            for (int i = 0; i < iterations; i++)
            {
                startingString += toAppend;
            }
        }
    }
}
