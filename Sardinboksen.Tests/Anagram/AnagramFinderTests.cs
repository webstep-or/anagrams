using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text.RegularExpressions;
using Sardinboksen.Anagram;

namespace Sardinboksen.Tests.Anagram
{
    /// <summary>
    /// Summary description for AnagramTests
    /// </summary>
    [TestClass]
    public class AnagramFinderTests
    {
        public AnagramFinderTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void AnagramFinder_EnglishFind()
        {
            var words = new List<string>();
            using (StreamReader sr = new StreamReader(@"Files\unixdict.txt", true))
            {
                words = Regex.Split(sr.ReadToEnd(), @"\r?\n").ToList();
            }

            var finder = new AnagramFinder()
            {
                Words = new List<string>(words)
            };

            var word = "alice";

            var anagrams = finder.Find(word, word.Length);

            Assert.IsTrue(anagrams.TrueForAll(p=> words.Contains(p)));

        }

        [TestMethod]
        public void AnagramFinder_Find()
        {            
            var words = new List<string>();
            
            words= TextFileParser.ParseFile(@"Files\fullform_bm.txt");

            var finder = new AnagramFinder()
            {
                Words = new List<string>(words)
            };

            var word = "regninger";

            var anagrams = finder.Find(word, word.Length);

            Assert.IsTrue(anagrams.TrueForAll(p => words.Contains(p)));

        }

        [TestMethod]
        public void AnagramFinder_LinqFind()
        {

            var words = new List<string>();
            
            words = TextFileParser.ParseFile(@"Files\fullform_bm.txt");

            var finder = new AnagramFinder()
            {
                Words = new List<string>(words)
            };

            var word = "regninger";

            var anagrams = finder.LinqFind(word);

            Assert.IsTrue(anagrams.TrueForAll(p => words.Contains(p)));

        }

        [TestMethod]
        public void AnagramFinder_MessyFind()
        {
            var words = new List<string>();

            words = TextFileParser.ParseFile(@"Files\fullform_bm.txt");
            
            var tester = new SampleTester(words);
            
            var word = "regninger";
            var anagrams = tester.Find(word);

            Assert.IsTrue(anagrams.TrueForAll(p => words.Contains(p)));
        }

        [TestMethod]
        public void AnagramFinder_IngesFind()
        {
            var test = new AnagramFinder2();

            test.DataToIntArray();
        }

        [TestMethod]
        public void Recursion_Run()
        {
            var words = new List<string>() 
            {
                "acne",
                "a",
                "cane",
                "ca",
                "en",
                "enc",
                "ac",
                "ne"
            };

            //words = TextFileParser.ParseFile(@"Files\fullform_bm.txt");
            

            var test = new Recursion(words);

            test.Run();
        }

        [TestMethod]
        public void Recursion_Run2()
        {
            var words = new List<string>() 
            {
                "acne",
                "a",
                "cane",
                "ca",
                "en",
                "enc",
                "ac",
                "ne"
            };

            var test = new Recursion(words);
            test.Run2();
        
        }
    }
}
