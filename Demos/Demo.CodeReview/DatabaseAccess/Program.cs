namespace DatabaseAccess
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter course name: ");
            string courseName = Console.ReadLine();

            Console.WriteLine("Enter course description: ");
            string courseDescription = Console.ReadLine();

            Console.WriteLine("Enter course start date: ");
            string courseStartDate = Console.ReadLine();

            Console.WriteLine("Enter course end date: ");
            string courseEndDate = Console.ReadLine();

            Console.WriteLine("Hello, World!");
        }

        private static void PrintCourseDetails(string courseName, string courseDescription, string courseStartDate, string courseEndDate)
        {
        }
}
