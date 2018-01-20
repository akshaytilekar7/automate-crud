using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Database.Core
{
    /// <summary>
    /// resposible for all operation related to database
    /// 1.  database connection
    /// 2.  class property mapping related to database
    /// 2.  primary key constraint
    /// </summary>
    /// <typeparam name="MyType"></typeparam>
    //public class BaseEntity<MyType> where MyType : class, IBaseEntity<MyType>
    public class BaseEntity<MyType> where MyType : class

    {
        private DBOperation dBOperation;
        private string PropertiesNames;
        private string PropertiesValues;
        private string UpdatePropertiesNamesValuesPair;
        private string PrimaryKeyNameInSourceCode = string.Empty;
        private string PrimaryKeyNameInDB = string.Empty;
        private string TableName = string.Empty;

        public Dictionary<string, object> lstDBParamaters { get; set; }

        /// <summary>
        /// initilize database connection
        /// </summary>
        public BaseEntity(string connectionString)
        {
            dBOperation = new DBOperation(connectionString);
        }

        private void Init(MyType myType, bool includePrimary = true)
        {
            SetPropertiesNames(myType, includePrimary);
            SetPropertiesValues(myType, includePrimary);
            SetTableName(myType);
        }

        /// <summary>
        /// check that the class has [primarykey] attribute or not
        /// if yes true
        /// else false
        /// </summary>
        /// <param name="mytype">datatype [ex employee class or student class etc]</param>
        /// <returns></returns>
        private bool ContainsPrimaryKey(MyType myType)
        {
            foreach (PropertyInfo property in myType.GetType().GetProperties())
            {
                if (AttributeHeler.IsPrimaryKeyAttribute(property))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// return primary key name
        /// </summary>
        /// <param name="mytype"></param>
        /// <returns></returns>
        private string SetPimaryKeyName(MyType myType)
        {
            var isContainsPrimarKey = ContainsPrimaryKey(myType);
            if (isContainsPrimarKey)
            {
                //foreach loop can be optimised
                foreach (PropertyInfo property in myType.GetType().GetProperties())
                {
                    if (AttributeHeler.IsPrimaryKeyAttribute(property))
                    {
                        PrimaryKeyNameInSourceCode = property.Name;
                        string primaryKeyName = AttributeHeler.GetColumnName(property);
                        PrimaryKeyNameInDB = primaryKeyName;
                        return primaryKeyName;
                    }
                }
            }
            else
            {
                throw new Exception(ExceptionMessage.PK_NOT_EXIST);
            }
            throw new Exception("Error occured in SetPkName function");
        }

        /// <summary>
        /// set table name from class
        /// </summary>
        /// <param name="mytype"></param>
        private void SetTableName(MyType myType)
        {
            TableNameAttribute tableNameAttribute = myType.GetType().GetCustomAttribute<TableNameAttribute>(false);
            TableName = tableNameAttribute == null ? myType.GetType().Name : tableNameAttribute.TableName;
        }

        /// <summary>
        /// create comma separated propertyname list
        /// if includeprimary is true then primary key propery also consider in list
        /// else primary key property is remove from list
        /// </summary>
        /// <param name="mytype">type of class [ex person class or student etc]</param>
        /// <param name="includeprimary">weather primary key property should consider in list or not if yes : consider , if false : removed from list</param>
        private void SetPropertiesNames(MyType myType, bool includePrimary = true)
        {
            foreach (var property in myType.GetType().GetProperties())
            {
                if (!AttributeHeler.CheckIgnoreColumn(property))
                {
                    if (includePrimary)
                    {
                        string Name = AttributeHeler.GetColumnName(property);
                        PropertiesNames += (Name + ",");
                    }
                    else if (!AttributeHeler.IsPrimaryKeyAttribute(property))
                    {
                        string Name = AttributeHeler.GetColumnName(property);
                        PropertiesNames += (Name + ",");
                    }
                }
            }
            PropertiesNames = PropertiesNames.Remove(PropertiesNames.Length - 1);
        }

        /// <summary>
        /// create comma separated list of property values
        /// if includeprimary is true then primary key value also consider in list
        /// else primary key value is remove from list
        /// </summary>
        /// <param name="mytype">type of class [ex person class or student etc]</param>
        /// <param name="includeprimary">weather primary key property should consider in list or not if yes : consider , if false : removed from list</param>
        private void SetPropertiesValues(MyType myType, bool includePrimary = true)
        {
            string CompleteVal = string.Empty;
            foreach (PropertyInfo property in myType.GetType().GetProperties())
            {
                if (!AttributeHeler.CheckIgnoreColumn(property) && includePrimary == AttributeHeler.IsPrimaryKeyAttribute(property))
                {
                    TypeCode typeCode = Type.GetTypeCode(property.PropertyType);
                    string Name = property.Name;
                    Object value = property.GetValue(myType, null);
                    var val = ClassHelper<MyType>.SetValue(Name, value, myType);
                    CompleteVal += (ClassHelper<MyType>.CreateSqlString(val) + ",");
                }
            }
            CompleteVal = CompleteVal.Remove(CompleteVal.Length - 1);
            PropertiesValues = CompleteVal;
        }

        /// <summary>
        /// use for update command/query
        /// create list of key-value pair like [ propertyname = propertyvalue ,]  and so on
        /// dont consider primary key property in list
        /// </summary>
        /// <param name="mytype">type of class [ex person class or student etc]</param>
        private void SetUpdatePropertise(MyType myType)
        {
            try
            {
                UpdatePropertiesNamesValuesPair = "";
                SetPimaryKeyName(myType);
                foreach (PropertyInfo property in myType.GetType().GetProperties())
                {
                    if (property.Name != PrimaryKeyNameInSourceCode && !AttributeHeler.CheckIgnoreColumn(property))
                    {

                        TypeCode typeCode = Type.GetTypeCode(property.PropertyType);
                        string Name = AttributeHeler.GetColumnName(property);// property.Name;
                        Object value = property.GetValue(myType, null);
                        if (Name != PrimaryKeyNameInDB)
                        {
                            UpdatePropertiesNamesValuesPair += (Name + "='" + value + "',");
                        }
                    }
                }
                UpdatePropertiesNamesValuesPair = UpdatePropertiesNamesValuesPair.Remove(UpdatePropertiesNamesValuesPair.Length - 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// insert entity in datatabse.
        /// inside calls :  init(mytype, false);
        /// </summary>
        /// <param name="mytype"></param>
        /// <returns></returns>
        public int InsertEntity(MyType myType)
        {
            Init(myType, false);
            string sqlInsert = "INSERT INTO " + TableName + " (" + PropertiesNames + ") VALUES (" + PropertiesValues + ")";
            return dBOperation.ExcuteNonQuery(sqlInsert);
        }

        /// <summary>
        /// update query call.
        /// inside calls :  init(mytype);
        /// </summary>
        /// <param name="mytype"></param>
        /// <returns></returns>
        public int UpdateEntity(MyType myType)
        {
            try
            {
                Init(myType);
                SetUpdatePropertise(myType);
                var PKvalue = myType.GetType().GetProperty(PrimaryKeyNameInSourceCode).GetValue(myType, null);
                string sqlInsert = "UPDATE " + TableName + " SET " + UpdatePropertiesNamesValuesPair + " WHERE " + PrimaryKeyNameInDB + " = '" + PKvalue + "'";
                return dBOperation.ExcuteNonQuery(sqlInsert);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///delete record from database based on id
        /// </summary>
        /// <param name="id">id for delete</param>
        /// <returns></returns>
        public int DeleteEntity(int id)
        {
            try
            {
                var type = Activator.CreateInstance<MyType>();
                Init(type);
                SetPimaryKeyName(type);
                string sqlInsert = "DELETE FROM " + TableName + " WHERE " + PrimaryKeyNameInDB + " = '" + id + "'";
                return dBOperation.ExcuteNonQuery(sqlInsert);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// get whole entity based on id. [ex : person class , student class]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MyType GetEntity(int id)
        {
            try
            {
                var myType = Activator.CreateInstance<MyType>();
                SetPimaryKeyName(myType);
                Init(myType);
                string sqlInsert = "SELECT TOP 1 * FROM " + TableName + " WHERE " + PrimaryKeyNameInDB + "=" + id;
                var res = dBOperation.ExcuteReader(sqlInsert).ToCustomObject<MyType>();
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// return list of class [ex : list of person class ,list of student class]
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MyType> GetAllEntities()
        {
            Init(Activator.CreateInstance<MyType>());
            string sqlInsert = "SELECT * FROM " + TableName;
            var res = dBOperation.ExcuteReader(sqlInsert).ToCustomList<MyType>();
            return res;
        }

        /// <summary>
        /// excute sql query into database excute non query
        /// </summary>
        /// <param name="query">query to fire on database</param>
        /// <returns></returns>
        public int ExecNonQuery(string query)
        {
            var result = dBOperation.ExcuteNonQuery(query);
            ClearParameters();
            return result;
        }

        /// <summary>
        /// excute sql query into database excute reader
        /// </summary>
        /// <param name="query">query to fire on database</param>
        /// <returns>return IEnumerable of type</returns>
        public IEnumerable<MyType> ExcuteReader(string query)
        {
            Init(Activator.CreateInstance<MyType>());
            var result = dBOperation.ExcuteReader(query).ToCustomList<MyType>();
            ClearParameters();
            return result;
        }

        /// <summary>
        /// excute sql query into database excute scalar
        /// </summary>
        /// <param name="query">query to fire on database</param>
        /// <returns>return object type</returns>
        public object ExecScalar(string query)
        {
            var result = dBOperation.ExecuteScalar(query);
            ClearParameters();
            return result;
        }

        /// <summary>
        /// excute store procedure 
        /// </summary>
        /// <param name="query">name of stored procedure</param>
        /// <returns>return object type</returns>
        public object ExecScalarWithSP(string storeProcName)
        {
            var result = dBOperation.ExecuteScalarWithSP(storeProcName, lstDBParamaters);
            ClearParameters();
            return result;
        }

        /// <summary>
        /// excute non query store procedure 
        /// </summary>
        /// <param name="query">name of stored procedure</param>
        /// <returns>return int type</returns>
        public int ExcuteNonQueryWithSP(string storeProcName)
        {
            var result = dBOperation.ExcuteNonQueryWithSP(storeProcName, lstDBParamaters);
            ClearParameters();
            return result;
        }

        /// <summary>
        /// excute store procedure excute reader
        /// </summary>
        /// <param name="query">name of stored procedure</param>
        /// <returns>return IEnumerable type</returns>
        public IEnumerable<MyType> ExcuteReaderWithSP(string storeProcName)
        {
            var result = dBOperation.ExcuteReaderWithSP(storeProcName, lstDBParamaters).ToCustomList<MyType>();
            ClearParameters();
            return result;
        }

        public List<Dictionary<string, object>> ExcuteReaderWithSPDyanamicList(string storeProcName)
        {
            dynamic MyDynamic = new System.Dynamic.ExpandoObject();
            var lstdicts = dBOperation.ExcuteReaderWithSP(storeProcName, lstDBParamaters).ToCustomListOfDictinory<System.Dynamic.ExpandoObject>();
            ClearParameters();
            return lstdicts;
        }

        public Dictionary<string, object> ExcuteReaderWithSPDyanamicObject(string storeProcName)
        {
            dynamic MyDynamic = new System.Dynamic.ExpandoObject();
            var lstdicts = dBOperation.ExcuteReaderWithSP(storeProcName, lstDBParamaters).ToCustomObjectOfDictinory<System.Dynamic.ExpandoObject>();
            ClearParameters();
            return lstdicts;

        }

        public void ClearParameters()
        {
            lstDBParamaters = new Dictionary<string, object>();
        }

    }
}
