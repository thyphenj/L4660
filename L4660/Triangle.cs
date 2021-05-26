using System;
using System.Collections.Generic;
using System.Text;

namespace L4660
{
    class Triangle
    {
        public List<int> LoNums;
        public List<int> HiNums;
        public List<int> MdNums;

        public Triangle()
        {
            LoNums = new List<int>();
            HiNums = new List<int>();
            MdNums = new List<int>();
            int i = 1;
            int nxtTry = i * (i + 1) / 2;
            while (nxtTry < 10000)
            {
                if (nxtTry < 100)
                    LoNums.Add(nxtTry);
                else if (nxtTry < 1000)
                    MdNums.Add(nxtTry);
                else
                    HiNums.Add(nxtTry);
                i++;
                nxtTry = i * (i + 1) / 2;
            }
        }

    }
}
