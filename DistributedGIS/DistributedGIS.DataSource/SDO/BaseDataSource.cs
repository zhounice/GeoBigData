using DistributedGIS.DataSource.Inf;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedGIS.DataSource.SDO
{
    public abstract class BaseDataSource: IDisposable
    {
        public BaseDataSource(DbType dbType, string connection)
        {
            this.DbType = dbType;
            this.Connection = connection;
        }
        /// <summary>
        /// 数据类型
        /// </summary>
        public DbType DbType { get; set; }
        /// <summary>
        /// 链接字符串
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// 数据库链接客户端
        /// </summary>
        public SqlSugarClient DataSorceClient { get; set; }

        public void Dispose()
        {
            if (this.DataSorceClient != null)
            {
                this.DataSorceClient.Close();
                this.DataSorceClient.Dispose();
            }
        }
    }
}
