using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newcats.HangFire.HostServer.Models;

namespace Newcats.HangFire.HostServer.Repositories
{
    public class JobRepository : IJobRepository
    {
        /// <summary>
        /// 解密后的数据库连接字符串字典
        /// </summary>
        private static readonly ConcurrentDictionary<string, string> _connStrDic = new ConcurrentDictionary<string, string>();

        public JobRepository()
        {
            Connection = CreateDbConnection();
        }

        /// <summary>
        /// 1.数据库连接,在构造函数初始化(默认连接为"DefaultConnection")。
        /// 2.若要使用非默认的数据库连接，请重新赋值。
        /// 3.一般在Service类的构造函数赋值_repository.Connection=_repository.CreateDbConnection("OtherDB")。
        /// </summary>
        public IDbConnection Connection { get; set; }

        /// <summary>
        /// 1.根据应用程序执行目录下的appsettings.json配置文件(默认ConnectionStrings:DefaultConnection)的连接字符串创建数据库连接
        /// 2.会在构造函数自动调用并赋值，不需要手动调用，除非需要使用非默认的数据库连接
        /// </summary>
        /// <param name="key">连接字符串名，默认为"DefaultConnection"</param>
        /// <returns>数据库连接</returns>
        public IDbConnection CreateDbConnection(string key = "DefaultConnection")
        {
            //return new SqlConnection("Data Source = .; Initial Catalog =NewcatsDB; User ID = sa; Password = 123456;");
            return new SqlConnection(GetConnectionString(key));
        }

        /// <summary>
        /// 1.获取应用程序执行目录下的appsettings.json配置文件(默认ConnectionStrings:DefaultConnection)里的连接字符串
        /// 2.此处有缓存，如果更改了配置文件，请重新启动应用程序
        /// </summary>
        /// <param name="key">连接字符串名称</param>
        /// <returns>解密之后的连接字符串</returns>
        private string GetConnectionString(string key)
        {
            string dicKey = $"connStr_{key}";
            string connStr = "";
            if (_connStrDic.TryGetValue(dicKey, out connStr))
            {
                if (!string.IsNullOrWhiteSpace(connStr))
                    return connStr;
            }
            connStr = AppSettings.GetConnectionString(key);
            if (string.IsNullOrWhiteSpace(connStr))
            {
                throw new KeyNotFoundException($"The config item ConnectionStrings:{key} do not exists on file appsettings.json");
            }
            _connStrDic[dicKey] = connStr;
            return connStr;
        }

        public async Task<int> ExecuteStoredProcedureAsync(string storedProcedureName, DynamicParameters pars, int? commandTimeout = null)
        {
            if (string.IsNullOrWhiteSpace(storedProcedureName))
                throw new ArgumentNullException(nameof(storedProcedureName));
            return await Connection.ExecuteAsync(storedProcedureName, pars, null, commandTimeout, CommandType.StoredProcedure);
        }
    }
}