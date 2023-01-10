using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
