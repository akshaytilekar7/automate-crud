using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Database.Core
{
    public static class AttributeHeler
    {

        /// <summary>
        /// return database column name respective to class property
        /// check if property have columnattribute or not 
        /// if yes return columnattribute constructor parameter 
        /// if no return property name as it is
        /// </summary>
        /// <param name="pro">property of class [ex. name or salary etc]</param>
        /// <returns></returns>
        public static string GetColumnName(PropertyInfo pro)
        {
            CustomAttributeData cusAttr = pro.CustomAttributes.FirstOrDefault(c => c.AttributeType == typeof(ColumnNameAttribute));
            return cusAttr == null ? pro.Name : Convert.ToString(cusAttr.ConstructorArguments.FirstOrDefault().Value);
        }

        /// <summary>
        /// true if property is primary key in table
        /// else false
        /// </summary>
        /// <param name="property">property of class [ex. name or salary etc]</param>
        /// <returns></returns>
        public static bool IsPrimaryKeyAttribute(PropertyInfo property)
        {
            return Attribute.IsDefined(property, typeof(PrimaryKeyAttribute));
        }

        /// <summary>
        /// if property have ignorecolumnattribute 
        /// then return true
        /// else false
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static bool CheckIgnoreColumn(PropertyInfo pro)
        {
            CustomAttributeData cusAttr = pro.CustomAttributes.FirstOrDefault(c => c.AttributeType == typeof(IgnoreColumnAttribute));
            return cusAttr == null ? false : true;
        }

        public static bool IsColumnNameAttribute(PropertyInfo property)
        {
            return Attribute.IsDefined(property, typeof(ColumnNameAttribute));
        }
    }
}
