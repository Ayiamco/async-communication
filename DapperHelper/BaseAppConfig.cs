using Microsoft.Extensions.DependencyInjection;

namespace DapperHelper
{
    public static class ConnectionStrings
    {
        public static string? SqlServerConnection { get; set; }

        public static string?  SybaseConnection { get; set; }

        public static string?  OracleConnection { get; set; }

        public static IServiceCollection AddConnectionStrings(this IServiceCollection services,Action<ConnectionStringOptions> options)
        {

            var connectionStringOptions = new ConnectionStringOptions();
            options(connectionStringOptions);

            SqlServerConnection = connectionStringOptions.SqlServerConnectionString;
            SybaseConnection = connectionStringOptions.SybaseConnectionString;
            OracleConnection = connectionStringOptions.OracleConnectionString;
            return services;
        }
    }

    public class ConnectionStringOptions
    {
        public string? SqlServerConnectionString { get; set; }

        public string? SybaseConnectionString { get; set; }

        public  string? OracleConnectionString { get; set; }
    }

    public enum CommandResp
    {
        Success,Failure,UniqueKeyViolation
    }


}
