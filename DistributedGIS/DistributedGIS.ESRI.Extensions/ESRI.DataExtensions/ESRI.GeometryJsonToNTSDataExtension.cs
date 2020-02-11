using DistributedGIS.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Linq;
using ESRI.ArcGIS.Geometry;
using DistributedGIS.ESRI.Extensions.ESRI.Opeation;

namespace DistributedGIS.ESRI.Extensions.ESRI.DataExtensions
{
    public static class GeometryJsonToNTSDataExtensions
    {
        public static NetTopologySuite.Geometries.Polygon GeometryJsonToPolygon(this SpatialDataObtain spatialDataObtain,IPolygon polygon)
        {
            string geometryJson = ESRIDataOpention.ConvertGeometryToJson(polygon);
            if(string.IsNullOrEmpty(geometryJson))
                 geometryJson = ESRIDataOpention.ConvertGeometryToJson(polygon,true);
            return SpatialDataObtain.GeometryJsonToPolygon(JObject.Parse(geometryJson));
        }
        
    }
}
