using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace OAuthDemo.Core.Infrastructure
{
    public class DefaultDataSource : IDataSource
    {
        private const string DbName = "test";

        public IDbConnection GetConnection()
        {
            var connectionSetting = ConfigurationManager.ConnectionStrings[DbName];
            if (connectionSetting == null)
            {
                throw new Exception($"缺少{DbName}");
            }

            return new SqlConnection(connectionSetting.ConnectionString);
        }
    }
}