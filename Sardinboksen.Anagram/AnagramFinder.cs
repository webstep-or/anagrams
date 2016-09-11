using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sardinboksen.Anagram
{
    public class AnagramFinder
    {
        public AnagramFinder()
        {
            Words = new List<string>();
        }
        
        public List<string> RegExFind(string word) 
        {
            if (string.IsNullOrEmpty(word)) 
            {
                throw new ArgumentNullException("word");
            }

            //^(?!.*a.*a)(?!.*c.*c)(?!.*n.*n)(?!.*e.*e)[acne]*$
            //var myRegex = new Regex(@"^(?!.*a.*a)(?!.*c.*c)(?!.*n.*n)(?!.*e.*e)[acne]*$");
            //^[acne]{4}$

            var test = word.ToList().Select(p => string.Format("(?!.*{{{0}}}.*{{{0}}})", p));
            var test2 = string.Concat(test);

            var minLength = string.Format("{{{0},{0}}}", word.Length); //{2,}
            //var maxLength = string.Format("{{{0}}}", 2);

            //var pattern = string.Format(@"^(?!.*(.).*\1)[{0}]{1}$", word, minLength);
            var pattern = string.Format(@"^{0}^[{1}]{2}$", test2, word, minLength);
            var myRegex = new Regex(pattern);
            var anagrams = Words.Where(p=> myRegex.IsMatch(p));

            return anagrams.Where(p => !p.Equals(word)).ToList();

            //return new List<string>()
            //{
            //    "test",
            //    "test",
            //    "test",
            //    "test",
            //    "test",
            //};
 
        }

        public List<string> LinqFind(string word) 
        {
            var anagrams = new List<string>();

            var wordIdx = string.Concat(word.ToLower().OrderBy(c => c));

            //var groups = from string w in Words
            //             group w by string.Concat(w.OrderBy(x => x)) into c
            //             group c by c.Count() into d
            //             orderby d.Key descending
            //             select d;

            //var groups = from string w in Words
            //             group w by string.Concat(w.OrderBy(x => x)) into c
            //             select c;
                         //group c by c.Key.Length into d
                         //orderby d.Key descending
                         //select d;
            var groups = Words
                .GroupBy(w => NewMethod(w, wordIdx.Length))
                .Where(p => p.Key != 0)
                .Select(p => new KeyValuePair<int, List<string>>(p.Key, p.ToList()))
                .Where(p => p.Key == wordIdx.GetHashCode())
                ;

            //var test = groups.FirstOrDefault(p => p.Key == wordIdx.Length);

            foreach (var group in groups)
            {
                anagrams.AddRange(group.Value);
            }


            //foreach (var c in groups.First())
            //{
            //    Console.WriteLine(string.Join(" ", c));
            //}
            

            return anagrams;

        }

        private static int NewMethod(string candidate, int expectedLength)
        {
            var candIdx = string.Concat(candidate.OrderBy(x => x));

            return candIdx.Length != expectedLength ? 0: candIdx.GetHashCode();
        }


        public List<string> Find(string word, int length)
        {
            var wordLength = word.Length;
            var wordArray = word.ToLower().OrderBy(c => c).ToArray();

            var wordCount = 2;

            var anagrams = new List<string>();
            
            foreach(var candidate in Words)
            {
                //if (candidate.Length > wordLength)
                //{
                //    continue;
                //}

                if (candidate.Length != length)
                {
                    continue;
                }

                var isAnagram = true;

                var position = 0;

                foreach (var letter in candidate.ToLower().OrderBy(c => c).ToArray()) 
                {
                    if (position >= wordLength)
                    {
                        isAnagram = false;

                        break;
                    }

                    while (letter != wordArray[position]) 
                    {
                        position++;

                        if (position == wordLength || letter < wordArray[position]) 
                        {
                            isAnagram = false;
                            break;
                        }
                    }

                    position++; 
                }

                if(isAnagram)
                {
                    anagrams.Add(candidate);
                }            
            }

            return anagrams;

        }

        public List<string> Words { get; set; }


        public IEnumerable<string> FindOne(string word)
        {
            var wordLength = word.Length;
            var wordArray = word.ToLower().OrderBy(c => c).ToArray();

            var anagrams = new List<string>();

            foreach (var candidate in Words)
            {
                //if (candidate.Length > wordLength)
                //{
                //    continue;
                //}

                if (candidate.Length != wordLength)
                {
                    continue;
                }

                var isAnagram = true;

                var position = 0;

                foreach (var letter in candidate.ToLower().OrderBy(c => c).ToArray())
                {
                    if (position >= wordLength)
                    {
                        isAnagram = false;

                        break;
                    }

                    while (letter != wordArray[position])
                    {
                        position++;

                        if (position == wordLength || letter < wordArray[position])
                        {
                            isAnagram = false;
                            break;
                        }
                    }

                    position++;
                }

                if (isAnagram)
                {
                    anagrams.Add(candidate);
                }
            }

            return anagrams.Where(p => !p.Equals(word));
        }
    }

    //working for one word result
    //public List<string> Find(string word)
    //    {
    //        var wordLength = word.Length;
    //        var wordArray = word.ToLower().OrderBy(c => c).ToArray();

    //        var anagrams = new List<string>();
            
    //        foreach(var candidate in Words)
    //        {
    //            //if (candidate.Length > wordLength)
    //            //{
    //            //    continue;
    //            //}

    //            if ( candidate.Length != wordLength)
    //            {
    //                continue;
    //            }

    //            var isAnagram = true;

    //            var position = 0;

    //            foreach (var letter in candidate.ToLower().OrderBy(c => c).ToArray()) 
    //            {
    //                if (position >= wordLength)
    //                {
    //                    isAnagram = false;

    //                    break;
    //                }

    //                while (letter != wordArray[position]) 
    //                {
    //                    position++;

    //                    if (position == wordLength || letter < wordArray[position]) 
    //                    {
    //                        isAnagram = false;
    //                        break;
    //                    }
    //                }

    //                position++; 
    //            }

    //            if(isAnagram)
    //            {
    //                anagrams.Add(candidate);
    //            }            
    //        }

    //        return anagrams.Where(p => !p.Equals(word)).ToList();

    //    }
}
