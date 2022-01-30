namespace heitech.configXt.ui
{
    public class HttpConfiguration
    {
        public Uri BaseUri { get; }
        public IEnumerable<(string, string)> HeadersForMutation { get; }

        public HttpConfiguration(string url, params (string, string)[] headers)
        {
            BaseUri = new Uri(url);
            HeadersForMutation = headers;
        }
    }
}
