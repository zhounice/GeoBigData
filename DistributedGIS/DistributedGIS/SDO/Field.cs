using DistributedGIS.SDO.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedGIS.SDO
{
    public class Field
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public enumFieldType Type { get; set; }
        /// <summary>
        /// 字段别名
        /// </summary>
        public string Alias { get; set; }
    }
}
