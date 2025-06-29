namespace Algorithms.Library
{
    public class StringAlgorithms
    {
        public static string ReverseString(string input)
        {
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static int CountVowels(string input)
        {
            int count = 0;
            foreach (char c in input.ToLower())
            {
                if ("aiou".Contains(c))
                {
                    count++;
                }
            }
            return count;
        }

        public static bool IsPalindrome(string input)
        {
            string cleanedInput = new string(input.Where(char.IsLetterOrDigit).ToArray()).ToLower();
            int left = 0;
            int right = cleanedInput.Length - 1;
            while (left < right)
            {
                if (cleanedInput[left] != cleanedInput[right])
                {
                    return false;
                }
                left++;
                right--;
            }
            return true;
        }
    }
}
