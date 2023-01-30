﻿namespace Dapper.BaseRepository.Components
{
    using Dapper;
    using Dapper.BaseRepository.Attributes;
    using Dapper.BaseRepository.Config;
    using Dapper.Repository.interfaces;
    using System.Data;
    using System.Runtime.CompilerServices;


    public enum DbType
    {
        OracleServer,
        SqlServer,
        SybaseServer
    }

    /// <summary>
    /// Supported db types for Sql text that manipulates data (DML).
    /// </summary>
    public enum CommandDbTypes
    {
        SqlServer, Oracle, Sybase
    }

    /// <summary>
    /// Supported db types for Sql stored procedure that manipulates data (DML).
    /// </summary>
    public enum StoredProcDbTypes
    {
        SqlServer, Oracle, Sybase
    }

    /// <summary>
    /// Supported db types for Sql Text that queries data (DQL) and returns non scalar objects.
    /// </summary>
    public enum QueryDbTypes
    {
        SqlServer, Oracle, Sybase
    }

    public enum ScalarQueryDbTypes
    {
        SqlServer, Oracle, Sybase
    }

    public abstract class BaseRepository<TRepo>
    {

        //TODO: Figure out how libraries log
        protected readonly Dictionary<CommandDbTypes, Func<string, string, object?, int>> CommandsMap;

        protected BaseRepository()
        {
            CommandsMap = new()
                {
                    {CommandDbTypes.Sybase, DapperOrmExecutor.ExecuteSybaseCommand },
                    {CommandDbTypes.Oracle, DapperOrmExecutor.ExecuteOracleCommand },
                    {CommandDbTypes.SqlServer, DapperOrmExecutor.ExecuteCommand},
                };
        }

        /// <summary>
        /// Executes a sql DDL/DML on a sql server database. The connectionString is gotten from
        /// the DefaultConnection property on the <see cref="ConnectionStrings"/> property of the  <see cref="BaseAppConfig"/> configuration object.
        /// </summary>
        /// <param name="sqlCommand">sql statement</param>
        /// <param name="queryParam">queryParam parameters for the sql command.</param>
        /// <returns></returns>
        protected Task<CommandResp> RunCommand(string sqlCommand, object queryParam,
             [CallerMemberName] string callerMemberName = "")
        {
            if (queryParam.GetType() == typeof(string))
                throw new ArgumentException("queryParam must be an object containing the sqlCommand queryParam parameters");

            DapperOrmExecutor.ExecuteCommand(sqlCommand, GetConnectionString(string.Empty, DbType.SqlServer), queryParam);
            //logger.LogInformation($"Successfully ran command from function: {callerMemberName}");
            return Task.FromResult(CommandResp.Success);
        }

        /// <summary>
        /// Executes a sql DDL/DML on a sql server database. If the connectionString is not specified 
        /// the DefaultConnection property on the BaseAppConfig Connectionstring is used.
        /// </summary>
        /// <param name="sqlCommand">sql statement</param>
        /// <param name="connectionString">db connection string.</param>
        /// <param name="queryParam">queryParam parameters for the sql command.</param>
        /// <returns></returns>
        protected Task<CommandResp> RunCommand(string sqlCommand, object queryParam,
            string connectionString, [CallerMemberName] string callerMemberName = "")
        {
            if (queryParam is string)
                throw new ArgumentException("queryParam must be an object containing the sqlCommand queryParam parameters");

            connectionString = string.IsNullOrWhiteSpace(connectionString) ?
                GetConnectionString(connectionString, DbType.SqlServer) : connectionString;

            DapperOrmExecutor.ExecuteCommand(sqlCommand, connectionString, queryParam);
            //logger.LogInformation($"Successfully ran command from function: {callerMemberName}");
            return Task.FromResult(CommandResp.Success);
        }

        /// <summary>
        /// Executes a sql DDL/DML on the specified database type. The connectionString is gotten from
        /// the corresponding BaseAppConfig Connectionstring property.
        /// </summary>
        /// <param name="sqlCommand">sql statement</param>
        /// <param name="connectionString">db connection string.</param>
        /// <param name="queryParam">queryParam parameters for the sql command.</param>
        /// <returns></returns>
        protected Task<CommandResp> RunCommand(string sqlCommand, object queryParam,
            CommandDbTypes commandType, [CallerMemberName] string callerMemberName = "")
        {
            var map = new Dictionary<CommandDbTypes, DbType>
                {
                    { CommandDbTypes.SqlServer, DbType.SqlServer },
                    { CommandDbTypes.Oracle, DbType.OracleServer },
                    { CommandDbTypes.Sybase, DbType.SybaseServer },
                };

            if (queryParam.GetType() == typeof(string))
                throw new ArgumentException("queryParam must be an object containing the sqlCommand queryParam parameters");

            CommandsMap[commandType](sqlCommand, GetConnectionString(string.Empty, map[commandType]), queryParam);
            //logger.LogInformation($"Successfully ran command from function: {callerMemberName}");
            return Task.FromResult(CommandResp.Success);
        }

        /// <summary>
        /// Executes a sql DDL/DML on the specified database type. If the connectionString is not specified 
        /// the corresponding BaseAppConfig Connectionstring property is used.
        /// </summary>
        /// <param name="sqlCommand">sql statement</param>
        /// <param name="connectionString">db connection string.</param>
        /// <param name="queryParam">queryParam parameters for the sql command.</param>
        /// <returns></returns>
        protected Task<CommandResp> RunCommand(string sqlCommand, object queryParam,
            CommandDbTypes commandType, string connectionString, [CallerMemberName] string callerMemberName = "")
        {
            if (queryParam.GetType() == typeof(string))
                throw new ArgumentException("queryParam must be an object containing the sqlCommand queryParam parameters");

            var map = new Dictionary<CommandDbTypes, DbType> {
                { CommandDbTypes.SqlServer, DbType.SqlServer },
                { CommandDbTypes.Oracle, DbType.OracleServer },
                { CommandDbTypes.Sybase, DbType.SybaseServer },
            };

            connectionString = string.IsNullOrWhiteSpace(connectionString) ?
                GetConnectionString(connectionString, map[commandType]) : connectionString;

            CommandsMap[commandType](sqlCommand, connectionString, queryParam);
            //logger.LogInformation($"Successfully ran command from function: {callerMemberName}");
            return Task.FromResult(CommandResp.Success);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <param name="callerMemberName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery,
            [CallerMemberName] string callerMemberName = "") where TResult : class
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentException("sqlQuery cannot be empty");

            var conn = ConnectionStrings.SqlServerConnection;
            if (string.IsNullOrWhiteSpace(conn))
                throw new ArgumentNullException("SqlConnection string is null");

            IEnumerable<TResult> resp = DapperOrmExecutor.Query<TResult>(sqlQuery, conn, new { });
            //logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
            return Task.FromResult(resp);
        }

        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery, object queryParam,
            [CallerMemberName] string callerMemberName = "") where TResult : class
        {
            if (string.IsNullOrWhiteSpace(sqlQuery) || queryParam.GetType() == typeof(string))
                throw new ArgumentException("sqlQuery cannot be empty and queryParam must be sqlQuery parameter object");

            IEnumerable<TResult> resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, queryParam);
            //logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
            return Task.FromResult(resp);
        }

        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery, object queryParam, string connectionString,
            [CallerMemberName] string callerMemberName = "") where TResult : class
        {

            if (string.IsNullOrWhiteSpace(sqlQuery) || queryParam.GetType() == typeof(string))
                throw new ArgumentException("sqlQuery cannot be empty and queryParam must be sqlQuery parameter object");

            IEnumerable<TResult> resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, queryParam);
            //logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
            return Task.FromResult(resp);
        }

        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery,
           QueryDbTypes queryDbType, [CallerMemberName] string callerMemberName = "") where TResult : class
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentException("sqlQuery cannot be empty and queryParam must be sqlQuery parameter object");

            IEnumerable<TResult> resp = null;
            switch (queryDbType)
            {
                case QueryDbTypes.SqlServer:
                    resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, new { });
                    break;
                case QueryDbTypes.Sybase:
                    resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, new { });
                    break;
                case QueryDbTypes.Oracle:
                    resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, new { });
                    break;
            }
            //logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
            return Task.FromResult(resp ?? Enumerable.Empty<TResult>());
        }

        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery, object queryParam,
           QueryDbTypes queryDbType, [CallerMemberName] string callerMemberName = "") where TResult : class
        {
            if (string.IsNullOrWhiteSpace(sqlQuery) || queryParam.GetType() == typeof(string))
                throw new ArgumentException("sqlQuery cannot be empty and queryParam must be sqlQuery parameter object");

            IEnumerable<TResult> resp = null;
            switch (queryDbType)
            {
                case QueryDbTypes.SqlServer:
                    resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, queryParam);
                    break;
                case QueryDbTypes.Sybase:
                    resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, queryParam);
                    break;
                case QueryDbTypes.Oracle:
                    resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, queryParam);
                    break;
            }
            //logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
            return Task.FromResult(resp ?? Enumerable.Empty<TResult>());
        }

        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery, string connectionString,
           QueryDbTypes queryDbType, [CallerMemberName] string callerMemberName = "") where TResult : class
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentException("sqlQuery cannot be empty.");

            IEnumerable<TResult> resp = null;
            switch (queryDbType)
            {
                case QueryDbTypes.SqlServer:
                    resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, new { });
                    break;
                case QueryDbTypes.Sybase:
                    resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, new { });
                    break;
                case QueryDbTypes.Oracle:
                    resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, new { });
                    break;
            }
            //logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
            return Task.FromResult(resp ?? Enumerable.Empty<TResult>());
        }

        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery, object queryParam, string connectionString,
           QueryDbTypes queryDbType, [CallerMemberName] string callerMemberName = "") where TResult : class
        {
            if (string.IsNullOrWhiteSpace(sqlQuery) || queryParam.GetType() == typeof(string))
                throw new ArgumentException("sqlQuery cannot be empty and queryParam must be sqlQuery parameter object");

            IEnumerable<TResult> resp = null;
            switch (queryDbType)
            {
                case QueryDbTypes.SqlServer:
                    resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, queryParam);
                    break;
                case QueryDbTypes.Sybase:
                    resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, queryParam);
                    break;
                case QueryDbTypes.Oracle:
                    resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, queryParam);
                    break;
            }
            //logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
            return Task.FromResult(resp ?? Enumerable.Empty<TResult>());
        }

        protected Task<DynamicParameters> RunCommandProc<TInputParam>(string storedProcedure, TInputParam paramObject,
             [CallerMemberName] string callerMemberName = "")
        {
            var dynamicParameters = CreateDynamicParameter(paramObject);
            DapperOrmExecutor.ExecuteCommandProc(storedProcedure, ConnectionStrings.SqlServerConnection, dynamicParameters);
            return Task.FromResult(dynamicParameters);
        }

        /// <summary>
        /// Creates <see cref="DynamicParameters"/> object or adds additional parameters from the storedProcParams.
        /// </summary>
        /// <param name="storedProcParams">Object containing stored proc input, outpust and return paramaters</param>
        /// <param name="dynamicParameters">Dynamic parameter that the storedProcParams would be added to.</param>
        /// <returns><see cref=" DynamicParameters "/> containing the stored procedure input, output and return parameters.</returns>
        protected DynamicParameters CreateDynamicParameter(object storedProcParams, DynamicParameters? dynamicParameters = default)
        {
            dynamicParameters = dynamicParameters == null ? new DynamicParameters() : dynamicParameters;
            if (storedProcParams == null)
                return dynamicParameters;

            var properties = storedProcParams.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var key = prop.Name;
                Attribute[] attrs = Attribute.GetCustomAttributes(prop);
                if (attrs.Length == 0)
                {
                    var value = prop.GetValue(storedProcParams);
                    dynamicParameters.Add(key, value);
                    continue;
                }
                AddOutputParam(dynamicParameters, key, attrs);
            }
            return dynamicParameters;
        }

        protected Task<decimal?> RunScalar(string query, [CallerMemberName] string callerMemberName = "")
        {
            //logger.LogInformation($"Sending query from  function: {callerMemberName}...");
            decimal? resp = (decimal)DapperOrmExecutor.QueryScalar(query, ConnectionStrings.SqlServerConnection, new { }); ;

            //logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
            return Task.FromResult(resp);
        }


        #region private methods
        private string GetLogMessage(string name, Exception ex) =>
            @$"Error occured at while running query from function :{name}; Message:{ex.Message}. 
{ex.StackTrace}";

        private static string GetConnectionString(string? conn, DbType sqlType)
        {
            if (!string.IsNullOrWhiteSpace(conn)) return conn;


            //TODO: Finish map
            var connectionStringMap = new Dictionary<DbType, string>()
            {
                {DbType.SqlServer,ConnectionStrings.SqlServerConnection },
                {DbType.SybaseServer,ConnectionStrings.SybaseConnection },
                {DbType.OracleServer,ConnectionStrings.OracleConnection },
            };

            var dbTypeNameMap = new Dictionary<DbType, string>()
            {
                {DbType.SqlServer,"SqlServer database" },
                {DbType.SybaseServer,"Sybase database" },
                {DbType.OracleServer,"Oracle database" },
            };
            var connectionString = connectionStringMap[sqlType];
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException($"Connection string for {dbTypeNameMap[sqlType]} in BaseAppConfig is not setup.Please pass in connectionString or setup BaseAppConfig");
            return connectionString;
        }

        private static void AddOutputParam(DynamicParameters dynamicParameters, string propName, Attribute[] customAttributes)
        {
            foreach (var attr in customAttributes)
            {
                var attributeType = attr.GetType();
                if (attributeType == typeof(SpOutputStringAttribute))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.String, direction: ParameterDirection.Output, size: (int)attributeType.GetProperties().Where<System.Reflection.PropertyInfo>(x => x.Name == nameof(SpOutputStringAttribute.Size)).First().GetValue(attr));
                if (attributeType == typeof(SpReturnStringAttribute))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.String, direction: ParameterDirection.ReturnValue, size: (int)attributeType.GetProperties().Where<System.Reflection.PropertyInfo>(x => x.Name == nameof(SpReturnStringAttribute.Size)).First().GetValue(attr));
                if (attributeType == typeof(SpReturnIntAttribute))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.Int32, direction: ParameterDirection.ReturnValue);
                if (attributeType == typeof(SpOutputIntAttribute))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.Int32, direction: ParameterDirection.Output);
                if (attributeType == typeof(SpReturnBigIntAttribute))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.Int64, direction: ParameterDirection.ReturnValue);
                if (attributeType == typeof(SpOutputBigIntAttribute))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.Int64, direction: ParameterDirection.Output);
            }
        }

        #endregion
    }

    public abstract class BaseRepository<TRepo, TLogger> where TLogger : IBaseRepositoryLogger<TRepo> where TRepo : class
    {
        protected readonly IBaseRepositoryLogger<TRepo> logger;
        protected readonly Dictionary<CommandDbTypes, Func<string, string, object?, int>> CommandsMap;

        protected BaseRepository(IBaseRepositoryLogger<TRepo> logger)
        {
            this.logger = logger;
            CommandsMap = new()
            {
                {CommandDbTypes.Sybase, DapperOrmExecutor.ExecuteSybaseCommand },
                {CommandDbTypes.Oracle, DapperOrmExecutor.ExecuteOracleCommand },
                {CommandDbTypes.SqlServer, DapperOrmExecutor.ExecuteCommand},
            };
        }

        /// <summary>
        /// Executes a sql DDL/DML on a sql server database. The connectionString is gotten from
        /// the DefaultConnection property on the <see cref="ConnectionStrings"/> property of the  <see cref="BaseAppConfig"/> configuration object.
        /// </summary>
        /// <param name="sqlCommand">sql statement</param>
        /// <param name="queryParam">queryParam parameters for the sql command.</param>
        /// <returns></returns>
        protected Task<CommandResp> RunCommand(string sqlCommand, object queryParam,
             [CallerMemberName] string callerMemberName = "")
        {
            try
            {
                if (queryParam.GetType() == typeof(string))
                    throw new ArgumentException("queryParam must be an object containing the sqlCommand queryParam parameters");

                DapperOrmExecutor.ExecuteCommand(sqlCommand, GetConnectionString(string.Empty, DbType.SqlServer), queryParam);
                logger.LogInformation($"Successfully ran command from function: {callerMemberName}");
                return Task.FromResult(CommandResp.Success);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                    return Task.FromResult(CommandResp.UniqueKeyViolation);

                logger.LogError(GetLogMessage(callerMemberName, ex));
                return Task.FromResult(CommandResp.Failure);
            }
        }

        /// <summary>
        /// Executes a sql DDL/DML on a sql server database. If the connectionString is not specified 
        /// the DefaultConnection property on the BaseAppConfig Connectionstring is used.
        /// </summary>
        /// <param name="sqlCommand">sql statement</param>
        /// <param name="connectionString">db connection string.</param>
        /// <param name="queryParam">queryParam parameters for the sql command.</param>
        /// <returns></returns>
        protected Task<CommandResp> RunCommand(string sqlCommand, object queryParam,
            string connectionString, [CallerMemberName] string callerMemberName = "")
        {
            try
            {
                if (queryParam is string)
                    throw new ArgumentException("queryParam must be an object containing the sqlCommand queryParam parameters");

                connectionString = string.IsNullOrWhiteSpace(connectionString) ?
                    GetConnectionString(connectionString, DbType.SqlServer) : connectionString;

                DapperOrmExecutor.ExecuteCommand(sqlCommand, connectionString, queryParam);
                logger.LogInformation($"Successfully ran command from function: {callerMemberName}");
                return Task.FromResult(CommandResp.Success);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                    return Task.FromResult(CommandResp.UniqueKeyViolation);

                logger.LogError(GetLogMessage(callerMemberName, ex));
                return Task.FromResult(CommandResp.Failure);
            }
        }

        /// <summary>
        /// Executes a sql DDL/DML on the specified database type. The connectionString is gotten from
        /// the corresponding BaseAppConfig Connectionstring property.
        /// </summary>
        /// <param name="sqlCommand">sql statement</param>
        /// <param name="connectionString">db connection string.</param>
        /// <param name="queryParam">queryParam parameters for the sql command.</param>
        /// <returns></returns>
        protected Task<CommandResp> RunCommand(string sqlCommand, object queryParam,
            CommandDbTypes commandType, [CallerMemberName] string callerMemberName = "")
        {
            try
            {
                var map = new Dictionary<CommandDbTypes, DbType>
                {
                    { CommandDbTypes.SqlServer, DbType.SqlServer },
                    { CommandDbTypes.Oracle, DbType.OracleServer },
                    { CommandDbTypes.Sybase, DbType.SybaseServer },
                };

                if (queryParam.GetType() == typeof(string))
                    throw new ArgumentException("queryParam must be an object containing the sqlCommand queryParam parameters");

                CommandsMap[commandType](sqlCommand, GetConnectionString(string.Empty, map[commandType]), queryParam);
                logger.LogInformation($"Successfully ran command from function: {callerMemberName}");
                return Task.FromResult(CommandResp.Success);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                    return Task.FromResult(CommandResp.UniqueKeyViolation);

                logger.LogError(GetLogMessage(callerMemberName, ex));
                return Task.FromResult(CommandResp.Failure);
            }
        }

        /// <summary>
        /// Executes a sql DDL/DML on the specified database type. If the connectionString is not specified 
        /// the corresponding BaseAppConfig Connectionstring property is used.
        /// </summary>
        /// <param name="sqlCommand">sql statement</param>
        /// <param name="connectionString">db connection string.</param>
        /// <param name="queryParam">queryParam parameters for the sql command.</param>
        /// <returns></returns>
        protected Task<CommandResp> RunCommand(string sqlCommand, object queryParam,
            CommandDbTypes commandType, string connectionString, [CallerMemberName] string callerMemberName = "")
        {
            try
            {
                if (queryParam.GetType() == typeof(string))
                    throw new ArgumentException("queryParam must be an object containing the sqlCommand queryParam parameters");

                var map = new Dictionary<CommandDbTypes, DbType>
                {
                    { CommandDbTypes.SqlServer, DbType.SqlServer },
                    { CommandDbTypes.Oracle, DbType.OracleServer },
                    { CommandDbTypes.Sybase, DbType.SybaseServer },
                };

                connectionString = string.IsNullOrWhiteSpace(connectionString) ?
                    GetConnectionString(connectionString, map[commandType]) : connectionString;
                CommandsMap[commandType](sqlCommand, connectionString, queryParam);
                logger.LogInformation($"Successfully ran command from function: {callerMemberName}");
                return Task.FromResult(CommandResp.Success);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                    return Task.FromResult(CommandResp.UniqueKeyViolation);

                logger.LogError(GetLogMessage(callerMemberName, ex));
                return Task.FromResult(CommandResp.Failure);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <param name="callerMemberName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery,
            [CallerMemberName] string callerMemberName = "") where TResult : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlQuery))
                    throw new ArgumentException("sqlQuery cannot be empty");

                var conn = ConnectionStrings.SqlServerConnection;
                if (string.IsNullOrWhiteSpace(conn))
                    throw new ArgumentNullException("SqlConnection string is null");

                IEnumerable<TResult> resp = DapperOrmExecutor.Query<TResult>(sqlQuery, conn, new { });
                logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
                return Task.FromResult(resp);
            }
            catch (Exception ex)
            {
                logger.LogError(GetLogMessage(callerMemberName, ex));
                return Task.FromResult(Enumerable.Empty<TResult>());
            }
        }

        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery, object queryParam,
            [CallerMemberName] string callerMemberName = "") where TResult : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlQuery) || queryParam.GetType() == typeof(string))
                    throw new ArgumentException("sqlQuery cannot be empty and queryParam must be sqlQuery parameter object");

                IEnumerable<TResult> resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, queryParam);
                logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
                return Task.FromResult(resp);
            }
            catch (Exception ex)
            {
                logger.LogError(GetLogMessage(callerMemberName, ex));
                return Task.FromResult(Enumerable.Empty<TResult>());
            }
        }

        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery, object queryParam, string connectionString,
            [CallerMemberName] string callerMemberName = "") where TResult : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlQuery) || queryParam.GetType() == typeof(string))
                    throw new ArgumentException("sqlQuery cannot be empty and queryParam must be sqlQuery parameter object");

                IEnumerable<TResult> resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, queryParam);
                logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
                return Task.FromResult(resp);
            }
            catch (Exception ex)
            {
                logger.LogError(GetLogMessage(callerMemberName, ex));
                return Task.FromResult(Enumerable.Empty<TResult>());
            }
        }

        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery,
           QueryDbTypes queryDbType, [CallerMemberName] string callerMemberName = "") where TResult : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlQuery))
                    throw new ArgumentException("sqlQuery cannot be empty and queryParam must be sqlQuery parameter object");

                IEnumerable<TResult> resp = null;
                switch (queryDbType)
                {
                    case QueryDbTypes.SqlServer:
                        resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, new { });
                        break;
                    case QueryDbTypes.Sybase:
                        resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, new { });
                        break;
                    case QueryDbTypes.Oracle:
                        resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, new { });
                        break;
                }
                logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
                return Task.FromResult(resp ?? Enumerable.Empty<TResult>());
            }
            catch (Exception ex)
            {
                logger.LogError(GetLogMessage(callerMemberName, ex));
                return Task.FromResult(Enumerable.Empty<TResult>());
            }
        }

        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery, object queryParam,
           QueryDbTypes queryDbType, [CallerMemberName] string callerMemberName = "") where TResult : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlQuery) || queryParam.GetType() == typeof(string))
                    throw new ArgumentException("sqlQuery cannot be empty and queryParam must be sqlQuery parameter object");

                IEnumerable<TResult> resp = null;
                switch (queryDbType)
                {
                    case QueryDbTypes.SqlServer:
                        resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, queryParam);
                        break;
                    case QueryDbTypes.Sybase:
                        resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, queryParam);
                        break;
                    case QueryDbTypes.Oracle:
                        resp = DapperOrmExecutor.Query<TResult>(sqlQuery, ConnectionStrings.SqlServerConnection, queryParam);
                        break;
                }
                logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
                return Task.FromResult(resp ?? Enumerable.Empty<TResult>());
            }
            catch (Exception ex)
            {
                logger.LogError(GetLogMessage(callerMemberName, ex));
                return Task.FromResult(Enumerable.Empty<TResult>());
            }
        }

        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery, string connectionString,
           QueryDbTypes queryDbType, [CallerMemberName] string callerMemberName = "") where TResult : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlQuery))
                    throw new ArgumentException("sqlQuery cannot be empty.");

                IEnumerable<TResult> resp = null;
                switch (queryDbType)
                {
                    case QueryDbTypes.SqlServer:
                        resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, new { });
                        break;
                    case QueryDbTypes.Sybase:
                        resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, new { });
                        break;
                    case QueryDbTypes.Oracle:
                        resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, new { });
                        break;
                }
                logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
                return Task.FromResult(resp ?? Enumerable.Empty<TResult>());
            }
            catch (Exception ex)
            {
                logger.LogError(GetLogMessage(callerMemberName, ex));
                return Task.FromResult(Enumerable.Empty<TResult>());
            }
        }

        protected Task<IEnumerable<TResult>> RunQuery<TResult>(string sqlQuery, object queryParam, string connectionString,
           QueryDbTypes queryDbType, [CallerMemberName] string callerMemberName = "") where TResult : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlQuery) || queryParam.GetType() == typeof(string))
                    throw new ArgumentException("sqlQuery cannot be empty and queryParam must be sqlQuery parameter object");

                IEnumerable<TResult> resp = null;
                switch (queryDbType)
                {
                    case QueryDbTypes.SqlServer:
                        resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, queryParam);
                        break;
                    case QueryDbTypes.Sybase:
                        resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, queryParam);
                        break;
                    case QueryDbTypes.Oracle:
                        resp = DapperOrmExecutor.Query<TResult>(sqlQuery, connectionString, queryParam);
                        break;
                }
                logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
                return Task.FromResult(resp ?? Enumerable.Empty<TResult>());
            }
            catch (Exception ex)
            {
                logger.LogError(GetLogMessage(callerMemberName, ex));
                return Task.FromResult(Enumerable.Empty<TResult>());
            }
        }

        protected Task<DynamicParameters> RunCommandProc<TInputParam>(string storedProcedure, TInputParam paramObject,
             [CallerMemberName] string callerMemberName = "")
        {
            try
            {
                var dynamicParameters = CreateDynamicParameter(paramObject);
                DapperOrmExecutor.ExecuteCommandProc(storedProcedure, ConnectionStrings.SqlServerConnection, dynamicParameters);
                return Task.FromResult(dynamicParameters);
            }
            catch (Exception ex)
            {
                logger.LogError(GetLogMessage($"{callerMemberName}/{storedProcedure}", ex));
                return default;
            }
        }

        /// <summary>
        /// Creates <see cref="DynamicParameters"/> object or adds additional parameters from the storedProcParams.
        /// </summary>
        /// <param name="storedProcParams">Object containing stored proc input, outpust and return paramaters</param>
        /// <param name="dynamicParameters">Dynamic parameter that the storedProcParams would be added to.</param>
        /// <returns><see cref=" DynamicParameters "/> containing the stored procedure input, output and return parameters.</returns>
        protected DynamicParameters CreateDynamicParameter(object storedProcParams, DynamicParameters? dynamicParameters = default)
        {
            dynamicParameters = dynamicParameters == null ? new DynamicParameters() : dynamicParameters;
            if (storedProcParams == null)
                return dynamicParameters;

            var properties = storedProcParams.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var key = prop.Name;
                Attribute[] attrs = Attribute.GetCustomAttributes(prop);
                if (attrs.Length == 0)
                {
                    var value = prop.GetValue(storedProcParams);
                    dynamicParameters.Add(key, value);
                    continue;
                }
                AddOutputParam(dynamicParameters, key, attrs);
            }
            return dynamicParameters;
        }

        protected Task<decimal?> RunScalar(string query, [CallerMemberName] string callerMemberName = "")
        {
            try
            {
                logger.LogInformation($"Sending query from  function: {callerMemberName}...");
                decimal? resp = (decimal)DapperOrmExecutor.QueryScalar(query, ConnectionStrings.SqlServerConnection, new { }); ;

                logger.LogInformation($"Successfully ran query from function: {callerMemberName}");
                return Task.FromResult(resp);
            }
            catch (Exception ex)
            {
                logger.LogError(GetLogMessage(callerMemberName, ex));
                return default;
            }
        }


        #region private methods
        private string GetLogMessage(string name, Exception ex) =>
            @$"Error occured at while running query from function :{name}; Message:{ex.Message}. 
{ex.StackTrace}";

        private static string GetConnectionString(string? conn, DbType sqlType)
        {
            if (!string.IsNullOrWhiteSpace(conn)) return conn;


            //TODO: Finish map
            var connectionStringMap = new Dictionary<DbType, string>()
            {
                {DbType.SqlServer,ConnectionStrings.SqlServerConnection },
                {DbType.SybaseServer,ConnectionStrings.SybaseConnection },
                {DbType.OracleServer,ConnectionStrings.OracleConnection },
            };

            var dbTypeNameMap = new Dictionary<DbType, string>()
            {
                {DbType.SqlServer,"SqlServer database" },
                {DbType.SybaseServer,"Sybase database" },
                {DbType.OracleServer,"Oracle database" },
            };
            var connectionString = connectionStringMap[sqlType];
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException($"Connection string for {dbTypeNameMap[sqlType]} in BaseAppConfig is not setup.Please pass in connectionString or setup BaseAppConfig");
            return connectionString;
        }

        private static void AddOutputParam(DynamicParameters dynamicParameters, string propName, Attribute[] customAttributes)
        {
            foreach (var attr in customAttributes)
            {
                var attributeType = attr.GetType();
                if (attributeType == typeof(SpOutputStringAttribute))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.String, direction: ParameterDirection.Output, size: (int)attributeType.GetProperties().Where<System.Reflection.PropertyInfo>(x => x.Name == nameof(SpOutputStringAttribute.Size)).First().GetValue(attr));
                if (attributeType == typeof(SpReturnStringAttribute))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.String, direction: ParameterDirection.ReturnValue, size: (int)attributeType.GetProperties().Where<System.Reflection.PropertyInfo>(x => x.Name == nameof(SpReturnStringAttribute.Size)).First().GetValue(attr));
                if (attributeType == typeof(SpReturnIntAttribute))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.Int32, direction: ParameterDirection.ReturnValue);
                if (attributeType == typeof(SpOutputIntAttribute))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.Int32, direction: ParameterDirection.Output);
                if (attributeType == typeof(SpReturnBigIntAttribute))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.Int64, direction: ParameterDirection.ReturnValue);
                if (attributeType == typeof(SpOutputBigIntAttribute))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.Int64, direction: ParameterDirection.Output);
            }
        }

        #endregion
    }
}