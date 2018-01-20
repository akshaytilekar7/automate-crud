using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Helper
{
    public static class ManageConnection
    {

        private static string _GetConnectionString;

        /// <summary>
        /// return connection string from web/app config
        /// </summary>
        public static string ConnectionString
        {
            get { return _GetConnectionString; }
        }


        /// <summary>
        /// Get Connection string from web/app.config
        /// </summary>
        static ManageConnection()
        {
            _GetConnectionString = @"Data Source=DESKTOP-GT72SLT\SQLEXPRESS;Initial Catalog =Test;integrated security = true"; ;
        }
    }

}
