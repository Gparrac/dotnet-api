using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotnetAPI
{
    class DataContextDapper(IConfiguration config)
    {
        private readonly IDbConnection dbConnection = new SqlConnection(config.GetConnectionString("connection"));

        public IEnumerable<T> LoadData<T>(string sql)
        {
            return dbConnection.Query<T>(sql);
        }
        public T LoadDataSingle<T>(string sql)
        {
            return dbConnection.QuerySingle<T>(sql);
        }
        /*
         The next two methods are vulnerables to SQL injection due
         they don't handle parameters and stright interpolate  ⚠️
         */
        public bool ExecuteSql(string sql)
        {
            return dbConnection.Execute(sql) > 0;
        }
        public int ExecuteSqlWithRow(string sql)
        {
            return dbConnection.Execute(sql);
        }
        // however this is the solution
        public bool ExecuteSqlWithParams(string sql, object parameters)
        {
            return dbConnection.Execute(sql, parameters) > 0;
        }
    }
}