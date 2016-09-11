using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sardinboksen.Anagram
{
    public class TextFileParser
    {
        public static List<string> ParseFile(string path) 
        {
            var words = File
                .ReadLines(path, encoding: Encoding.GetEncoding("iso-8859-1"))
                .SelectMany(GetWord)
                .Distinct()
                .ToList();

            return words;

            //foreach (var line in File.ReadLines(@"Files\fullform_bm.txt"))
            //{
            //    if (line.Contains('\t'))
            //    {
            //        var test = line.Split('\t');
            //    }
            //}
        }

        //private TResult test(string arg)
        //{
        //    throw new NotImplementedException();
        //}

        private static IEnumerable<string> GetWord(string line) 
        { 
            if (line.Contains('\t'))
            {
                var columns = line.Split('\t');

                yield return columns[1].ToLower();
                yield return columns[2].ToLower();
            }
        
        }
    }
}
