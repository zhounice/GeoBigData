using DistributedGIS.DataSource.SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedGIS.SDO
{
    public class DataTableBase
    {
        public SqliteDatasource sqliteDatasource { get; set; }
        public bool AddFeatureToTable(Feature feature)
        {
            
            return true;
        }
    }
}
