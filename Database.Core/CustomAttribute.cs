using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Core
{

    /// <summary>
    /// use to set primary key of tabel
    /// </summary>
    public class PrimaryKeyAttribute : Attribute
    {
       
    }

    /// <summary>
    /// use to set table name 
    /// </summary>
    public class TableNameAttribute : Attribute
    {
        public string TableName { get; set; }

        public TableNameAttribute(string Name)
        {
            this.TableName = Name;
        }
    }

    /// <summary>
    /// used to set extra property in class/table
    /// </summary>
    public class IgnoreColumnAttribute : Attribute
    {

    }

    /// <summary>
    /// used to set table coloumn name on property of class
    /// </summary>
    public class ColumnNameAttribute : Attribute
    {
        private string ColumnName { get; set; }

        public ColumnNameAttribute(string Name)
        {
            this.ColumnName = Name;
        }

    }

}
