using System.Collections.Generic;

using System;

namespace L4660
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> usedPrimes = new List<int>();

            // ---------------------------------------------------
            Console.WriteLine("-- Reversible Primes --\n");
            foreach (int p in Primes.PrimeList)
            {
                int q = (p % 10) * 10 + p / 10;
                if (p < q && Primes.isPrime(q))
                    Console.WriteLine($"{p} {q}");
            }
            Console.WriteLine();

            // ---------------------------------------------------
            Console.WriteLine("--Possible D L S --\n");

            List<int> possDlist = new List<int>();

            foreach (int possD in Primes.PrimeList)
            {
                int cube = possD * possD * possD;
                if (cube >= 10000 && cube < 100000)
                {
                    Answer tryThis = new Answer(cube);

                    foreach (var possL in tryThis.Possibles())
                    {
                        double root = Math.Sqrt((double)possL.Entry);
                        if (root - Math.Floor(root) < 0.001)
                        {
                            possDlist.Add(possD);
                            Console.WriteLine($"D {possD}   L {possL.Prime}   S {root}         15ac ({possL.Clue} / {possL.Entry})");
                        }
                    }
                }
            }
            Console.WriteLine();

            // ---------------------------------------------------
            Console.WriteLine("--Possible V --\n");
            for (int i = 1; i < 6; i++)
                for (int j = i + 1; j < 7; j++)
                    for (int k = j + 1; k < 8; k++)
                        for (int l = k + 1; l < 9; l++)
                            for (int m = l + 1; m < 10; m++)
                            {
                                int n = i * 10000 + j * 1000 + k * 100 + l * 10 + m;
                                if (n % (i + j + k + l + m) == 0)
                                {
                                    Answer tryThis = new Answer(n);
                                    foreach (var possV in tryThis.Possibles())
                                    {
                                        // 15ac entry is 121 or 289, so middle digit is 2 or 8, so 10dn (V) entry ends in 2, 8 or !!
                                        if (possV.Entry % 10 == 2 || possV.Entry % 10 == 8)
                                            Console.WriteLine(possV);
                                    }
                                }
                            }
            Console.WriteLine();

            // At this point we know entry 10dn (V) ends in 8 so 15ac = 289 and D, L and S are 29, 43 and 17 
            int D = 29;
            int L = 43;
            int S = 17;
            usedPrimes.Add(D);
            usedPrimes.Add(L);
            usedPrimes.Add(S);

            //{
            //    int fac = 10000 / D;
            //    int tryD = (fac + 1) * D;
            //    while (tryD < 100000)
            //    {
            //        Answer tryThis = new Answer(tryD);

            //        foreach (var poss in tryThis.Possibles())
            //        {
            //            if (poss.Prime == D
            //                && poss.Entry > 99
            //                && (poss.Entry / 10) % 10 > 2
            //                && poss.Entry % D == 0)
            //            {
            //                Console.WriteLine($"D {poss.Prime}           7ac ({poss.Clue} / {poss.Entry})");
            //            }
            //        }

            //        tryD += D;
            //    }
            //}

            Console.WriteLine();

            // ---------------------------------------------------
            Console.WriteLine("-- Possible H --\n");

            var triangles = new Triangle();
            foreach (int num in triangles.HiNums)
            {
                Answer tryH = new Answer(num);
                foreach (var a in tryH.Possibles())
                {
                    if (triangles.LoNums.Contains((int)a.Entry))
                        Console.WriteLine($"H {a.Prime}         12ac ({a.Clue} / {a.Entry})");
                }
            }
            Console.WriteLine();

            // ---------------------------------------------------
            Console.WriteLine("--Possible P Z --\n");
            foreach (var possZ in Primes.PrimeList)
            {
                for (int i = 0; i < Primes.PrimeList.Length; i++)
                {
                    if (possZ != Primes.PrimeList[i])
                    {
                        int tri = possZ + Primes.PrimeList[i];
                        if (triangles.LoNums.Contains(tri) || triangles.MdNums.Contains(tri))
                            Console.WriteLine($"Z {possZ}   P {Primes.PrimeList[i]}   tri {tri}");
                    }
                }
            }

            Console.WriteLine();
        }
    }
}
