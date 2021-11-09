using System.Collections.Generic;

namespace L4660
{
    internal class Answer
    {
        public int Clue { get; set; }
        public int? Prime { get; set; }
        public int? Entry { get; set; }

        private string clueString;

        public Answer(int clue)
        {
            Clue = clue;
            clueString = Clue.ToString();
        }

        public Answer(int clue, int prime, int entry)
        {
            Clue = clue;
            Prime = prime;
            Entry = entry;
            clueString = Clue.ToString();
        }

        public List<Answer> Possibles()
        {
            var retval = new List<Answer>();

            for (int i = 0; i < clueString.Length - 1; i++)
            {
                int possPrime = int.Parse(clueString.Substring(i, 2));
                if (Primes.isPrime(possPrime))
                {
                    string ent = "";
                    if (i > 0)
                        ent = clueString.Substring(0, i);
                    if (i + 2 < clueString.Length)
                        ent += clueString.Substring(i + 2);

                    retval.Add(new Answer(Clue, possPrime, int.Parse(ent)));
                }
            }
            return retval;
        }

        public override string ToString()
        {
            return $"{Clue,6}  -  {Prime}  -  {Entry}";
        }
    }
}