using DistributedGIS.SDO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedGIS.Utils
{
    public class FeatureClassDTOUtil
    {
        public static FeatureClassSDO CopyInfoWithoutFeatures(FeatureClassSDO featureClassSDO)
        {
            return new FeatureClassSDO()
            {
                Name = featureClassSDO.Name,
                DisplayFieldName = featureClassSDO.DisplayFieldName,
                FieldAliases = featureClassSDO.FieldAliases,
                Fields = featureClassSDO.Fields,
                GeometryType = featureClassSDO.GeometryType,
                GeometryTypeNet = featureClassSDO.GeometryTypeNet,
                spatialReference = featureClassSDO.spatialReference,
                Features = new System.Collections.Concurrent.ConcurrentBag<Feature>()
            };
        }
    }
}
