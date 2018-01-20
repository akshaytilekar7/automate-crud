using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Database.Core
{
    public class DBOperation
    {
        string conStr;
        SqlCommand sqlCmd;
        SqlConnection sqlConn;
        public string CON_STR = "DefaultConnection";
        public Dictionary<string, object> _lstDBParamaters { get; set; }

        public DBOperation(string conString)
        {
            conStr = conString;
            sqlConn = new SqlConnection(conStr);
        }

        public void ClearParameters()
        {
            _lstDBParamaters = new Dictionary<string, object>();
        }

        public object ExecuteScalar(string sql)
        {
            try
            {
                sqlConn.Open();
                sqlCmd = new SqlCommand(sql, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                var obj = sqlCmd.ExecuteScalar();
                sqlCmd.Dispose();
                sqlConn.Close();
                return obj;
            }
            catch (Exception ex)
            {
                sqlCmd.Dispose();
                sqlConn.Close();
                throw ex;
            }
        }

        public int ExcuteNonQuery(string sql)
        {
            try
            {
                sqlConn.Open();
                sqlCmd = new SqlCommand(sql, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                var res = sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConn.Close();
                return res;
            }
            catch (Exception ex)
            {
                sqlCmd.Dispose();
                sqlConn.Close();
                throw ex;
            }
        }

        public DataTable ExcuteReader(string sql)
        {
            try
            {
                sqlConn.Open();
                sqlCmd = new SqlCommand(sql, sqlConn);
                sqlCmd.CommandType = CommandType.Text;
                var sdr = sqlCmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sdr);
                sqlCmd.Dispose();
                sqlConn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                sqlCmd.Dispose();
                sqlConn.Close();
                throw ex;
            }
        }

        public int ExcuteNonQueryWithSP(string StoredprocName, Dictionary<string, object> lstDBParamaters)
        {
            try
            {
                sqlConn.Open();
                sqlCmd = new SqlCommand(StoredprocName, sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                GenerateSqlParameters(lstDBParamaters);
                var res = sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConn.Close();
                return res;
            }
            catch (Exception ex)
            {
                ClearParameters();
                sqlCmd.Dispose();
                sqlConn.Close();
                throw ex;
            }
        }

        public DataTable ExcuteReaderWithSP(string StoredprocName, Dictionary<string, object> lstDBParamaters)
        {
            try
            {
                sqlConn.Open();
                sqlCmd = new SqlCommand(StoredprocName, sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                GenerateSqlParameters(lstDBParamaters);
                var sdr = sqlCmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sdr);
                sqlCmd.Dispose();
                sqlConn.Close();
                ClearParameters();
                return dt;
            }
            catch (Exception ex)
            {
                ClearParameters();
                sqlCmd.Dispose();
                sqlConn.Close();
                throw ex;
            }
        }

        public object ExecuteScalarWithSP(string StoredprocName, Dictionary<string, object> lstDBParamaters)
        {
            try
            {
                sqlConn.Open();
                sqlCmd = new SqlCommand(StoredprocName, sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                GenerateSqlParameters(lstDBParamaters);
                var obj = sqlCmd.ExecuteScalar();
                sqlCmd.Dispose();
                ClearParameters();
                sqlConn.Close();
                return obj;
            }
            catch (Exception ex)
            {
                ClearParameters();
                sqlCmd.Dispose();
                sqlConn.Close();
                throw ex;
            }
        }

        private void GenerateSqlParameters(Dictionary<string, object> lstDbParameter)
        {
            if (lstDbParameter == null)
                return;

            foreach (var item in lstDbParameter)
            {
                sqlCmd.Parameters.AddWithValue(item.Key, item.Value);
            }
        }
    }
}


