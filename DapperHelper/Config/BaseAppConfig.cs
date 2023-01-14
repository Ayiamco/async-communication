using Microsoft.Extensions.DependencyInjection;

namespace Dapper.BaseRepository.Config
{
    public static class ConnectionStrings
    {
        public static string? SqlServerConnection { get; set; }

        public static string? SybaseConnection { get; set; }

        public static string? OracleConnection { get; set; }

        public static bool ThrowErrors { get; set; }

        public static IServiceCollection AddConnectionStrings(this IServiceCollection services, Action<ConnectionStringOptions> options)
        {

            var connectionStringOptions = new ConnectionStringOptions();
            options(connectionStringOptions);

            SqlServerConnection = connectionStringOptions.SqlServerConnectionString;
            SybaseConnection = connectionStringOptions.SybaseConnectionString;
            OracleConnection = connectionStringOptions.OracleConnectionString;
            ThrowErrors = connectionStringOptions.ThrowErrors;
            return services;
        }
    }

    public class ConnectionStringOptions
    {
        /// <summary>
        /// The Default connection string for Sql server connections.
        /// </summary>
        public string? SqlServerConnectionString { get; set; }

        /// <summary>
        /// The Default connection string for Sybase connectionf.
        /// </summary>
        public string? SybaseConnectionString { get; set; }

        /// <summary>
        /// The Default connection string for Oracle connections.
        /// </summary>
        public string? OracleConnectionString { get; set; }

        /// <summary>
        /// Flag to throw errors or use logger to log errors. Default is <see cref="false"/>
        /// </summary>
        public bool ThrowErrors { get; set; }
    }

    public enum CommandResp
    {
        Success, Failure, UniqueKeyViolation
    }
}
