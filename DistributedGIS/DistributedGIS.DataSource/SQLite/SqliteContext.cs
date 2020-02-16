using DistributedGIS.DataSource.Inf;
using DistributedGIS.DataSource.SDO;
using DistributedGIS.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedGIS.DataSource.SQLite
{
    public class SqliteContext : BaseDataSource, IDataSourceContext
    {
        public SqliteContext(string connectionString)
        {
            base.Connection = connectionString;
        }

        public void Init()
        {
            try
            {
                Console.WriteLine();
                base.DataSorceClient = new SqlSugarClient(new ConnectionConfig()
                {
                    DbType = DbType.Sqlite,
                    ConnectionString = base.Connection,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    AopEvents = new AopEvents
                    {
                        OnLogExecuting = (sql, p) =>
                        {
                            Console.WriteLine(sql);
                            Console.WriteLine(string.Join(",", p?.Select(it => it.ParameterName + ":" + it.Value)));
                        }
                    }
                });
            }
            catch (Exception e)
            {
                LogUtil.Error(e.ToString());
                throw;
            }
        }
    }
}
