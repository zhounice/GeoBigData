using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Linq;

namespace DistributedGIS.Utils
{
    public class SpatialDataObtain
    {
        public static int MinWorkThreadCount = Environment.ProcessorCount * 4;
        public static Polygon GeometryJsonToPolygon(JObject polygonJson)
        {
            if (polygonJson == null)
                throw new ArgumentNullException(nameof(polygonJson));
            try
            {
                JArray rings = polygonJson.Value<JArray>("rings");
                LinearRing shell = ToMakeLinearRingFromPoints(rings[0].Value<JArray>());
                LinearRing[] holes = null;
                int ringscount = rings.Count;
                if (ringscount - 1 > 0)
                {
                    holes = new LinearRing[ringscount - 1];
                    for (int i = 1; i < ringscount; i++)
                    {
                        LinearRing hole = ToMakeLinearRingFromPoints(rings[i].Value<JArray>());
                        holes[i - 1] = hole;
                    }
                }
                return new Polygon(shell, holes);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static LinearRing ToMakeLinearRingFromPoints(JArray points)
        {

            if (points == null)
                throw new ArgumentNullException(nameof(points));

            int count = points.Count;
            if (count == 0)
                throw new ArgumentException("points's count is zero");
            Coordinate[] coordinates = new Coordinate[count];
            for (int i = 0; i < count; i++)
            {
                JArray values = points[i].Value<JArray>();
                Coordinate coordinate = new Coordinate(values[0].Value<double>(), values[1].Value<double>());
                coordinates[i] = coordinate;
            }
            return new LinearRing(coordinates);
        }
    }
}
