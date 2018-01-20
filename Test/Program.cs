using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Model;
using Test.Service;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            StudentService studentService = new StudentService();

            #region Add 
            Student s = new Student()
            {
                DOB = new DateTime(1995, 01, 10),
                IsActive = true,
                Name = "XXX YYY ZZZZ",
                SalaryNew = Convert.ToDecimal("7800")
            };

            studentService.Add(s);
            #endregion

            foreach (var item in studentService.GetAll())
            {
                Console.WriteLine(item.ToString());
            }

            Console.WriteLine("Update Student");

            #region Get and Update 
            var res = studentService.Get(1);
            res.IsActive = false;
            res.Name = "Akshay Tileakar";
            res.DOB = new DateTime(1992, 08, 05);
            studentService.Update(res);
            #endregion

            #region delete
            studentService.Delete(2);
            #endregion

            Console.WriteLine("List Is :-");
            foreach (var item in studentService.GetAll())
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("List end");


            #region Excute SP Example 1 
            Student Studentsearch = new Student();
            Studentsearch.Name = Console.ReadLine();
            var SearchLst = studentService.GetStudentWithSearchSP(Studentsearch);
            Console.WriteLine("Search Result : ");
            foreach (var item in SearchLst)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("Search Result ended : ");
            #endregion

            #region Excute SP Example 2
            Student snew = new Student();
            snew.Name = "Aksya Tileakr";
            snew.IsActive = true;
            snew.SalaryNew = Convert.ToDecimal(2000.465f);
            snew.DOB = DateTime.Now;
            studentService.InsertStudentWithSP(snew);
            #endregion

            #region GetAll List
            Console.WriteLine("List Is :-");
            foreach (var item in studentService.GetAll())
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("list end");
            #endregion


            List<Dictionary<string, object>> res123 = studentService.getDyanamicLst(new Student());
            Dictionary<string, object> res1233 = studentService.getDyanamicObject(new Student());

            Console.ReadLine();
        }
    }
}
