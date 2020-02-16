using DistributedGIS.SDO.Inf;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using static NetTopologySuite.Geometries.Geometry;

namespace DistributedGIS.SDO
{
    public class FeatureClassSDO:IDataTable
    {
        public string Name { get; set; }
        public string DisplayFieldName { get; set; }
        public Dictionary<string, string> FieldAliases { get; set; } = new Dictionary<string, string>();
        public List<Field> Fields { get; set; } = new List<Field>();
        public string GeometryType { get; set; }
        public enumGeometryTypeNet GeometryTypeNet { get; set; }
        public object spatialReference { get; set; }
        public ConcurrentQueue<Feature> Features { get; set; } = new ConcurrentQueue<Feature>();
        public Envelope Envelope { get; set; }
    }
}
