using DistributedGIS.Utils;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGIS.SDO
{
    public class SpatialIndexNode
    {
        public SpatialIndexNode(Envelope envelope, string name, int leve, int SubnodeIndex, string parentCode = null)
        {
            this.IndexLeve = leve;
            this.Envelope = envelope;
            this.ParentCode = parentCode;
            this.DataBlockCode = string.IsNullOrEmpty(parentCode) ? "0" : $"0{SubnodeIndex}";
            this.DataBlockName = $"{name}_{DataBlockCode}";

        }
        public string ParentCode { get; set; }
        public string DataBlockCode { get; set; }
        public string DataBlockName { get; set; }
        /// <summary>
        /// 数据块
        /// </summary>
        public FeatureClassSDO DataBlock { get; set; }
        /// <summary>
        /// 空间索引的范围
        /// </summary>
        public Envelope Envelope { get; set; }
        /// <summary>
        /// 四叉树索引层级
        /// 从0开始
        /// </summary>
        public int IndexLeve { get; set; }

        /// <summary>
        /// 子四边形编号如下:
        /// 2 | 3
        /// --+--
        /// 0 | 1
        /// </summary>
        private SpatialIndexNode[] Subnode = new SpatialIndexNode[4];
        private Envelope[] CreateSubdeEnvelope(Envelope envelope)
        {
            Envelope[] envelopes = new Envelope[4];
            double _centreX = (envelope.MaxX + Envelope.MinX) / 2;
            double _centreY = (envelope.MaxY + Envelope.MinY) / 2;
            try
            {
                double minx = 0.0;
                double maxx = 0.0;
                double miny = 0.0;
                double maxy = 0.0;
                //0:
                minx = envelope.MinX;
                maxx = _centreX;
                miny = envelope.MinY;
                maxy = _centreY;
                envelopes[0] = new Envelope(minx, maxx, miny, maxy);
                //1:
                minx = _centreX;
                maxx = envelope.MaxX;
                miny = envelope.MinY;
                maxy = _centreY;
                envelopes[1] = new Envelope(minx, maxx, miny, maxy);
                //2:
                minx = envelope.MinX;
                maxx = _centreX;
                miny = _centreY;
                maxy = envelope.MaxY;
                envelopes[2] = new Envelope(minx, maxx, miny, maxy);
                //3:
                minx = _centreX;
                maxx = envelope.MaxX;
                miny = _centreY;
                maxy = envelope.MaxY;
                envelopes[3] = new Envelope(minx, maxx, miny, maxy);

            }
            catch (Exception e)
            {
                LogUtil.Error(e.ToString());
                throw e;
            }
            return envelopes;
        }

        /// <summary>
        /// 创建3级索引
        /// </summary>
        /// <param name="featureClassSDO"></param>
        public void CreateSpatialIndex(FeatureClassSDO featureClassSDO)
        {
            if (string.IsNullOrEmpty(this.DataBlockCode) || string.Equals("0", this.DataBlock))
            {
                LogUtil.Error("不是第一个节点");
                throw new ArgumentException("该节点不是第一个节点");
            }
            Envelope[] envelopes = CreateSubdeEnvelope(featureClassSDO.Envelope);
            int featuresCount = featureClassSDO.Features.Count;
            Parallel.For(0, featuresCount, (t) =>
            {
                Feature feature;
                featureClassSDO.Features.TryTake(out feature);
                for (int i = 0; i < 4; i++)
                {
                    Envelope envelope = envelopes[i];
                    SpatialIndexNode spatialIndexNode = new SpatialIndexNode(envelope, featureClassSDO.Name,
                     this.IndexLeve, i, this.ParentCode);
                    this.Subnode[i] = spatialIndexNode;
                    if (envelope.Contains(feature.Geometry.EnvelopeInternal))
                    {
                        spatialIndexNode.CreateSpatialIndex(feature, envelope);
                    }
                    else
                    {
                        this.DataBlock.AddFeature(feature);
                    }
                }

            });

        }
        /// <summary>
        /// 向下层级创建索引
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="envelope"></param>
        private void CreateSpatialIndex(Feature feature, Envelope envelope)
        {
            Envelope[] envelopes = CreateSubdeEnvelope(envelope);
            
            for (int i = 0; i < 4; i++)
            {
                Envelope envelopeTemp = envelopes[i];
                SpatialIndexNode spatialIndexNode = new SpatialIndexNode(envelope, this.DataBlockName,
                 this.IndexLeve, i, this.ParentCode);
                this.Subnode[i] = spatialIndexNode;
                if (envelope.Contains(feature.Geometry.EnvelopeInternal))
                {
                    if (this.DataBlockCode.Length == 4)
                    {
                        this.DataBlock.AddFeature(feature);
                        return;
                    }
                    spatialIndexNode.CreateSpatialIndex(feature, envelopeTemp);
                    return;
                }
            }
            this.DataBlock.AddFeature(feature);

        }



    }
}
