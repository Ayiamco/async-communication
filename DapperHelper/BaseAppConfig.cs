namespace DapperHelper
{
    public class ConnectionStrings
    {
        public static string? SqlServerConnection { get; set; }

        public static string?  SybaseConnection { get; set; }

        public static string?  OracleConnection { get; set; }
    }

    public enum CommandResp
    {
        Success,Failure,UniqueKeyViolation
    }
}
