using DistributedGIS.SDO.Inf;
using DistributedGIS.Utils;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static NetTopologySuite.Geometries.Geometry;

namespace DistributedGIS.SDO
{
    public class FeatureClassSDO: DataTableBase,IDataTable, IDisposable
    {
        #region 公共属性
        public string Name { get; set; }
        public string DisplayFieldName { get; set; }
        public Dictionary<string, string> FieldAliases { get; set; } = new Dictionary<string, string>();
        public List<Field> Fields { get; set; } = new List<Field>();
        public string GeometryType { get; set; }
        public enumGeometryTypeNet GeometryTypeNet { get; set; }
        public object spatialReference { get; set; }
        public ConcurrentBag<Feature> Features { get; set; } = new ConcurrentBag<Feature>();
        public Envelope Envelope { get; set; }
        #endregion
        #region 私有属性
        private int InsterCount=0;
        #endregion
        #region 公共方法
        public void AddFeature(Feature newFeature)
        {
            Features.Add(newFeature);
            Envelope envelope = newFeature.Geometry.EnvelopeInternal;
            if (this.Envelope == null)
            {
                this.Envelope = envelope;
            }
            else
            {
                double minx = this.Envelope.MinX < envelope.MinX ? this.Envelope.MinX : envelope.MinX;
                double maxx = this.Envelope.MaxX > envelope.MaxX ? this.Envelope.MaxX : envelope.MaxX;
                double miny = this.Envelope.MinY < envelope.MinY ? this.Envelope.MinY : envelope.MinY;
                double maxy = this.Envelope.MaxY > envelope.MaxY ? this.Envelope.MaxY : envelope.MaxY;
                this.Envelope = new Envelope(minx, maxx, miny, maxy);
            }
        }

        public void Dispose()
        {
            base.sqliteDatasource.Dispose();
            Features.Clear();
            GC.Collect();
        }

        public bool InsterFeatrueToTable(Feature feature)
        {
            try
            {
                foreach (var item in feature.Attributes.Keys)
                {

                }
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Error(e.ToString());
                throw;
            }
        }

        public DataTable QueryFeatures(string sql)
        {
            DataTable dataTable = null;
            try
            {
                return dataTable;
            }
            catch (Exception e)
            {
                LogUtil.Error(e.ToString());
                throw;
            }
        }
        #endregion
        #region 私有方法
        private bool CreateTable(string tableName)
        {
            try
            {

                return true;
            }
            catch (Exception e)
            {
                LogUtil.Error(e.ToString());
                throw;
            }
        }
        private string GetInsterSQL(Feature feature)
        {
            string sql = null;
            try
            {

                return sql;
            }
            catch (Exception e)
            {
                LogUtil.Error(e.ToString());
                throw;
            }
        }
        #endregion

    }
}
