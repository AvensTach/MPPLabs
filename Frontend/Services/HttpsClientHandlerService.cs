namespace Frontend.Services
{
    public class HttpsClientHandlerService
    {
        public HttpMessageHandler GetPlatformMessageHandler()
        {
#if ANDROID
            var handler = new Xamarin.Android.Net.AndroidMessageHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                return cert?.Issuer.Equals("CN=localhost") ?? true;
            };
            return handler;
#elif WINDOWS
            var handler = new HttpClientHandler();
            // This forces Windows to accept any SSL certificate (for local dev only)
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                return true;
            };
            return handler;
#else
            return new HttpClientHandler();
#endif
        }
    }
}