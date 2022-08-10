using System.Collections.Generic;

namespace L4660
{
    internal class Answer
    {
        public int Clue { get; set; }
        public int Prime { get; set; }
        public int Entry { get; set; }
        public char Letter { get; set; }

        private string clueString;

        public Answer()
        {
        }
        public Answer(int clue, int prime, int entry, char letter)
        {
            Clue = clue;
            Prime = prime;
            Entry = entry;
            Letter = letter;

            clueString = Clue.ToString();
        }

        public Answer(int clue, char letter=' ')
        {
            Clue = clue;
            Prime = 0;
            Entry = 0;
            Letter = letter;

            clueString = Clue.ToString();
        }

        public List<Answer> ExtractPrime()
        {
            var work = new HashSet<(int,int,int,char)>();
            var retval = new List<Answer>();

            for (int i = 0; i < clueString.Length - 1; i++)
            {
                int possPrime = int.Parse(clueString.Substring(i, 2));
                if (Program.isPrime(possPrime))
                {
                    string ent = "";
                    if (i > 0)
                        ent = clueString.Substring(0, i);
                    if (i + 2 < clueString.Length)
                        ent += clueString.Substring(i + 2);

                    if (int.Parse(ent) >= 10)
                        work.Add((Clue, possPrime, int.Parse(ent), Letter));
                }
            }
            foreach (var a in work)
                retval.Add(new Answer(a.Item1,a.Item2,a.Item3,a.Item4));

            return retval;
        }

        public override string ToString()
        {
            string str = Letter == ' ' ? " =" : $"{Letter}=";
            return $"({Clue,5} {str}{Prime} {Entry,3})";
        }
    }
}