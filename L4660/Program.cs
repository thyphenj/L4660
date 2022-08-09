using System;
using System.Collections.Generic;

namespace L4660
{
    internal class Program
    {
        static readonly double epsilon = 0.0001;
        static readonly int[] primes = { 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };

        private static void Main()
        {
            var usedPrimes = new HashSet<int>();

            // ---------------------------------------------------
            //Console.WriteLine("-- Reversible Primes --\n");

            var reversiblePrimes = new List<int>();
            foreach (int p in primes)
            {
                int q = (p % 10) * 10 + p / 10;
                if (p < q && isPrime(q))
                {
                    reversiblePrimes.Add(p);
                    reversiblePrimes.Add(q);
                }
            }

            // ---------------------------------------------------
            //Console.WriteLine("-- Reversible squares C, M, Q and Y\n");

            var possSquares = FindPairs(posSquar(reversiblePrimes));

            // -- we have 4 actual primes here - let's just make sure no-one else tries to use them

            foreach (var a in possSquares)
            {
                if (!usedPrimes.Contains(a.Prime))
                    usedPrimes.Add(a.Prime);
            }

            // ---------------------------------------------------
            //Console.WriteLine("-- ac15 (and dn10) for possible D L S\n");

            var ac15 = new Answer();
            int prime_D = 0;
            int prime_S = 0;

            foreach (int possD in primes)
            {
                int cube = possD * possD * possD;
                if (cube < 10000 || cube >= 100000) continue;

                usedPrimes.Add(possD);

                Answer tryThis = new Answer(cube, 'L');
                foreach (var poss_ac15 in tryThis.ExtractPrime())
                {
                    double possS = Math.Sqrt((double)poss_ac15.Entry);
                    if (possS - Math.Floor(possS) > epsilon) continue;

                    // -- Compare and contrast with last digit of [10dn] which must be '3' or more due to ascending digits

                    if (poss_ac15.Entry.ToString()[1] < '4') continue;

                    // -- There is only ONE set of solutions here!

                    ac15 = poss_ac15;
                    prime_D = possD;
                    prime_S = (int)possS;
                }
                usedPrimes.Remove(possD);
            }
            usedPrimes.Add(prime_S);
            usedPrimes.Add(prime_D);
            usedPrimes.Add((int)ac15.Prime);

            Console.WriteLine($"15ac {ac15}  D={prime_D}  S={prime_S}");

            // ---------------------------------------------------
            //Console.WriteLine("-- dn15 has to start with the same as ac15");

            var dn15 = new Answer();

            foreach (var a in possSquares)
            {
                if (a.Entry.ToString()[0] == ac15.Entry.ToString()[0])
                {
                    dn15 = a;
                    dn15.Letter = 'Y';
                }
            }
            usedPrimes.Add(dn15.Prime);
            Console.WriteLine($"15dn {dn15}");

            // ---------------------------------------------------
            //Console.WriteLine("-- dn03 - reverse of Y\n");

            var dn03 = new Answer();

            int rev = dn15.Prime;
            rev = rev / 10 + (rev % 10) * 10;

            foreach (var a in possSquares)
            {
                if (a.Prime == rev)
                {
                    dn03 = a;
                    dn03.Letter = 'Q';
                }
            }
            usedPrimes.Add(dn03.Prime);
            Console.WriteLine($" 3dn {dn03}");

            // ---------------------------------------------------
            //Console.WriteLine("-- ac17 - reverse of [3dn]\n");

            var ac17 = new Answer();

            rev = dn03.Entry;
            rev = rev / 10 + (rev % 10) * 10;

            foreach (var a in possSquares)
            {
                if (a.Entry == rev)
                {
                    ac17 = a;
                    ac17.Letter = 'M';
                }
            }
            usedPrimes.Add(ac17.Prime);
            Console.WriteLine($"17ac {ac17}");

            // ---------------------------------------------------
            //Console.WriteLine("-- ac05 - reverse of M\n");

            var ac05 = new Answer();

            rev = ac17.Prime;
            rev = rev / 10 + (rev % 10) * 10;

            foreach (var a in possSquares)
            {
                if (a.Prime == rev)
                {
                    ac05 = a;
                    ac05.Letter = 'C';
                }
            }
            usedPrimes.Add(ac05.Prime);
            Console.WriteLine($" 5ac {ac05}");

            // ---------------------------------------------------
            //Console.WriteLine("-- dn06 - palindrome containing S\n");

            var dn06 = new Answer();
            int prime_K = 0;
            int prime_T = 0;

            for (int i = 0; i < primes.Length; i++)
            {
                if (usedPrimes.Contains(primes[i])) continue;
                for (int j = i + 1; j < primes.Length; j++)
                {
                    if (usedPrimes.Contains(primes[j])) continue;

                    if (primes[i] / 10 <= primes[i] % 10 && primes[j] / 10 <= primes[j] % 10) continue;

                    string KT = (primes[i] * primes[j]).ToString();

                    var guess = $"{prime_S}{KT}";
                    if (guess.Length > 5) continue;
                    if (guess[0] == guess[4] && guess[1] == guess[3])
                    {
                        var poss_dn06 = new Answer(int.Parse(guess), 'S');
                        foreach (var an in poss_dn06.ExtractPrime())
                        {
                            if (an.Prime != prime_S) continue;
                            //Console.WriteLine($"dn06 {an}  KT={primes[i]}/{primes[j]}");
                            dn06 = new Answer(an.Clue, an.Prime, an.Entry, 'S');
                            prime_K = primes[i];
                            prime_T = primes[j];
                        }
                    }

                    guess = $"{KT[0]}{prime_S}{KT[1]}{KT[2]}";
                    if (guess[0] == guess[4] && guess[1] == guess[3])
                    {
                        var poss_dn06 = new Answer(int.Parse(guess), 'S');
                        foreach (var an in poss_dn06.ExtractPrime())
                        {
                            if (an.Prime != prime_S) continue;
                            //Console.WriteLine($"dn06 {an}  KT={primes[i]}/{primes[j]}");
                        }
                    }

                    guess = $"{KT[0]}{KT[1]}{prime_S}{KT[2]}";
                    if (guess[0] == guess[4] && guess[1] == guess[3])
                    {
                        var poss_dn06 = new Answer(int.Parse(guess), 'S');
                        foreach (var an in poss_dn06.ExtractPrime())
                        {
                            if (an.Prime != prime_S) continue;
                            //Console.WriteLine($"dn06 {an}  KT={primes[i]}/{primes[j]}");
                        }
                    }

                    guess = $"{KT}{prime_S}";
                    if (guess[0] == guess[4] && guess[1] == guess[3])
                    {
                        var poss_dn06 = new Answer(int.Parse(guess), 'S');
                        foreach (var an in poss_dn06.ExtractPrime())
                        {
                            if (an.Prime != prime_S) continue;
                            //Console.WriteLine($"06dn {an}  KT={primes[i]}/{primes[j]}");
                        }
                    }
                }
            }
            usedPrimes.Add(prime_T);
            usedPrimes.Add(prime_K);
            usedPrimes.Add((int)dn06.Prime);

            Console.WriteLine($" 6dn {dn06}  K={prime_K}  T={prime_T}");
            Console.WriteLine();

            // ---------------------------------------------------
            Console.WriteLine("-- 11ac & 11dn - possible G & W\n");

            var poss_dn11 = new List<Answer>();

            for (int i = 1; i < 6; i++)
                for (int j = i + 1; j < 7; j++)
                    for (int k = j + 1; k < 8; k++)
                        for (int l = k + 1; l < 9; l++)
                            for (int m = l + 1; m < 10; m++)
                            {
                                var num = new Answer(i * 10000 + j * 1000 + k * 100 + l * 10 + m, 'W');
                                foreach (var d11 in num.ExtractPrime())
                                {
                                    if (d11.Entry % 10 != ac17.Entry / 10) continue;
                                    if (Array.IndexOf(primes, d11.Entry - d11.Prime) < 0) continue;

                                    poss_dn11.Add(d11);
                                }
                            }
            for (int i = 1; i < 7; i++)
                for (int j = i + 1; j < 8; j++)
                    for (int k = j + 1; k < 9; k++)
                        for (int l = k + 1; l < 10; l++)
                        {
                            var num = new Answer(i * 1000 + j * 100 + k * 10 + l, 'G');
                            foreach (var a11 in num.ExtractPrime())
                            {
                                foreach (var d11 in poss_dn11)
                                {
                                    if (d11.Entry / 100 != a11.Entry / 10) continue;
                                    if (d11.Prime + a11.Prime != d11.Entry) continue;
                                    if (d11.Entry % a11.Entry != 0) continue;

                                    Console.WriteLine($"11ac={a11} 11dn={d11}");
                                }
                            }
                        }

            Console.WriteLine();


            // ---------------------------------------------------
            // ---------------------------------------------------
            Console.WriteLine("-- Possible H --\n");

            var triangles = new Triangle();
            foreach (int num in triangles.HiNums)
            {
                var possH = new Answer(num, 'H');
                foreach (var a in possH.ExtractPrime())
                {
                    if (usedPrimes.Contains(a.Prime)) continue;
                    if (!triangles.LoNums.Contains(a.Entry)) continue;

                    Console.WriteLine($"12ac {a}");
                }
            }
            Console.WriteLine();

            // ---------------------------------------------------
            Console.WriteLine("-- dn10 (and ac15) for possible V\n");

            for (int i = 1; i < 6; i++)
                for (int j = i + 1; j < 7; j++)
                    for (int k = j + 1; k < 8; k++)
                        for (int l = k + 1; l < 9; l++)
                            for (int m = l + 1; m < 10; m++)
                            {
                                int num = i * 10000 + j * 1000 + k * 100 + l * 10 + m;
                                int digitsum = i + j + k + l + m;

                                if (num % digitsum == 0)
                                {
                                    foreach (var possV in new Answer((int)num, 'V').ExtractPrime())
                                    {
                                        if (usedPrimes.Contains(possV.Prime)) continue;
                                        if (possV.Entry.ToString()[2] != ac15.Entry.ToString()[1]) continue;

                                        Console.WriteLine($"10dn {possV}");
                                    }
                                }
                            }
            Console.WriteLine();

            // ---------------------------------------------------
            Console.WriteLine("--Possible P Z --\n");

            foreach (var possZ in primes)
            {
                if (usedPrimes.Contains(possZ)) continue;
                for (int i = 0; i < primes.Length; i++)
                {
                    if (possZ == primes[i]) continue;
                    if (usedPrimes.Contains(primes[i])) continue;

                    int tri = possZ + primes[i];
                    if (!triangles.LoNums.Contains(tri) && !triangles.MdNums.Contains(tri)) continue;


                    Console.WriteLine($"Z {possZ}   P {primes[i]}   tri {tri}");
                }
            }

            Console.WriteLine();
        }

        internal static bool isPrime(int n)
        {
            if (n % 2 == 0 || n % 5 == 0)
                return false;

            foreach (int a in primes)
            {
                if (a == n)
                    return true;
            }
            return false;
        }
        internal static List<Answer> posSquar(List<int> reversiblePrimes)
        {
            var retval = new List<Answer>();

            for (int i = 32; i * i < 10000; i++)
            {
                Answer tryThis = new Answer(i * i);
                foreach (var a in tryThis.ExtractPrime())
                {
                    if (reversiblePrimes.Contains(a.Prime))
                        retval.Add(a);
                }
            }
            return retval;
        }
        internal static List<Answer> FindPairs(List<Answer> possA)
        {
            var work = new HashSet<Answer>();
            var retval = new List<Answer>();

            for (int i = 0; i < possA.Count; i++)
            {
                int rev = possA[i].Prime;
                rev = (rev % 10) * 10 + rev / 10;
                for (int j = i + 1; j < possA.Count; j++)
                {
                    if (possA[j].Prime == rev)
                    {
                        work.Add(possA[i]);
                        work.Add(possA[j]);
                    }
                }
            }
            foreach (var a in work)
                retval.Add(a);

            return retval;
        }
    }
}