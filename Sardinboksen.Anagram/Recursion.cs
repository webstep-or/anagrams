using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sardinboksen.Anagram
{
    public class Recursion
    {
        AnagramFinder _finder;
        List<string> _anagrams = new List<string>();
        public Recursion(List<string> words)
        {
            _finder = new AnagramFinder() { Words = words };
        }
                
        public void Run() 
        {
            var word = "acne";

            List<char> lettersUsed = new List<char>();
            List<char> lettersLeft = new List<char>(word);

            RecursiveCount(lettersUsed, lettersLeft);

            Trace.WriteLine(_anagrams.Count);
        }

        public void Run2()
        {
            var word = "acen";

            var combos = Combo.GetCombinations(word.Length);

            foreach (var combo in combos.ToList()) 
            {
                var anagrams = FindFullAnagrams(word, combo).ToList();
                
                _anagrams.AddRange(anagrams); 

            }

            var msg = string.Format("{0} anagrams: {1}", _anagrams.Count, string.Join(",", _anagrams));

            Trace.WriteLine(msg);
        }
                

        IEnumerable<string> FindFullAnagrams(string s, IEnumerable<int> partition)
        {
            int partitionHead = partition.First();
            var partitionTail = partition.Skip(1);

            var headAnagrams = FindSubanagramsExact(s, partitionHead);
            //var headAnagrams = new List<string>();
            
            if (!partitionTail.Any())
            {
                foreach (string a in headAnagrams)
                    yield return a;
            }
            else
            {
                foreach (string headAnagram in headAnagrams)
                {
                    string remainder = SubtractString(s, headAnagram);

                    foreach (string anagram in FindFullAnagrams(remainder, partitionTail))
                    {
                        yield return string.Concat(headAnagram, " ", anagram);
                        //yield return string.Concat(anagram, " ", headAnagram);
                    }
                }
            }
        }

        public string SubtractString(string a, string b) 
        {
            //var temp = new string(a);
            char[] c = b.ToCharArray();
            Array.Sort(c);
            var s = new string(c);

            var test = a.Replace(s, string.Empty);
            return  test;
        }

        public List<string> FindSubanagramsExact(string s, int lenght) 
        {
            return _finder.Find(s, lenght); 
        }

        public List<int> GetNextPartition(List<int> p)
        {
            //mutates the list p to produce the next partition in
            // reverse lex order, first partition being a single number n, 
            // and the last one is n ones.
            int len = p.Count;
            //there is a tail of ones. count it and cut it off.
            int tailLen = 0;
            while (tailLen < len && p[len - tailLen - 1] == 1)
            {
                p.RemoveAt(len - tailLen - 1);
                tailLen++;
            }
            if (p.Count == 0) //all values were 1, this is the last partition
                return null;
            //all numbers in p are now > 1.
            //decrement the last (smallest) element;
            int cutValue = p[p.Count - 1] - 1;
            p[p.Count - 1] = cutValue;

            //now we have to add back all the 1s that we cut off
            int remainder = tailLen + 1;
            //append the 1s, clumped into groups of non-increasing size.
            //e.g. 3,1,1,1,1 becomes 2,2,2,1 (which in turn will become 2,2,1,1,1).
            while (remainder > 0)
            {
                if (remainder > cutValue)
                {
                    p.Add(cutValue);
                    remainder -= cutValue;
                }
                else
                {
                    p.Add(remainder);
                    remainder = 0;
                }
            }
            return p;
        }

        public int[] GetNextCombination(int n, int[] state)
        {
            int r = state.Length;
            int i = r - 1;
            while (i >= 0 && state[i] == n - r + i)
                i--;

            if (i < 0)
                return null;

            state[i]++;
            for (int j = i + 1; j < r; j++)
                state[j] = state[i] + j - i;

            return state;
        }

        public void RecursiveCount(List<char> lettersUsed, List<char> lettersLeft)
        {
            if (lettersUsed.Count > 0)
            {
                var word = new string(lettersUsed.ToArray());

                //look up word in dictionary
                Trace.WriteLine(word);


                //_anagrams.AddRange(_finder.LinqFind(word)); 

                var grams = _finder.LinqFind(word);
                Trace.WriteLine("anagrams: " +string.Join(",", grams));

               
            }
            if (lettersLeft.Count() > 0)
            {
                for (int index = 0; index < lettersLeft.Count(); index++)
                {
                    List<char> newLettersUsed = new List<char>(lettersUsed);
                    newLettersUsed.Add(lettersLeft[index]);
                    List<char> newLettersLeft = new List<char>(lettersLeft);
                    newLettersLeft.RemoveAt(index);
                    RecursiveCount(newLettersUsed, newLettersLeft);
                }
            }
        }
    }
}
