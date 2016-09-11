using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sardinboksen.Anagram
{
    public class SampleTester
    {
        List<bag_and_anagrams> _dictionary = new List<bag_and_anagrams>();

        public SampleTester(List<string> words)
        {

            //using (StreamReader sr = new StreamReader(wordlist_stream))
            foreach(var line in words)
            {
                //String line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                int linesRead = 0;
                Hashtable stringlists_by_bag = new Hashtable();
                //while ((line = sr.ReadLine()) != null)
                //{
                    // TODO -- filter out nonletters.  Thus "god's"
                    // should become "gods".  And since both of those
                    // are likely to appear, we need to ensure that we
                    // only store one.
                    //line = line.ToLower();
                    if (!acceptable(line))
                        continue;
                    Bag aBag = new Bag(line);
                    if (!stringlists_by_bag.ContainsKey(aBag))
                    {
                        strings l = new strings();
                        l.Add(line);
                        stringlists_by_bag.Add(aBag, l);
                    }
                    else
                    {
                        strings l = (strings)stringlists_by_bag[aBag];
                        if (!l.Contains(line))
                            l.Add(line);
                    }
                    linesRead++;
                    //ProgressBar.Increment(line.Length + 1); // the +1 is for the line ending character, I'd guess.

                    //Application.DoEvents();
                //}

                // Now convert the hash table, which isn't useful for
                // actually generating anagrams, into a list, which is.

                foreach (DictionaryEntry de in stringlists_by_bag)
                {
                    _dictionary.Add(new bag_and_anagrams((Bag)de.Key, (strings)de.Value));
                }
            }
        }

        public List<string> Find(string word) 
        {
            var results = new List<string>();

            Bag input_bag = new Bag(word);
            Anagrams.anagrams(input_bag, _dictionary, 0,
                delegate()
                {

                },
                delegate(uint recursion_level, List<bag_and_anagrams> pruned_dict)
                {

                },
                delegate(strings words)
                {
                    results.AddRange(words);
                });

            return results;
        }

        private bool acceptable(string s)
        {
            if (s.Length < 2)
            {
                if (s == "i" || s == "a")
                    return true;
                return false;
            }
            char[] vowels = { 'a', 'e', 'i', 'o', 'u', 'y' };
            if (s.IndexOfAny(vowels, 0) > -1)
                return true;
            return false;
        }
    }
}
