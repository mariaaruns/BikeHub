namespace BikeHub.Extension
{
    public static class UrlHelperExtensions
    {
        public static string GetBaseUrl(this string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;
            var uri = new Uri(url);
            var baseUrl = $"{uri.Scheme}://{uri.Host}";
            if (!uri.IsDefaultPort)
            {
                baseUrl += $":{uri.Port}";
            }
            return baseUrl;
        }
    }
}
