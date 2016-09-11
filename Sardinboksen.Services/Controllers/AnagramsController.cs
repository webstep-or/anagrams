using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Hosting;
using System.Web.Http;
using Sardinboksen.Anagram;
using Sardinboksen.Services.Models;

namespace Sardinboksen.Services.Controllers
{
    public class AnagramController : ApiController
    {
        // GET: api/Anagram
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //[Route("anagrams/{word}")]
        public IHttpActionResult Get([FromUri] string word)
        {
            if(string.IsNullOrEmpty(word) | word.Length < 2)
            {
                return BadRequest("Word is too short!");
            }

            var words = new List<string>();

            //var anagrams = new List<Anagram>()
            //{
            //    new Anagram(){ Name = "test"},
            //    new Anagram(){ Name = "test2"},
            //    new Anagram(){ Name = "test3"},                
            //};

            //return anagrams;
            ObjectCache cache = MemoryCache.Default;
            var cachedWords = cache.Get("cachedWords") as List<string>;
            if (cachedWords != null)
            {
                words = cachedWords;
            }
            else
            {
                var path = HostingEnvironment.MapPath("~/Resources/fullform_bm.txt");
                words = TextFileParser.ParseFile(path);

                CacheItemPolicy policy = new CacheItemPolicy 
                { 
                    AbsoluteExpiration = DateTimeOffset.Now.AddDays(30) 
                };

                cache.Add("cachedWords", words, policy);
            }

            if (!words.Contains(word)) 
            {
                return BadRequest("Word does not exist in dictionary!"); 
            }


            var finder = new AnagramFinder()
            { 
                Words = words
            };

            return Ok(new { Anagrams = finder.FindOne(word) });
        }

        // GET: api/Anagram/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Anagram
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Anagram/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Anagram/5
        //public void Delete(int id)
        //{
        //}
    }
}
