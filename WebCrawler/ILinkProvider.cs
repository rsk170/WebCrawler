using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler
{
    public interface ILinkProvider
    {
        IEnumerable<string> GetLinks();
    }
}
