using Dapper.BaseRepository.Attributes;
using Dapper.BaseRepository.Config;
using System.Data;

namespace Dapper.BaseRepository.Components
{
    internal static class BaseUtility
    {
        #region private methods
        internal static string GetLogMessage(string name, Exception ex) =>
            @$"Error occured at while running query from function :{name}; Message:{ex.Message}. 
{ex.StackTrace}";

        internal static string GetConnectionString(string? conn, DbType sqlType)
        {
            if (!string.IsNullOrWhiteSpace(conn)) return conn;


            //TODO: Finish map
            var connectionStringMap = new Dictionary<DbType, string>()
            {
                {DbType.SqlServer,ConnectionStrings.SqlServerConnection },
                {DbType.Sybase,ConnectionStrings.SybaseConnection },
                {DbType.Oracle,ConnectionStrings.OracleConnection },
            };

            var dbTypeNameMap = new Dictionary<DbType, string>()
            {
                {DbType.SqlServer,"SqlServer database" },
                {DbType.Sybase,"Sybase database" },
                {DbType.Oracle,"Oracle database" },
            };
            var connectionString = connectionStringMap[sqlType];
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException($"Default Connection string for {dbTypeNameMap[sqlType]} is not setup.Please pass in connectionString or setup a default connection string.");
            return connectionString;
        }

        internal static void AddOutputParam(DynamicParameters dynamicParameters, string propName, Attribute[] customAttributes)
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
                if (attributeType == typeof(SpOutputGuid))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.Guid, direction: ParameterDirection.Output);
                if (attributeType == typeof(SpOutputDateTime))
                    dynamicParameters.Add(propName, dbType: System.Data.DbType.DateTime, direction: ParameterDirection.Output);
            }
        }

        #endregion
    }
}
