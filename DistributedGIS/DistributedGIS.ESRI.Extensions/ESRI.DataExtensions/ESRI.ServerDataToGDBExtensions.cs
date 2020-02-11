using DistributedGIS.SDO;
using DistributedGIS.Utils;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace DistributedGIS.ServerDataToGDBExtensions
{
    public static class ServerDataToGDBExtensions
    {
        public static void ServerDataToGDB(this SpatialDataObtain spatialDataObtain,string serverUrl, string WhereClause, string OutFields, int FeattureNumPerRead)
        {
            string message;
            string getUrl;
            IWorkspace workspace = null;
            IFeatureClass featureClass = null;
            int start = 104676;
            esriGeometryType esriGeometryType = esriGeometryType.esriGeometryAny;
            ISpatialReference spatialReference = null;
            while (true)
            {
                if (serverUrl[serverUrl.Length - 1] == '/')
                    getUrl = $"{serverUrl}query?";
                else
                    getUrl = $"{serverUrl}/query?";
                string newWhereClause = string.Format("{0}>={1} and {0}<{2}", "OBJECTID", start, start + FeattureNumPerRead);
                newWhereClause = WhereClause == "" ? newWhereClause : WhereClause + " and " + newWhereClause;
                start += FeattureNumPerRead;
                getUrl = $"{getUrl}where={newWhereClause}&outFields={(string.IsNullOrEmpty(OutFields) ? "*" : OutFields)}&returnGeometry=true&f=pjson";
                if (GetFilterCount(serverUrl, newWhereClause) <= 0)
                    break;
                //http://192.168.1.137:6080/arcgis/rest/services/HZDG/HZDGJSFAFW/MapServer/0/query?where=1%3D1&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=OBJECTID%2CXMLB&returnGeometry=true&maxAllowableOffset=10&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=html
                string queryResult = HttpGet(getUrl);
                FeatureClassSDO featureClassSDO = JsonConvert.DeserializeObject<FeatureClassSDO>(queryResult);
                JObject jObject = JObject.Parse(queryResult);


                if (featureClass == null)
                {
                    JArray keyValuePairs = jObject.Value<JArray>("fields");
                    esriGeometryType = GetGeometryTypeFromName(jObject.Value<string>("geometryType"));
                    JToken spatialRef = jObject.Value<JToken>("spatialReference");
                    int wkid = spatialRef.Value<int>("wkid");
                    spatialReference = GetSpatialRefFromWkid(wkid);
                    List<IField> fields = new List<IField>();
                    foreach (var item in keyValuePairs)
                    {
                        JObject itemObject = item as JObject;
                        IField field = new FieldClass();
                        IFieldEdit fieldEdit = field as IFieldEdit;
                        fieldEdit.Name_2 = itemObject.Value<string>("name");
                        fieldEdit.AliasName_2 = itemObject.Value<string>("alias");
                        fieldEdit.Type_2 = EnumUtil.GetEnumObjByName<esriFieldType>(itemObject.Value<string>("type"));//sourceFields.Field[FieldCount].Type;
                        fields.Add(field);
                    }

                    featureClass = CreateMemoryFeatureClass(fields, spatialReference, esriGeometryType, out workspace);
                }

                JArray features = jObject.Value<JArray>("features");
                List<Dictionary<string, object>> dicExportData = new List<Dictionary<string, object>>();
                foreach (var item in features)
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    JToken attributes = item.Value<JToken>("attributes");
                    foreach (var value in (attributes as JObject).Properties())
                    {
                        values.Add(value.Name, value.Value);
                    }
                    JToken geometryJson = item.Value<JToken>("geometry");
                    IGeometry geometry = ConvertToGeometry(geometryJson.ToString(), esriGeometryType);
                    values.Add("geometry", geometry);
                    dicExportData.Add(values);
                }

                exportDataToMemery(dicExportData, featureClass, spatialReference, out message);

            }

        }
        public static IGeometry ConvertToGeometry(string strJson, esriGeometryType type)
        {
            return ConvertToGeometry(strJson, type, false, false);
        }

        /// <summary>  
        /// JSON字符串转成IGeometry  
        /// </summary>  
        public static IGeometry ConvertToGeometry(string strJson, esriGeometryType type,
        bool bHasZ, bool bHasM)
        {
            IJSONReader jsonReader = new JSONReaderClass();
            jsonReader.ReadFromString(strJson);

            JSONConverterGeometryClass jsonCon = new JSONConverterGeometryClass();
            return jsonCon.ReadGeometry(jsonReader, type, bHasZ, bHasM);
        }
        public static IFeatureClass exportDataToMemery(List<Dictionary<string, object>> dicExportData, IFeatureClass memoryFeaClsOfIntersect, ISpatialReference pSpatialReference,
          out string msg)
        {
            msg = string.Empty;
            IWorkspaceEdit pWorkapceEdit = null;
            IFeatureBuffer pTargetFeatureBuffer = null;
            IFeatureCursor pTargetFeatureCursor = null;
            try
            {
                IDataset dataset = memoryFeaClsOfIntersect as IDataset;
                pWorkapceEdit = dataset.Workspace as IWorkspaceEdit;
                pWorkapceEdit.StartEditing(false);
                pWorkapceEdit.StartEditOperation();
                pTargetFeatureBuffer = memoryFeaClsOfIntersect.CreateFeatureBuffer();
                pTargetFeatureCursor = memoryFeaClsOfIntersect.Insert(true);
                IFields fieldsSource = memoryFeaClsOfIntersect.Fields;//插入
                                                                      // fieldsSource.
                foreach (var item in dicExportData)
                {
                    foreach (var key in item.Keys)
                    {
                        int i = fieldsSource.FindField(key);

                        if (string.Equals(key, "geometry"))
                            pTargetFeatureBuffer.Shape = (IGeometry)item[key];

                        if (i > -1)
                        {
                            if (fieldsSource.Field[i].Type == esriFieldType.esriFieldTypeOID || fieldsSource.Field[i].Type == esriFieldType.esriFieldTypeGeometry)
                                continue;
                            if (item[key] != null)
                            {
                                pTargetFeatureBuffer.Value[i] = item[key];
                            }
                        }
                    }
                    pTargetFeatureCursor.InsertFeature(pTargetFeatureBuffer);

                }
                pTargetFeatureCursor.Flush();
                pWorkapceEdit.StopEditOperation();
                pWorkapceEdit.StopEditing(true);

            }
            catch (Exception e)
            {
                msg = e.Data + e.HelpLink + e.HResult + e.InnerException + e.Message + e.Source;
            }
            finally
            {
                if (pWorkapceEdit != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pWorkapceEdit);
                if (pTargetFeatureBuffer != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pTargetFeatureBuffer);
                if (pTargetFeatureCursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pTargetFeatureCursor);
            }
            return memoryFeaClsOfIntersect;
        }
        public static ISpatialReference GetSpatialRefFromWkid(int wkid)
        {
            try
            {
                if (wkid == 0)
                    return null;
                ISpatialReferenceFactory srf = new SpatialReferenceEnvironmentClass();
                ISpatialReference spaRef;
                try
                {
                    spaRef = srf.CreateProjectedCoordinateSystem(wkid);
                }
                catch (Exception)
                {

                    spaRef = srf.CreateGeographicCoordinateSystem(wkid);
                }

                return spaRef;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        public static IFeatureClass CreateMemoryFeatureClass(List<IField> dicExportData, ISpatialReference spatialReference, esriGeometryType geometryType, out IWorkspace memoryWS, string name = "Temp", bool isCad = false)
        {
            try
            {

                // 创建内存工作空间  
                IWorkspaceFactory pWSF = new FileGDBWorkspaceFactoryClass();
                IWorkspaceName pWSName = pWSF.Create(@"D:\FTP\TestData\test.gdb", name, null, 0);
                IName pName = (IName)pWSName;
                memoryWS = (IWorkspace)pName.Open();

                IField field = new FieldClass();
                IFields fields = new FieldsClass();
                IFieldsEdit fieldsEdit = fields as IFieldsEdit;
                IFieldEdit fieldEdit = field as IFieldEdit;

                fieldEdit.Name_2 = "OBJECTID";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldEdit.IsNullable_2 = false;
                fieldEdit.Required_2 = false;
                fieldsEdit.AddField(field);

                field = new FieldClass();
                fieldEdit = field as IFieldEdit;
                IGeometryDef geoDef = new GeometryDefClass();
                IGeometryDefEdit geoDefEdit = (IGeometryDefEdit)geoDef;
                geoDefEdit.AvgNumPoints_2 = 5;
                geoDefEdit.GeometryType_2 = geometryType;
                geoDefEdit.GridCount_2 = 1;
                geoDefEdit.HasM_2 = false;
                geoDefEdit.HasZ_2 = false;
                geoDefEdit.SpatialReference_2 = spatialReference;


                //Console.WriteLine();
                fieldEdit.Name_2 = "SHAPE";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                fieldEdit.GeometryDef_2 = geoDef;
                fieldEdit.IsNullable_2 = true;
                fieldEdit.Required_2 = true;
                fieldsEdit.AddField(field);

                foreach (var item in dicExportData)
                {
                    if (item.Type == esriFieldType.esriFieldTypeOID || item.Type == esriFieldType.esriFieldTypeGeometry)
                        continue;
                    fieldsEdit.AddField(item);
                }

                //创建要素类  
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)memoryWS;
                IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(
                    name, fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");

                return featureClass;
            }
            catch (Exception e)
            {
                memoryWS = null;
                return null;
            }
        }
        public static int GetFilterCount(string serverUrl, string WhereClause)
        {
            string getUrl;
            if (serverUrl[serverUrl.Length - 1] == '/')
                getUrl = $"{serverUrl}query?";
            else
                getUrl = $"{serverUrl}/query?";
            getUrl = $"{getUrl}where={WhereClause}&returnCountOnly=true&f=pjson";
            string queryResult = HttpGet(getUrl);
            JObject jObject = JObject.Parse(queryResult);
            return jObject.Value<int>("count");
        }
        public static string HttpGet(string Url, string contentType = "application/json")
        {
            try
            {
                string retString = string.Empty;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.ContentType = contentType;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(myResponseStream);
                retString = streamReader.ReadToEnd();
                streamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static esriFieldType GetFieldTypeFromName(string enumName)
        {
            return EnumUtil.GetEnumObjByName<esriFieldType>(enumName);
        }
        public static esriGeometryType GetGeometryTypeFromName(string enumName)
        {
            return EnumUtil.GetEnumObjByName<esriGeometryType>(enumName);
        }

    }
}
