using DistributedGIS.SDO.Inf;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedGIS.SDO
{
    public class Feature:IDataItem
    {
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
        public Geometry Geometry { get; set; }
    }
}
