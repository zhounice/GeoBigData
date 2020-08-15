using DistributedGIS.DataSource.Inf;
using DistributedGIS.DataSource.SDO;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DistributedGIS.DataSource.SQLite
{
    public class SqliteDatasource : BaseDataSource, IDataSourceContext
    {
        public SqliteDatasource(string connectionString):base(SqlSugar.DbType.Sqlite,connectionString)
        {
            Init();
        }

       
        public void Init()
        {
            try
            {
                Console.WriteLine();
                base.DataSorceClient = new SqlSugarClient(new ConnectionConfig()
                {
                    DbType = SqlSugar.DbType.Sqlite,
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
                Console.WriteLine(e.ToString());
                throw;
            }
        }

        public bool InsterBySQL(string insterSql)
        {
            throw new NotImplementedException();
        }


        DataTable IDataSourceContext.QuerySql(string querSQL)
        {
            throw new NotImplementedException();
        }
    }
}
