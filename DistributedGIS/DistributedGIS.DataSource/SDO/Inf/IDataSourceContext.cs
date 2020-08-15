using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DistributedGIS.DataSource.Inf
{
    public interface IDataSourceContext
    { 
        /// <summary>
        /// 数据信息实例化
        /// </summary>
        void Init();
        /// <summary>
        /// 使用原生SQL插入Feature
        /// </summary>
        /// <param name="insterSql">原生SQL</param>
        /// <returns></returns>
        bool InsterBySQL(string insterSql);
        /// <summary>
        /// 使用原生SQL查询
        /// </summary>
        /// <param name="querSQL"></param>
        /// <returns>查询结果</returns>
        DataTable QuerySql(string querSQL);


        
    }
}
