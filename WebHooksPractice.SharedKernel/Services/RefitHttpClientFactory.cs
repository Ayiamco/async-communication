using Refit;
using WebHooks.SharedKernel.Services.Interfaces;

namespace WebHooks.SharedKernel.Services
{
    public class RefitHttpClientFactory<T> : IRefitHttpClientFactory<T>
    {
        public T CreateClient(string baseAddressKey)
        {
            if (string.IsNullOrWhiteSpace(baseAddressKey))
                throw new ArgumentNullException($"Argument {nameof(baseAddressKey)} cannot be null or empty");

            return RestService.For<T>(baseAddressKey);
        }

        public T CreateClient(HttpClient httpClient)
        {
            if (httpClient == default)
                throw new ArgumentNullException($"Argument {nameof(httpClient)} cannot be null.");

            return RestService.For<T>(httpClient);
        }
    }
}
