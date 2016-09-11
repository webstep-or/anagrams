using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sardinboksen.Anagram
{

    

    public class strings : List<string>
    {
    }
    public class anagrams : List<strings>
    {
    }

    // callback functions to indicate progress.
    public delegate void bottom_of_main_loop();
    public delegate void done_pruning(uint recursion_level, List<bag_and_anagrams> pruned);
    public delegate void found_anagram(strings words);

    // each entry is a bag followed by words that can be made from that bag.

    public class bag_and_anagrams : IComparable
    {
        public Bag b;
        public strings words;

        // *sigh* this is tediously verbose
        public bag_and_anagrams(Bag b, strings words)
        {
            this.b = b;
            this.words = words;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return this.b.CompareTo(((bag_and_anagrams)obj).b);
        }

        #endregion
    }

    public class Anagrams
    {

        // given a list of words and a list of anagrams, make more
        // anagrams by combining the two.
        private static anagrams combine(strings ws, anagrams ans)
        {
            anagrams rv = new anagrams();
            foreach (strings a in ans)
            {
                foreach (string word in ws)
                {
                    strings bigger_anagram = new strings();
                    bigger_anagram.InsertRange(0, a);
                    bigger_anagram.Add(word);
                    rv.Add(bigger_anagram);
                }
            }

            return rv;
        }

        // return a list that is like dictionary, but which contains only those items which can be made from the letters in bag.
        private static List<bag_and_anagrams> prune(Bag bag, List<bag_and_anagrams> dictionary, done_pruning done_pruning_callback, uint recursion_level)
        {
            List<bag_and_anagrams> rv = new List<bag_and_anagrams>();
            foreach (bag_and_anagrams pair in dictionary)
            {
                Bag this_bag = pair.b;
                if (bag.subtract(this_bag) != null)
                {
                    rv.Add(pair);
                }
            }
            done_pruning_callback(recursion_level, rv);
            return rv;
        }


        public static anagrams anagrams(Bag bag,
            List<bag_and_anagrams> dictionary,
            uint recursion_level,
            bottom_of_main_loop bottom,
            done_pruning done_pruning_callback,
            found_anagram success_callback)
        {
            anagrams rv = new anagrams();
            List<bag_and_anagrams> pruned = prune(bag,
                dictionary,
                done_pruning_callback,
                recursion_level);
            int pruned_initial_size = pruned.Count;
            while (pruned.Count > 0)
            {
                bag_and_anagrams entry = pruned[0];
                Bag this_bag = entry.b;
                Bag diff = bag.subtract(this_bag);
                if (diff != null)
                {
                    if (diff.empty())
                    {
                        foreach (string w in entry.words)
                        {
                            strings loner = new strings();
                            loner.Add(w);
                            rv.Add(loner);
                            if (recursion_level == 0)
                                success_callback(loner);
                        }
                    }
                    else
                    {
                        anagrams from_smaller = anagrams(diff, pruned, recursion_level + 1,
                            bottom,
                            done_pruning_callback,
                            success_callback);
                        anagrams combined = combine(entry.words, from_smaller);
                        foreach (strings an in combined)
                        {
                            rv.Add(an);
                            if (recursion_level == 0)
                                success_callback(an);
                        }
                    }
                }
                pruned.RemoveAt(0);
                if (recursion_level == 0)
                    bottom();

                //Application.DoEvents();
            }
            return rv;
        }

    }

    public class Bag : IComparable
    {
        static private string subtract_strings(string minuend, string subtrahend)
        {
            Bag m = new Bag(minuend);

            Bag s = new Bag(subtrahend);
            Bag diff = m.subtract(s);
            if (diff == null) return null;
            return diff.AsString();
        }

        private string guts;
        public Bag(string s)
        {
            Char[] chars = s.ToLower().ToCharArray();
            Array.Sort(chars);
            Char[] letters = Array.FindAll<char>(chars, Char.IsLetter);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Insert(0, letters);
            guts = sb.ToString();
        }
        public bool empty()
        {
            return (guts.Length == 0);
        }
        public Bag subtract(Bag subtrahend)
        {
            string m = guts;
            string s = subtrahend.guts;
            string difference = "";

            while (true)
            {
                if (s.Length == 0) return new Bag(difference + m);
                if (m.Length == 0) return null;
                {
                    char s0 = s[0];
                    char m0 = m[0];
                    if (m0 > s0) return null;
                    if (m0 < s0)
                    {
                        m = m.Substring(1);
                        difference += m0;
                        continue;
                    }
                    System.Diagnostics.Trace.Assert(m0 == s0,
                        "internal error!  Aggggh");
                    m = m.Substring(1);
                    s = s.Substring(1);
                }
            }
        }
        private static void test_subtraction(string minuend, string subtrahend, string expected_difference)
        {
            string actual_difference = subtract_strings(minuend, subtrahend);
            System.Diagnostics.Trace.Assert(actual_difference == expected_difference,
                                             "Test failure: "
                                             + "Subtracting `" + subtrahend
                                             + "' from `" + minuend
                                             + "' yielded `" + actual_difference
                                             + "', but should have yielded `" + expected_difference + "'.");
        }
        public static void test()
        {
            test_subtraction("dog", "god", "");
            test_subtraction("ddog", "god", "d");
            test_subtraction("a", "b", null);
            Console.WriteLine("Bag tests all passed.");
        }

        public string AsString()
        {
            return guts;
        }
        public override int GetHashCode()
        {
            return guts.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return guts.Equals(((Bag)obj).guts);
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            Bag other = (Bag)obj;
            if (this.guts.Length > other.guts.Length)
                return -1;
            else if (this.guts.Length < other.guts.Length)
                return 1;
            else
                return 0;
        }

        #endregion
    }
}
