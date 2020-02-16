using DistributedGIS.DataSource.SDO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedGIS.SDO.Inf
{
    public interface IDistributedDataSource
    {
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="dataSourceSDO">分布式链接方式</param>
        /// <returns></returns>
        bool Save(BaseDataSource dataSourceSDO);
        /// <summary>
        /// 获取要素
        /// </summary>
        /// <param name="dataSourceSDO"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        IDataItem GetFeature(BaseDataSource dataSourceSDO, string sql);
        /// <summary>
        /// 获取要素
        /// </summary>
        /// <param name="dataSourceSDO"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        IDataTable GetFeatureClass(BaseDataSource dataSourceSDO);
    }
}
