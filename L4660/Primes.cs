namespace L4660
{
    internal static class Primes
    {
        public static int[] PrimeList = { 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };

        public static int[] AscendingList = { 13, 17, 19, 23, 29, 37, 47, 59, 67, 79, 89, 97 };

        public static bool isPrime(int n)
        {
            if (n % 2 == 0 || n % 5 == 0)
                return false;

            foreach (int a in PrimeList)
            {
                if (a == n)
                    return true;
            }
            return false;
        }
    }
}