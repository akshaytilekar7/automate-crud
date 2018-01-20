# automate-crud

Custom .NET framework 

I have created custom framework similar to entity framework

This framework will automate code for insert/update/delete/read and various other operations (like Excute Stroed procedure/Query etc).
- all core logic is in Database.Core project. 
- you just have to create enitity/model class 
- create object of my BaseEntity and pass your entity/model 
- that BaseEntity will provide various method for different database operations
- Project has one Console application (name as "Test") which will show how to use my DataBase.Core 

# How To Use
- just change connection string in ManageConnection.cs class
- and create Model/Entity class (Sample class is in console application (Named as "Test") :  Model/Student.cs)
    - Provide attributes on property that used in that class 
        - [TableName("UrDataBaseTableName")]
        - [PrimaryKey]
        - [ColumnName("ColumnName")]
        - [IgnoreColumn]
 
 # Use BaseEntity as below :
 -   BaseEntity<Student> baseEntity = new BaseEntity<Student>(ManageConnection.ConnectionString);
 
 # About BaseEntity :
 
 -  BaseEntity has following Methoods 
     -  public BaseEntity(string connectionString)
     -  public int InsertEntity(Entity entity)
     -  public int UpdateEntity(Entity entity)
     -  public int DeleteEntity(int id)
     -  public MyType GetEntity(int id)
     -  public IEnumerable<MyType> GetAllEntities()
     -  public int ExecNonQuery(string query)
     -  public IEnumerable<MyType> ExcuteReader(string query)
     -  public object ExecScalar(string query)
     -  public object ExecScalarWithSP(string storeProcName)
     -  public int ExcuteNonQueryWithSP(string storeProcName)
     -  public IEnumerable<MyType> ExcuteReaderWithSP(string storeProcName)
     -  public List<Dictionary<string, object>> ExcuteReaderWithSPDyanamicList(string storeProcName)
     -  public Dictionary<string, object> ExcuteReaderWithSPDyanamicObject(string storeProcName)
