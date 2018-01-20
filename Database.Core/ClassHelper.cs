using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Database.Core
{
    public static class ClassHelper<MyType>
    {
        /// <summary>
        /// generate sql paramater in single quotes
        /// like 'akshay' or '2016/01/01' or 'true'
        /// </summary>
        /// <param name="sqlstring"></param>
        /// <returns></returns>
        public static string CreateSqlString(object sqlString)
        {
            return "'" + sqlString + "'";
        }

        /// <summary>
        /// set value to property respective to database
        /// check data type and according to that generate value
        /// </summary>
        /// <param name="propertyname">name of property</param>
        /// <param name="value">value of propeery</param>
        /// <param name="baseentity">data type of class ie, class type</param>
        /// <returns></returns>
        public static object SetValue(string propertyName, object value, MyType baseEntity)
        {
            var pInfo = baseEntity.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (pInfo == null || !pInfo.CanWrite) return "";

            if (pInfo.PropertyType == typeof(DateTime) || pInfo.PropertyType == typeof(DateTime?) ||
                pInfo.PropertyType == typeof(Int32) || pInfo.PropertyType == typeof(Int32?)
                || pInfo.PropertyType == typeof(Int16) || pInfo.PropertyType == typeof(Int16?) ||
                pInfo.PropertyType == typeof(Int64) || pInfo.PropertyType == typeof(Int64?)
                || pInfo.PropertyType == typeof(Double) || pInfo.PropertyType == typeof(Double?) ||
                pInfo.PropertyType == typeof(Decimal) || pInfo.PropertyType == typeof(Decimal?) || pInfo.PropertyType == typeof(DateTimeOffset?) || pInfo.PropertyType == typeof(DateTimeOffset))
            {
                if (value == null || value is DBNull || string.IsNullOrEmpty(value.ToString()))
                {
                    value = pInfo.PropertyType == typeof(String) ? "" : "";
                }
                else
                {
                    if (pInfo.PropertyType == typeof(DateTimeOffset) || pInfo.PropertyType == typeof(DateTimeOffset?))
                    {
                        try
                        {
                            DateTimeOffset offset;
                            if (DateTimeOffset.TryParse(Convert.ToString(value), out offset))
                            {
                                value = offset;
                            }
                            else
                                throw new Exception("invalid date time offset");
                        }
                        catch { throw; }
                    }

                    else if (pInfo.PropertyType == typeof(DateTime) || pInfo.PropertyType == typeof(DateTime?))
                    {
                        try
                        {
                            value = Convert.ToDateTime(value, System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
                        }
                        catch { throw; }
                    }
                    else if (pInfo.PropertyType == typeof(Int64) || pInfo.PropertyType == typeof(Int64?))
                    {
                        long l1;
                        if (long.TryParse(value.ToString(), NumberStyles.Any, null, out l1))
                            value = l1;
                        else
                            throw new Exception("Error converting value to long");
                    }
                    else if (pInfo.PropertyType == typeof(Int32) || pInfo.PropertyType == typeof(Int32?))
                    {
                        Int32 t1;
                        if (Int32.TryParse(value.ToString(), NumberStyles.Any, null, out t1))
                            value = t1;
                        else
                            throw new Exception("Error converting value to Int32");
                    }
                    else if (pInfo.PropertyType == typeof(Int16) || pInfo.PropertyType == typeof(Int16?))
                    {
                        Int16 t1;
                        if (Int16.TryParse(value.ToString(), NumberStyles.Any, null, out t1))
                            value = t1;
                        else
                            throw new Exception("Error converting value to Int16");
                    }

                    else if (pInfo.PropertyType == typeof(Decimal) || pInfo.PropertyType == typeof(Decimal?))
                    {
                        Decimal d1;
                        if (Decimal.TryParse(value.ToString(), NumberStyles.Any, null, out d1))
                            value = d1;
                        else
                            throw new Exception("Error converting value to Decimal");
                    }
                    else if (pInfo.PropertyType == typeof(Double) || pInfo.PropertyType == typeof(Double?))
                    {
                        // Use Decimal
                        throw new Exception("Use Decimal");
                    }
                    else
                    {
                        throw new NotImplementedException("else case .. not expected ... developer error");
                    }

                    return value;
                    //pInfo.SetValue(baseEntity, value, null);
                }
            }
            else if (pInfo.PropertyType == typeof(string))
            {
                if (value == null || value is DBNull ||
                    string.IsNullOrEmpty(value.ToString()))
                {
                    value = "";
                }
                else
                    value = value.ToString().Replace("¥", "#");
                pInfo.SetValue(baseEntity, value, null);
            }
            else if (pInfo.PropertyType == typeof(byte[]))
            {
                if (value == null || value is DBNull)
                {
                    // don't set anything 
                }
                else
                    pInfo.SetValue(baseEntity, value, null);
            }
            else if (pInfo.PropertyType == typeof(bool))
            {
                bool result;
                if (!bool.TryParse(value.ToString(), out result))
                {
                    if (value is string)
                    {
                        value = value.ToString() == "1";
                    }
                    else //if (value is DBNull) // when DBtype allows NULL (KISPrescription:PrescPauseYN) //TODO Confirm
                        value = result;
                }
                else
                    value = result;
                //pInfo.SetValue(baseEntity, value, null);
                return value;
            }
            return value;
        }

    }
}
