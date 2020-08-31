using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler
{
    public class HtmlLinkProvider : ILinkProvider
    {
        private readonly string _htmlContent;

        public HtmlLinkProvider(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public IEnumerable<string> GetLinks()
        {
            var document = new HtmlDocument();
            document.LoadHtml(_htmlContent);

            foreach (HtmlNode linkNode in document.DocumentNode.SelectNodes("//a[@href]"))
            {
                yield return linkNode.Attributes["href"].Value;
            }
        }
    }
}
