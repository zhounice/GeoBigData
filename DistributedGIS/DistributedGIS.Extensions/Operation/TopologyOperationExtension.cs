using DistributedGIS.SDO;
using DistributedGIS.SDO.Enums;
using DistributedGIS.Utils;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Overlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGIS.Extensions.Operation
{
    public static class TopologyOperationExtension
    {
        public static FeatureClassSDO Intersect(this FeatureClassSDO featureClassSelf, FeatureClassSDO featureClassOther)
        {
            FeatureClassSDO newFeatureClass = null;
            try
            {
                if (featureClassSelf == null || featureClassOther == null)
                    throw new ArgumentException("对象未实例化");
                if (featureClassSelf.GeometryTypeNet != featureClassOther.GeometryTypeNet)
                    throw new ArgumentException("空间对象类型错误");
                if (CompareProperties(featureClassSelf.spatialReference, featureClassOther.spatialReference, typeof(object)))
                    throw new ArgumentException("空间坐标系不一致");
                newFeatureClass = new FeatureClassSDO()
                {
                    Name = $"{featureClassSelf.Name}_{featureClassOther.Name}",
                    DisplayFieldName = $"{featureClassSelf.DisplayFieldName}_{featureClassOther.DisplayFieldName}",
                    Fields = featureClassSelf.Fields,
                    GeometryType = featureClassSelf.GeometryType,
                    GeometryTypeNet = featureClassSelf.GeometryTypeNet,
                    spatialReference= featureClassSelf.spatialReference
                };
                foreach (var item in featureClassOther.Fields)
                {
                    if (item.Type == enumFieldType.esriFieldTypeGeometry || item.Name.ToLower() == "shape_area")
                        continue;
                    newFeatureClass.Fields.Add(new Field()
                    {
                        Alias = $"{featureClassOther.Name}_{item.Alias}",
                        Type = item.Type,
                        Name = $"{featureClassOther.Name}_{item.Name}"
                    });
                }
                int groupCount = featureClassOther.Features.Count / SpatialDataObtain.MinWorkThreadCount;
                Parallel.ForEach(featureClassOther.Features, (otherItem) =>
                {
                    foreach (var selfItem in featureClassSelf.Features)
                    {
                        if (otherItem.Geometry.Intersects(selfItem.Geometry))
                        {
                            OverlayOp overlayOp = new OverlayOp(otherItem.Geometry, selfItem.Geometry);
                            Geometry newGeometry = overlayOp.GetResultGeometry(SpatialFunction.Intersection);
                           
                            Feature newFeature = new Feature();
                            newFeature.Attributes = new Dictionary<string, object>(selfItem.Attributes);
                            foreach (var oterAttribute in otherItem.Attributes)
                            {
                                Field field = featureClassOther.Fields.Single(item => item.Name == oterAttribute.Key);
                                if (field.Type == enumFieldType.esriFieldTypeGeometry || field.Name.ToLower() == "shape_area")
                                    continue;
                                string newKey = $"{featureClassOther.Name}_{field.Name}";
                                newFeature.Attributes.Add(newKey, oterAttribute.Value);
                            }
                            newFeature.Geometry = newGeometry;
                            newFeatureClass.AddFeature(newFeature);
                        }                        
                    }
                });
                return newFeatureClass;
            }
            catch (Exception e)
            {
                LogUtil.Error(e.ToString());
                throw;
            }

        }
        /// <summary>
        /// 判断两个相同引用类型的对象的属性值是否相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1">对象1</param>
        /// <param name="obj2">对象2</param>
        /// <param name="type">按type类型中的属性进行比较</param>
        /// <returns></returns>
        public static bool CompareProperties<T>(T obj1, T obj2, Type type)
        {
            //为空判断
            if (obj1 == null && obj2 == null)
                return true;
            else if (obj1 == null || obj2 == null)
                return false;

            Type t = type;
            PropertyInfo[] props = t.GetProperties();
            foreach (var po in props)
            {
                if (IsCanCompare(po.PropertyType))
                {
                    if (!po.GetValue(obj1).Equals(po.GetValue(obj2)))
                    {
                        return false;
                    }
                }
                else
                {
                    return CompareProperties(po.GetValue(obj1), po.GetValue(obj2), po.PropertyType);
                }
            }

            return true;
        }

        /// <summary>
        /// 该类型是否可直接进行值的比较
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsCanCompare(Type t)
        {
            if (t.IsValueType)
            {
                return true;
            }
            else
            {
                //String是特殊的引用类型，它可以直接进行值的比较
                if (t.FullName == typeof(String).FullName)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
