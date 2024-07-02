namespace Kuiper.Clustering.ServiceApi.Middware
{
    public static class HttpRequestExtensions
    {
        public static string GetRequestBaseUri(this HttpContext httpContext)
            => httpContext.Request.GetRequestBaseUri();

        public static string GetRequestBaseUri(this HttpRequest request)
        {
            var forwardedProto = request.Headers["X-Forwarded-Proto"].ToString();
            var forwardedHost = request.Headers["X-Forwarded-Host"].ToString();

            if (string.IsNullOrEmpty(forwardedProto) || string.IsNullOrEmpty(forwardedHost))
            {
                // Fallback to request's scheme and host if headers are not available
                forwardedProto = request.Scheme;
                forwardedHost = request.Host.ToString();
            }

            var baseUri = $"{forwardedProto}://{forwardedHost}";

            return baseUri;
        }

        public static string CreateServerEndpoint(this HttpRequest request, string path)
        {
            var baseUri = request.GetRequestBaseUri();

            var endpoint = $"{baseUri}/{path.TrimStart('/')}";

            return endpoint;
        }
    }
}
