using Database.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Model
{
    [TableName("Student")]
    public class Student
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }

        [ColumnName("Salary")]
        public decimal SalaryNew { get; set; }

        public DateTime DOB { get; set; }

        public bool IsActive { get; set; }

        [IgnoreColumn]
        public string ExtraProperty { get; set; }

        public override string ToString()
        {
            return this.Id + "   \n   " + this.Name + "   \n    " + this.SalaryNew + "   \n    " + this.IsActive;
        }
    }
}