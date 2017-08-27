using System;
using System.Collections.Generic;

namespace Helper_9H
{
    public static class ParseHelper
    {
        public static DateTime ToDateTime(this object val, DateTime defVal = default(DateTime))
        {
            return val.To<DateTime>(defVal);
        }

        public static Int64 ToInt64(this object val, Int64 defVal = default(Int64))
        {
            return val.To<Int64>(defVal);
        }

        public static int ToInt(this object val, int defVal = default(int))
        {
            return val.To<int>(defVal);
        }

        /// <summary>
        /// 通用类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static T To<T>(this object val, T defVal = default(T))
        {
            if (val == null)
                return (T)defVal;
            if (val is T)
                return (T)val;

            Type type = typeof(T);
            try
            {
                if (type.BaseType == typeof(Enum))
                {
                    return (T)Enum.Parse(type, val.ToString(), true);
                }
                else
                {
                    return (T)Convert.ChangeType(val, type);
                }
            }
            catch
            {
                return defVal;
            }
        }
    }
}
