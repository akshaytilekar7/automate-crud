using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
namespace Database.Core
{
    public static class Extensions
    {
        /// <summary>
        /// create list of repective type from datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToCustomList<T>(this DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        /// <summary>
        /// create object of repective type from datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T ToCustomObject<T>(this DataTable dt)
        {
            T data = Activator.CreateInstance<T>();
            foreach (DataRow row in dt.Rows)
            {
                return GetItem<T>(row);
            }
            return data;
        }

        /// <summary>
        /// Helper method 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    string primaryKeyName = AttributeHeler.GetColumnName(pro);
                    if (primaryKeyName == column.ColumnName && dr[column.ColumnName] != DBNull.Value)
                    {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        /// <summary>
        /// create list of repective type from datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ToCustomListOfDictinory<T>(this DataTable dt)
        {
            List<Dictionary<string,object>> data = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                data.Add(GetItemExpandoObject<T>(row));
            }
            return data;
        }

        public static Dictionary<string, object> ToCustomObjectOfDictinory<T>(this DataTable dt)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            foreach (DataRow row in dt.Rows)
            {
                return GetItemExpandoObject<T>(row);
            }
            return null;
        }



        private static Dictionary<string, object> GetItemExpandoObject<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            Dictionary<string, object> dics = new Dictionary<string, object>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                dics.Add(column.ColumnName, dr[column]);
            }

            return dics;
        }

    }
}