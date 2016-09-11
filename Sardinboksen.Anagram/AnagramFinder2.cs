using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sardinboksen.Anagram
{
    public class AnagramFinder2
    {
        //Func<int, double> myFunc = new Func<int, double>(CalculateSomething);

        public Func<string, int> GetByteSize(string word) 
        {
            return (w) => w.Length;
        }
                
        public Func<int> GetByteIndex() 
        {
            return () => 1;
        }
                 
        public Func<string, int> CreateBitInput(string word)
        {
            
            int[] h = { 1, 3, 7, 15, 31, 63, 127, 255 };
            var l = 0;
            var c = 6;

            //GetByteSize(b);

            //ReadBoolean(word, c, l);

            return (w) => 1;
        }

        //public delegate bool CreateBitInput(string word);
        //public delegate int GetByteSize(string word);

        //public void createBitInput() { }

        public void DataToIntArray() 
        {
            CreateBitInput("test");
        }
        
        public void FindAnagram(){}
    }

    public class createBitInput
    {
        int[] h = { 1, 3, 7, 15, 31, 63, 127, 255 };
        int l = 0;
        int c = 6;
    }
}
