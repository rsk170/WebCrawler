using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class Crawler
    {
        private readonly HttpClient _client = new HttpClient();

        private readonly Queue<Uri> _queue = new Queue<Uri>();

        private readonly List<Uri> _visited = new List<Uri>();

        public Crawler()
        {

        }
        public Crawler(HttpClient client, Queue<Uri> queue, List<Uri> visited)
        {
            _client = client;
            _queue = queue;
            _visited = visited;
        }

        private async Task AddNewUrisAsync(Uri currentUri)
        {
            var provider = await GetLinkProvider(currentUri);
            foreach (var uri in ExtractUris(currentUri, provider))
            {
                EnqueueNewUri(uri);
            }
        }

        public void EnqueueNewUri(Uri uri)
        {
            if (!_visited.Contains(uri))
            {
                _visited.Add(uri);
                _queue.Enqueue(uri);
            }
        }

        public async Task<ILinkProvider> GetLinkProvider(Uri requestUri)
        {
            var response = await _client.GetAsync(requestUri);
            if (response.Content.Headers.ContentType.MediaType == "text/html")
            {
                var htmlContent = await response.Content.ReadAsStringAsync();
                return new HtmlLinkProvider(htmlContent);
            }
            else
            {
                return new NoLinkProvider();
            }
        }

        public static IEnumerable<Uri> ExtractUris(Uri currentUri, ILinkProvider linkProvider)
        {
            foreach (var link in linkProvider.GetLinks())
            {
                if (TryCreateUri(link, out Uri uri))
                {
                    if (!uri.IsAbsoluteUri)
                    {
                        var absoluteUri = new Uri(currentUri, link);
                        yield return absoluteUri;
                    }
                    else if (uri.Host == currentUri.Host)
                    {
                        yield return uri;
                    }
                }
            }
        }

        private static bool TryCreateUri(string link, out Uri uri)
        {
            if (Uri.TryCreate(link, UriKind.RelativeOrAbsolute, out uri))
            {
                if (uri.IsAbsoluteUri)
                {
                    return uri.Scheme == "http" || uri.Scheme == "https";
                }

                return true;
            }
            return false;
        }

        public async Task Output()
        {
            while (_queue.TryDequeue(out var currenturi))
            {
                Console.WriteLine("[q: {0}, v: {1}] {2}", _queue.Count, _visited.Count, currenturi);
                await AddNewUrisAsync(currenturi);
            }
        }
    }
}
