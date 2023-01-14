namespace Dapper.Repository.interfaces
{
    public interface IBaseRepositoryLogger<TRepo>
    {
        void LogInformation(string message);

        void LogError(string message);
    }
}
