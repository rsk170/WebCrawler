using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace WebCrawler
{
    public class Program
    {
        private static readonly HttpClient _client = new HttpClient();

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Please enter a webpage address: ");
            string startUriString = Console.ReadLine();

            var startUri = new Uri(startUriString);
            Crawler crawler = new Crawler();
            crawler.EnqueueNewUri(startUri);

            await crawler.Output();
        }
    }
}
