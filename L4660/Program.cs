using System;
using System.Collections.Generic;

namespace L4660
{
    internal class Program
    {
        static readonly int[] primes = { 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };

        private static void Main()
        {
            int entry;

            var usedPrimes = new HashSet<int>();
            var unusedPrimes = new HashSet<int>();
            var usedEntries = new HashSet<int>();
            var reversiblePrimes = new List<int>();

            // ---------------------------------------------------
            // -- INIT !!

            var triangles = new Triangle();

            foreach (int p in primes)
            {
                int q = reverseTwoDigitNumber(p);
                if (p < q && isPrime(q))
                {
                    reversiblePrimes.Add(p);
                    reversiblePrimes.Add(q);
                }
            }

            // ---------------------------------------------------
            //Console.WriteLine("-- Squares containing reversible primes C, M, Q and Y\n");

            var possSquares = FindPairs(reversiblePrimes);

            // -- we have 5 squares and 4 prime values here - let's just make sure no-one else tries to use them

            foreach (var a in possSquares)
            {
                usedPrimes.Add(a.Prime);
            }

            // ---------------------------------------------------
            //Console.WriteLine("-- ac15 (and dn10) for possible D L S\n");

            var ac15 = new Answer();
            int prime_D = 0;
            int prime_S = 0;

            foreach (int poss_D in primes)
            {
                int cube = poss_D * poss_D * poss_D;
                if (cube < 10000 || cube >= 100000) continue;

                foreach (var poss_ac15 in new Answer(cube, 'L').ExtractPrime())
                {
                    int poss_S = (int)Math.Sqrt((double)poss_ac15.Entry);
                    if (poss_S * poss_S != poss_ac15.Entry) continue;

                    // -- Compare and contrast with last digit of [10dn] which must be '3' or more due to ascending digits

                    if (digitAtPosition(poss_ac15.Entry, 1) < 4) continue;

                    // -- There is only ONE set of solutions here!

                    ac15 = poss_ac15;
                    prime_D = poss_D;
                    prime_S = poss_S;
                }
            }
            usedPrimes.Add(ac15.Prime);
            usedEntries.Add(ac15.Entry);

            usedPrimes.Add(prime_D);
            usedPrimes.Add(prime_S);

            // ---------------------------------------------------
            //Console.WriteLine("-- dn15 has to start with the same as ac15");

            var dn15 = new Answer();

            foreach (var poss_dn15 in possSquares)
            {
                if (digitAtPosition(poss_dn15.Entry, 0) == digitAtPosition(ac15.Entry, 0))
                {
                    dn15 = poss_dn15;
                    dn15.Letter = 'Y';
                }
            }
            usedPrimes.Add(dn15.Prime);
            usedEntries.Add(dn15.Entry);

            // ---------------------------------------------------
            //Console.WriteLine("-- dn03 - reverse of Y\n");

            var dn03 = new Answer();

            int poss_Q = reverseTwoDigitNumber(dn15.Prime);

            foreach (var poss_dn03 in possSquares)
            {
                if (poss_dn03.Prime != poss_Q) continue;

                dn03 = poss_dn03;
                dn03.Letter = 'Q';
            }
            usedPrimes.Add(dn03.Prime);
            usedEntries.Add(dn03.Entry);

            // ---------------------------------------------------
            //Console.WriteLine("-- ac17 - reverse of [3dn]\n");

            var ac17 = new Answer();

            int poss_ac17Entry = reverseTwoDigitNumber(dn03.Entry);

            foreach (var poss_ac17 in possSquares)
            {
                if (poss_ac17.Entry != poss_ac17Entry) continue;

                ac17 = poss_ac17;
                ac17.Letter = 'M';
            }
            usedPrimes.Add(ac17.Prime);
            usedEntries.Add(ac17.Entry);

            // ---------------------------------------------------
            //Console.WriteLine("-- ac05 - reverse of M\n");

            var ac05 = new Answer();

            int poss_C = reverseTwoDigitNumber(ac17.Prime);

            foreach (var poss_ac05 in possSquares)
            {
                if (poss_ac05.Prime != poss_C) continue;

                ac05 = poss_ac05;
                ac05.Letter = 'C';
            }
            usedPrimes.Add(ac05.Prime);
            usedEntries.Add(ac05.Entry);

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

                    int p1 = primes[i];
                    int p2 = primes[j];

                    // -- at least one of these should have ascending digits
                    if (p1 / 10 <= p1 % 10 && p2 / 10 <= p2 % 10) continue;

                    //int poss_KT = (p1*p2);

                    if (p1 * p2 > 999) continue;

                    foreach (var poss_dn06 in insertAnswer(prime_S, p1 * p2, 'S'))
                    {
                        if (digitAtPosition(poss_dn06.Clue, 0) != digitAtPosition(poss_dn06.Clue, 4)) continue;
                        if (digitAtPosition(poss_dn06.Clue, 1) != digitAtPosition(poss_dn06.Clue, 3)) continue;

                        if (poss_dn06.Prime != prime_S) continue;

                        dn06 = poss_dn06;
                        prime_K = p1;
                        prime_T = p2;       //-- lucky correct!
                    }
                }
            }
            usedPrimes.Add(prime_T);
            usedPrimes.Add(prime_K);
            usedPrimes.Add(dn06.Prime);

            usedEntries.Add(dn06.Entry);

            // ---------------------------------------------------
            //Console.WriteLine("--Possible P Z --\n");

            var dn01 = new Answer();
            var dn16 = new Answer();

            foreach (var poss_Z in primes)
            {
                if (usedPrimes.Contains(poss_Z)) continue;

                for (int i = 0; i < primes.Length; i++)
                {
                    var poss_P = primes[i];
                    if (poss_Z == poss_P) continue;
                    if (usedPrimes.Contains(poss_P)) continue;

                    int tri = poss_Z + poss_P;
                    if (!triangles.LoNums.Contains(tri) && !triangles.MdNums.Contains(tri)) continue;

                    for (int j = 10; j * poss_Z < 10000; j++)
                    {
                        if (j * poss_Z < 1000) continue;

                        foreach (var poss_dn16 in new Answer(j * poss_Z, 'Z').ExtractPrime())
                        {
                            if (poss_dn16.Prime != poss_Z) continue;
                            if (usedEntries.Contains(poss_dn16.Entry)) continue;
                            if (digitAtPosition(poss_dn16.Entry, 0) != digitAtPosition(ac15.Entry, 2)) continue;

                            for (int k = 10; k * poss_P < 10000; k++)
                            {
                                if (k * poss_P < 1000) continue;
                                foreach (var poss_dn01 in new Answer(k * poss_P, 'P').ExtractPrime())
                                {
                                    if (poss_dn01.Prime != poss_P) continue;
                                    if (usedEntries.Contains(poss_dn01.Entry)) continue;
                                    var root = (int)Math.Sqrt(poss_dn16.Entry + poss_dn01.Entry);
                                    if (poss_dn16.Entry + poss_dn01.Entry - root * root != 0) continue;

                                    dn01 = poss_dn01;
                                    dn16 = poss_dn16;
                                }
                            }
                        }
                    }
                }
            }
            usedPrimes.Add(dn01.Prime);
            usedPrimes.Add(dn16.Prime);

            usedEntries.Add(dn01.Entry);
            usedEntries.Add(dn16.Entry);

            // ---------------------------------------------------
            // -- We should be able to fix 7ac now

            var ac07 = new Answer();

            entry = 100 * digitAtPosition(dn01.Entry, 1) + digitAtPosition(dn03.Entry, 1);

            for (int i = 1; i < 10; i++)
            {
                entry += 10;

                if (entry % prime_D != 0) continue;

                foreach (var poss_ac07 in insertAnswer(prime_D, entry, 'D'))
                {
                    if (poss_ac07.Prime != prime_D) continue;

                    ac07 = poss_ac07;
                }
            }
            usedPrimes.Add(ac07.Prime);
            usedEntries.Add(ac07.Entry);

            // ---------------------------------------------------
            //Console.WriteLine("-- Possible H --\n");

            var ac12 = new Answer();

            foreach (int tri in triangles.HiNums)
            {
                foreach (var poss_ac12 in new Answer(tri, 'H').ExtractPrime())
                {
                    if (usedPrimes.Contains(poss_ac12.Prime)) continue;
                    if (!triangles.LoNums.Contains(poss_ac12.Entry)) continue;

                    ac12 = poss_ac12;
                }
            }
            usedPrimes.Add(ac12.Prime);
            usedEntries.Add(ac12.Entry);

            // ---------------------------------------------------
            // -- 8dn has descending digits and ends in T=61

            var dn08 = new Answer();

            int digit = digitAtPosition(prime_T, 0);
            foreach (var poss_dn08 in new Answer(prime_T + (digit + 1) * 100 + (digit + 2) * 1000 + (digit + 3) * 10000, 'T').ExtractPrime())
            {
                if (poss_dn08.Prime != prime_T) continue;

                dn08 = poss_dn08;
            }
            usedPrimes.Add(dn08.Prime);

            usedEntries.Add(dn08.Entry);

            // ---------------------------------------------------
            // -- let's look at 14ac

            var ac14 = new Answer();

            foreach (var tri in triangles.HiNums)
            {
                string str = tri.ToString();

                if (str.IndexOf($"{prime_K}") < 0) continue;
                int start = str.IndexOf($"{dn08.Entry % 10}");

                if (start < 0) continue;
                if (start == 1 || start == 3) continue;

                foreach (var poss_ac14 in new Answer(tri, 'K').ExtractPrime())
                {
                    if (poss_ac14.Prime != prime_K) continue;

                    ac14 = poss_ac14;
                }
            }
            usedPrimes.Add(ac14.Prime);

            usedEntries.Add(ac14.Entry);

            // ---------------------------------------------------
            //Console.WriteLine("-- 11ac & 11dn - possible G & W\n");

            var ac11 = new Answer();
            var dn11 = new Answer();

            var poss_11dn = new List<Answer>();

            for (int i = 1; i < 6; i++)
                for (int j = i + 1; j < 7; j++)
                    for (int k = j + 1; k < 8; k++)
                        for (int l = k + 1; l < 9; l++)
                            for (int m = l + 1; m < 10; m++)
                                foreach (var poss_d11 in new Answer(i * 10000 + j * 1000 + k * 100 + l * 10 + m, 'W').ExtractPrime())
                                {
                                    if (usedPrimes.Contains(poss_d11.Prime)) continue;
                                    if (digitAtPosition(poss_d11.Entry, 2) != digitAtPosition(ac17.Entry, 0)) continue;
                                    if (Array.IndexOf(primes, poss_d11.Entry - poss_d11.Prime) < 0) continue;

                                    poss_11dn.Add(poss_d11);
                                }

            for (int i = 1; i < 7; i++)
                for (int j = i + 1; j < 8; j++)
                    for (int k = j + 1; k < 9; k++)
                        for (int l = k + 1; l < 10; l++)
                            foreach (var poss_a11 in new Answer(i * 1000 + j * 100 + k * 10 + l, 'G').ExtractPrime())
                            {
                                if (usedPrimes.Contains(poss_a11.Prime)) continue;
                                if (poss_a11.Entry % 10 != (dn08.Entry / 10) % 10) continue;

                                foreach (var poss_d11 in poss_11dn)
                                {
                                    if (digitAtPosition(poss_d11.Entry, 0) != digitAtPosition(poss_a11.Entry, 0)) continue;
                                    if (poss_d11.Prime + poss_a11.Prime != poss_d11.Entry) continue;
                                    if (poss_d11.Entry % poss_a11.Entry != 0) continue;

                                    ac11 = poss_a11;
                                    dn11 = poss_d11;
                                }
                            }

            usedPrimes.Add(ac11.Prime);
            usedPrimes.Add(dn11.Prime);

            usedEntries.Add(ac11.Entry);
            usedEntries.Add(dn11.Entry);

            // ---------------------------------------------------
            //Console.WriteLine("------- 18ac");

            var ac18 = new Answer();

            for (int i = 100; i * ac12.Prime < 100000; i++)
            {
                int poss_N = i * ac12.Prime;
                if (poss_N < 10000) continue;

                foreach (var poss_ac18 in new Answer(poss_N, 'N').ExtractPrime())
                {
                    if (usedPrimes.Contains(poss_ac18.Prime)) continue;
                    if (poss_ac18.Entry % dn11.Prime != 0) continue;
                    if (poss_ac18.Clue % ac12.Prime != 0) continue;
                    if (digitAtPosition(poss_ac18.Entry, 1) != digitAtPosition(dn15.Entry, 1)) continue;

                    ac18 = poss_ac18;
                }
            }
            usedPrimes.Add(ac18.Prime);

            usedEntries.Add(ac18.Entry);

            // ---------------------------------------------------
            //Console.WriteLine("----- 12dn already populated - but let's check");

            var dn12 = new Answer();

            entry = digitAtPosition(ac12.Entry, 0) * 100 + digitAtPosition(ac14.Entry, 1) * 10 + digitAtPosition(ac18.Entry, 0);

            foreach (var poss_X in primes)
            {
                if (usedPrimes.Contains(poss_X)) continue;

                foreach (var a in new Answer(poss_X * 1000 + entry, 'X').ExtractPrime())
                {
                    if (a.Prime != poss_X) continue;
                    if (digitsum(a.Clue) != digitsum(ac18.Clue)) continue;

                    dn12 = a;
                }
            }
            usedPrimes.Add(dn12.Prime);

            usedEntries.Add(dn12.Entry);

            // ---------------------------------------------------
            //Console.WriteLine("-- dn10 (and ac15) for possible V\n");

            var dn10 = new Answer();

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
                                    foreach (var poss_d10 in new Answer(num, 'V').ExtractPrime())
                                    {
                                        if (usedPrimes.Contains(poss_d10.Prime)) continue;
                                        if (digitAtPosition(poss_d10.Entry, 2) != digitAtPosition(ac15.Entry, 1)) continue;

                                        dn10 = poss_d10;
                                    }
                                }
                            }

            usedPrimes.Add(dn10.Prime);

            usedEntries.Add(dn10.Entry);

            // ---------------------------------------------------

            foreach (var p in primes)
                if (!usedPrimes.Contains(p))
                    unusedPrimes.Add(p);

            // ---------------------------------------------------
            //Console.WriteLine("-- 9ac & F & R\n");

            var ac09 = new Answer();

            foreach (var ans in usedEntries)
            {
                if (ans > 99) continue;
                if (digitAtPosition(ans, 0) != digitAtPosition(dn10.Entry, 0)) continue;

                entry = reverseTwoDigitNumber(ans);

                foreach (var poss_F in unusedPrimes)
                {
                    foreach (var poss_ac09 in insertAnswer(poss_F, entry, 'F'))
                    {
                        if (poss_ac09.Prime != poss_F) continue;

                        foreach (var q in primes)
                        {
                            if (q == poss_F) continue;
                            if (usedPrimes.Contains(q)) continue;
                            if (poss_ac09.Clue % q != 0) continue;

                            ac09 = poss_ac09;
                        }
                    }
                }
            }
            usedPrimes.Add(ac09.Prime);

            unusedPrimes.Remove(ac09.Prime);

            // ---------------------------------------------------
            //Console.WriteLine("-- We actually have 13ac entry resolved, just not sure what the prime/clue are\n");

            var ac13 = new Answer();

            entry = ((dn10.Entry / 10) % 10) * 10 + dn06.Entry % 10;

            foreach (var p in unusedPrimes)
            {
                foreach (var poss_ac13 in insertAnswer(p, entry, 'J'))
                    if (isPrime(poss_ac13.Clue))
                        ac13 = poss_ac13;
            }
            usedPrimes.Add(ac13.Prime);
            unusedPrimes.Remove(ac13.Prime);

            // ---------------------------------------------------
            //Console.WriteLine("-- We can whittle B down to a choice of one prime\n");

            int prime_B = 0;
            var poss_ac02 = new List<Answer>();

            int midDigit = digitAtPosition(dn03.Entry, 0);

            foreach (var p in unusedPrimes)
                for (int i = 1; i < 10; i++)
                    for (int j = 1; j < 10; j++)
                    {
                        entry = i * 100 + midDigit * 10 + j;

                        for (int k = 10; k * entry < 100000; k++)
                            foreach (var poss in new Answer(k * entry, 'B').ExtractPrime())
                            {
                                if (poss.Prime != p) continue;
                                if (poss.Entry != entry) continue;

                                poss_ac02.Add(poss);
                                prime_B = poss.Prime;
                            }
                    }
            usedPrimes.Add(prime_B);
            unusedPrimes.Remove(prime_B);

            int prime_R = 0;
            foreach (int poss_R in unusedPrimes)
                prime_R = poss_R;

            // ---------------------------------------------------
            //Console.WriteLine("-- 4 down");

            var dn04 = new Answer();

            for ( int i = 1; i < 10; i++)
            {
                entry = i * 100 + 60;
                foreach ( var poss_dn04 in insertAnswer(prime_R,entry,'R'))
                {
                    if (poss_dn04.Clue % prime_B != 0) continue;
                    dn04 = poss_dn04;
                }
            }

            // ---------------------------------------------------

            var ac02 = new Answer();

            foreach ( var poss in poss_ac02)
            {
                if (digitAtPosition(poss.Entry, 2) != digitAtPosition(dn04.Entry, 0)) continue;

                ac02 = poss;
            }

            // ---------------------------------------------------

            Console.WriteLine("---------------------------------------");
            Console.WriteLine($" 2ac {ac02}");
            Console.WriteLine($" 5ac {ac05}");
            Console.WriteLine($" 7ac {ac07}");
            Console.WriteLine($" 9ac {ac09}");
            Console.WriteLine($"11ac {ac11}");
            Console.WriteLine($"12ac {ac12}");
            Console.WriteLine($"13ac {ac13}");
            Console.WriteLine($"14ac {ac14}");
            Console.WriteLine($"15ac {ac15}");
            Console.WriteLine($"17ac {ac17}");
            Console.WriteLine($"18ac {ac18}");
            Console.WriteLine();
            Console.WriteLine($" 1dn {dn01}");
            Console.WriteLine($" 3dn {dn03}");
            Console.WriteLine($" 4dn {dn04}");
            Console.WriteLine($" 6dn {dn06}");
            Console.WriteLine($" 8dn {dn08}");
            Console.WriteLine($"10dn {dn10}");
            Console.WriteLine($"11dn {dn11}");
            Console.WriteLine($"12dn {dn12}");
            Console.WriteLine($"15dn {dn15}");
            Console.WriteLine($"16dn {dn16}");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();

        }

        // -----------------------------------------------------------------------------------------------------------------------
        private static int reverseTwoDigitNumber(int num)
        {
            return num / 10 + 10 * (num % 10);
        }

        private static int digitAtPosition(int num, int pos)
        {
            return int.Parse($"{num.ToString()[pos]}");
        }

        private static List<Answer> insertAnswer(int p, int ent, char c)
        {
            List<Answer> retval = new List<Answer>();

            retval.Add(new Answer(int.Parse($"{p}{ent}"), p, ent, c));
            retval.Add(new Answer(int.Parse($"{ent}{p}"), p, ent, c));
            retval.Add(new Answer(int.Parse($"{ent / 10}{p}{ent % 10}"), p, ent, c));

            if (ent > 99)
                retval.Add(new Answer(int.Parse($"{ent / 100}{p}{ent % 100}"), p, ent, c));
            return retval;
        }

        private static int digitsum(int clue)
        {
            int retval = 0;
            while (clue > 0)
            {
                retval = retval + clue % 10;
                clue = clue / 10;
            }
            return retval;
        }

        internal static bool isPrime(int n)
        {
            if (n % 2 == 0 || n % 3 == 0 || n % 5 == 0 || n % 7 == 0)
                return false;

            foreach (int a in primes)
            {
                if (n != a && n % a == 0)
                    return false;
            }
            return true;
        }

        internal static List<Answer> FindPairs(List<int> reversiblePrimes)
        {
            var retval = new List<Answer>();

            var pairedSquares = new HashSet<Answer>();
            var allSquares = new List<Answer>();

            // -- only look at 4-digit squares
            for (int i = 32; i * i < 10000; i++)
            {
                foreach (var a in new Answer(i * i).ExtractPrime())
                {
                    if (reversiblePrimes.Contains(a.Prime))
                        allSquares.Add(a);
                }
            }

            // -- now find the ones that form pairs
            for (int i = 0; i < allSquares.Count; i++)
            {
                int revere = (allSquares[i].Prime % 10) * 10 + allSquares[i].Prime / 10;

                for (int j = i + 1; j < allSquares.Count; j++)
                {
                    if (allSquares[j].Prime == revere)
                    {
                        pairedSquares.Add(allSquares[i]);
                        pairedSquares.Add(allSquares[j]);
                    }
                }
            }
            foreach (var a in pairedSquares)
                retval.Add(a);

            return retval;
        }
    }
}