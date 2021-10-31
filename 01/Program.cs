using System;
using System.IO;
using System.Linq;

namespace _01
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 || !File.Exists(args[0]))
            {
                Console.WriteLine("Specify an existing file as input.");
                return;
            }

            int[] input = File.ReadAllLines(args[0]).Select(int.Parse).ToArray();
            
            long result2 = FindPair(input, 2020);
            Console.WriteLine(result2);

            long result3 = FindTriplet(input, 2020);
            Console.WriteLine(result3);
        }

        private static long FindTriplet(int[] input, int sum)
        {
            Array.Sort(input);

            int len = input.Length;

            // Now diff contains a sum of 2 numbers from input. To find those numbers, we will find complements for input using every value in diff (sum2).
            // If the new complement contains a number from input, it is the third term (summand).
            // Obviously, by subtracting this third additive from the sum2, we will find the second term.
            // And subtracting sum2 from the original sum (2020), we will get the first term.
            int[] diff = GenerateComplements(input, sum, out var _);

            for (int i = len - 1; i >= 0; i--)
            {
                int[] diff2 = GenerateComplements(input, diff[i], out var diff2StartPosition);

                for (int k = diff2StartPosition, j = 0; k < len && j < len;)
                {
                    if (diff2[k] == input[j])
                        return (diff[i] - diff2[k])  * diff2[k] * (sum - diff[i]);
                    else
                        if (diff2[k] > input[j])
                        j++;
                    else
                        k++;
                }
            }

            return -1;
        }

        private static long FindPair(int[] input, int sum)
        {
            Array.Sort(input);
            
            int[] diff = GenerateComplements(input, sum, out var _);

            for (int i = 0, j = 0, len = input.Length; i < len && j < len;)
            {
                if (input[i] == diff[j])
                    return input[i] * (sum - input[i]);
                else
                    if (input[i] > diff[j])
                        j++;
                    else
                        i++;
            }

            return -1;
        }

        private static int[] GenerateComplements(int[] input, int sum, out int lastDiffPosition)
        {
            int len = input.Length;

            int[] diff = new int[len];

            int j = len - 1;
            // Calculate positive complements only; stop as soon as the complements become zero or negative.
            for (int i = 0; i < len && input[i] < sum; i++, j--)
                diff[j] = sum - input[i];
            
            lastDiffPosition = j + 1;
            return diff;
        }
    }
}
