using DistributedGIS.SDO;
using DistributedGIS.Utils;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedGIS.ESRI.Extensions.ESRI.Opeation
{
    public class ESRIDataOpention
    {
        public static enumSpatialRelType GetSpatialRelType(ISpatialReference spatialReference)
        {
            enumSpatialRelType spatialRelType = enumSpatialRelType.Unknown;
            if (spatialReference == null) return spatialRelType;

            //地理坐标系统
            IGeographicCoordinateSystem geographicCoordinateSystem = spatialReference as IGeographicCoordinateSystem;
            if (geographicCoordinateSystem != null)
            {
                spatialRelType = enumSpatialRelType.GCS;
                return spatialRelType;
            }

            //投影坐标系统
            IProjectedCoordinateSystem projectedCoordinateSystem = spatialReference as IProjectedCoordinateSystem;
            if (projectedCoordinateSystem != null)
            {
                spatialRelType = enumSpatialRelType.PCS;
                return spatialRelType;
            }
            return spatialRelType;
        }
        public static string ConvertGeometryToJson(IGeometry geometry,
          bool isGeneralize = false)
        {
            string geomJsonStr = null;
            try
            {
                ITopologicalOperator topoGeom = geometry as ITopologicalOperator;
                topoGeom.Simplify();
                //IPolygon polygon = topoGeom as IPolygon;
                //if (polygon != null)
                //{
                //    polygon.Generalize(1);
                //}

                //if (geometry.SpatialReference == null || geometry.SpatialReference.Name == "Unknown")
                //{
                //    geometry.SpatialReference = SpatialRefOpt.GetProjectedCoordinate(esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_CM_120E);
                //}

                if (isGeneralize)
                {
                    ISpatialReference spatialReference = geometry.SpatialReference;
                    enumSpatialRelType spatialRelType = GetSpatialRelType(spatialReference);
                    double offset = 0.000001;
                    if (spatialRelType == enumSpatialRelType.GCS) offset = 0.00000001;
                    IPolycurve polycurve = geometry as IPolycurve;
                    polycurve.Generalize(offset);
                }

               IJSONWriter jsonWriter = new JSONWriterClass();
                jsonWriter.WriteToString();

               JSONConverterGeometryClass jsonCon = new JSONConverterGeometryClass();
                jsonCon.WriteGeometry(jsonWriter, null, geometry, false);

                geomJsonStr = Encoding.UTF8.GetString(jsonWriter.GetStringBuffer());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("\nConvertGeometryToJson::error::" + ex.Source + ".\n" + ex.ToString());
            }
            return geomJsonStr;
        }
    }
}
