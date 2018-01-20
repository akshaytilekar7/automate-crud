using Database.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Test.Helper;
using Test.Model;

namespace Test.Service
{
    public class StudentService
    {
        BaseEntity<Student> baseEntity;

        public StudentService()
        {
            baseEntity = new BaseEntity<Student>(ManageConnection.ConnectionString);
        }

        public void Add(Student student)
        {
            baseEntity.InsertEntity(student);
        }

        public void Update(Student student)
        {
            baseEntity.UpdateEntity(student);
        }

        public void Delete(int id)
        {
            baseEntity.DeleteEntity(id);
        }

        public IEnumerable<Student> GetAll()
        {
            return baseEntity.GetAllEntities();
        }

        public Student Get(int id)
        {
            return baseEntity.GetEntity(id);
        }

        public int ExecNonQuery()
        {
            return baseEntity.ExecNonQuery("Update Query");
        }

        public IEnumerable<Student> ExcuteReader()
        {
            return baseEntity.ExcuteReader("SELECT * from TABL OrederBy id asc");
        }

        public object ExecScalar()
        {
            return baseEntity.ExecScalar("SELECT TOP 1 from TABL OrederBy id asc");
        }

        public int ExecNonQuerySP()
        {
            return baseEntity.ExcuteNonQueryWithSP("SP NAME");
        }

        public IEnumerable<Student> GetStudentWithSearchSP(Student studentSearch)
        {
            baseEntity.lstDBParamaters.Add("@Name", studentSearch.Name);
            //baseEntity.lstDBParamaters.Add("@DOB", studentSearch.DOB);
            //baseEntity.lstDBParamaters.Add("@Salary", studentSearch.SalaryNew);
            return baseEntity.ExcuteReaderWithSP("TpGetStudentWithSearch");
        }

        public int InsertStudentWithSP(Student student)
        {
            baseEntity.lstDBParamaters.Add("@Name", student.Name);
            baseEntity.lstDBParamaters.Add("@DOB", student.DOB);
            baseEntity.lstDBParamaters.Add("@Salary", student.SalaryNew);
            baseEntity.lstDBParamaters.Add("@IsActive", student.IsActive);
            return baseEntity.ExcuteNonQueryWithSP("TpInsertStuednt");
        }

        public object ExecScalarWithSP()
        {
            return baseEntity.ExecScalarWithSP("SELECT TOP 1 from TABL OrederBy id asc");
        }

        public List<Dictionary<string, object>> getDyanamicLst(Student student)
        {
            baseEntity.lstDBParamaters.Add("@Name", student.Name);
            return baseEntity.ExcuteReaderWithSPDyanamicList("getDyanamic");
        }

        public Dictionary<string, object> getDyanamicObject(Student student)
        {
            baseEntity.lstDBParamaters.Add("@Name", student.Name);
            return baseEntity.ExcuteReaderWithSPDyanamicObject("getDyanamicObject");
        }
    }
}