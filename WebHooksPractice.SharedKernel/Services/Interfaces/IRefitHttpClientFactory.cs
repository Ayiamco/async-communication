namespace WebHooks.SharedKernel.Services.Interfaces
{
    public interface IRefitHttpClientFactory<T>
    {
        T CreateClient(string baseAddressKey);
    }
}