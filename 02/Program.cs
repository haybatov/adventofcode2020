using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _02
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

            var policies = LoadPasswordPolicies(args[0]);

            int validPolicies = policies.Select(p => p.IsValid()).Where(r => r).Count();
            Console.WriteLine(validPolicies);

            int validNewPolicies = policies.Select(p => p.IsValid2()).Where(r => r).Count();
            Console.WriteLine(validNewPolicies);

        }

        private static IEnumerable<PasswordPolicy> LoadPasswordPolicies(string fileName)
        {
            Regex rx = new Regex(@"(\d+)-(\d+) (.): (.+)$");
            return File.ReadAllLines(fileName)
                .Select(s => rx.Match(s))
                .Select(m => new PasswordPolicy(minCount: m.Groups[1].Value, maxCount: m.Groups[2].Value, letter: m.Groups[3].Value, password: m.Groups[4].Value));
        }
    }

    internal class PasswordPolicy
    {
        private int minCount;
        private int maxCount;
        private char letter;
        private string password;

        public PasswordPolicy(string minCount, string maxCount, string letter, string password)
        {
            this.minCount = int.Parse(minCount);
            this.maxCount = int.Parse(maxCount);
            this.letter = letter[0];
            this.password = password;
        }

        public bool IsValid()
        {
            int k = 0;

            for (int i = 0; i < password.Length; i++)
                k += (password[i] == letter) ? 1 : 0;

            return k >= minCount && k <= maxCount;
        }

        public bool IsValid2()
        {
            return (minCount <= password.Length && password[minCount-1] == letter)
                ^ (maxCount <= password.Length && password[maxCount-1] == letter);
        }
    }
}
