using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler
{
    public class NoLinkProvider : ILinkProvider
    {
        public IEnumerable<string> GetLinks()
        {
            yield break;
        }
    }
}
