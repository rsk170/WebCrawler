using System;
using System.Collections.Generic;

using WebCrawler;

using NUnit.Framework;
using System.Linq;
using FluentAssertions;

namespace Web_Crawler
{
    [TestFixture]
    public class WebCrawlerTests
    {
        [TestCaseSource(nameof(GetLinkTestData))]
        public void Should_output_links_correctly(LinkTestData data)
        {
            // Arrange
            var sourceUrl = data.SourceUrl;
            var linkProvider = new LinkProviderStub(data.Links);

            // Act
            var actual = Crawler.ExtractUris(sourceUrl, linkProvider).ToList();

            // Assert
            actual.Should().BeEquivalentTo(data.ExpectedUrls);
        }

        private class LinkProviderStub : ILinkProvider
        {
            private readonly string[] _links;

            public LinkProviderStub(params string[] links)
            {
                _links = links;
            }

            public IEnumerable<string> GetLinks()
            {
                return _links.AsEnumerable();
            }
        }

        private static IEnumerable<LinkTestData> GetLinkTestData()
        {
            yield return new LinkTestData()
            {
                SourceUrl = new Uri("https://melon.com"),
                Links = new[] { "/service/someservice", "/service/users" },
                ExpectedUrls = new[] { new Uri("https://melon.com/service/someservice"), new Uri("https://melon.com/service/users") },

            };
            yield return new LinkTestData()
            {
                SourceUrl = new Uri("http://blog.cwa.me.uk"),
                Links = new[] { "http://blog.cwa.me.uk/about/", "http://blog.cwa.me.uk/contact" },
                ExpectedUrls = new[] { new Uri("http://blog.cwa.me.uk/about/"), new Uri("http://blog.cwa.me.uk/contact") },
            };
            yield return new LinkTestData()
            {
                SourceUrl = new Uri("http://somewebsite.com"),
                Links = new[] { "/service/someservice", "http://somewebsite.com/service/users" },
                ExpectedUrls = new[] { new Uri("http://somewebsite.com/service/someservice"), new Uri("http://somewebsite.com/service/users") },
            };
            yield return new LinkTestData()
            {
                SourceUrl = new Uri("https://melon.com"),
                Links = new[] { "https://google.com/", "/service/users" },
                ExpectedUrls = new[] { new Uri("https://melon.com/service/users") },
            };
            yield return new LinkTestData()
            {
                SourceUrl = new Uri("http://blog.cwa.me.uk/2010/12/"),
                Links = new[] { "sohttp://blog.cwa.me.uk/2010/12/mepost", "http://blogcwameuk/2010/12/", "http://.blog.cwa.me.uk." },
                ExpectedUrls = new Uri[0],
            };
        }

        public class LinkTestData
        {
            public Uri SourceUrl { get; set; }
            public string[] Links { get; set; }
            public Uri[] ExpectedUrls { get; set; }
        }
    }
}
