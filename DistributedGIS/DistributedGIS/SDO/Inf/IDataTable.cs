using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DistributedGIS.SDO.Inf
{
    public interface IDataTable
    {
        /// <summary>
        /// 将Feature加入到集合中
        /// </summary>
        /// <param name="newFeature"></param>
        void AddFeature(Feature newFeature);
        bool InsterFeatrueToTable(Feature feature);
        DataTable QueryFeatures(string sql);
      
    }
}
