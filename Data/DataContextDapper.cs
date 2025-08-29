using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotnetAPI
{
    class DataContextDapper
    {
        private readonly IConfiguration _config;
        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }
        public IEnumerable<T> LoadData<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("connection"));
            return dbConnection.Query<T>(sql);
        }
        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("connection"));
            return dbConnection.QuerySingle<T>(sql);
        }
        public bool ExecuteSql(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("connection"));
            return dbConnection.Execute(sql) > 0;
        }
        public int ExecuteSqlWithRow(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("connection"));
            return dbConnection.Execute(sql);
        }
    }
}