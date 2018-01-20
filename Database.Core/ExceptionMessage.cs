using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Core
{
    public static class ExceptionMessage
    {
        public static string PK_NOT_EXIST = "Trying to Update/Delete/Get without PrimaryKey or [Primarykey] attribute in class is missing";
    }
}
