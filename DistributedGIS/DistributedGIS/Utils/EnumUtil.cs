using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace DistributedGIS.Utils
{
    public sealed class EnumUtil
    {
        /// <summary>
        /// 获取一个枚举值的中文描述
        /// </summary>
        /// <param name="enum">枚举值</param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum @enum)
        {
            FieldInfo fi = @enum.GetType().GetField(@enum.ToString());
            DescriptionAttribute[] arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return arrDesc[0].Description;
        }
        /// <summary>
        /// 获取枚举显示名称
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetEnumDisplayName(Enum @enum)
        {
            FieldInfo fi = @enum.GetType().GetField(@enum.ToString());
            DisplayAttribute[] arr = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);
            return arr[0].Name;//.DisplayName;
        }
        /// <summary>
        /// 根据值，获取枚举的名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumName<T>(int value)
        {
            return Enum.GetName(typeof(T), value);
        }
        /// <summary>
        /// 根据名称（不区分大小写）获取枚举对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetEnumObjByName<T>(string name)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), name, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default(T);
        }
        /// <summary>
        /// 根据值获取枚举对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetEnumObjByValue<T>(int value)
        {
            return (T)Enum.Parse(typeof(T), value.ToString());
        }
        /// <summary>
        /// 根据显示名称获取枚举对象（第一个匹配的显示名称）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static T GetEnumObjByDisplayName<T>(string displayName)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DisplayAttribute)) is DisplayAttribute attribute)
                {
                    if (string.Equals(attribute.Name, displayName, StringComparison.OrdinalIgnoreCase))
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }
            throw new Exception( $"枚举类型[{typeof(T)}]没有显示名称为[{displayName}]的枚举对象。");
        }
    }
}
