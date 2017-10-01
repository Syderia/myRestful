using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace myRestful.Utility
{
    public class SqlUtility
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void SetParameter(SqlCommand scmd, List<string> parameter, List<Object> value, List<SqlDbType> type)
        {
            SqlParameter[] parameters = new SqlParameter[parameter.ToArray().Length];
            for (int i = 0; i < parameter.ToArray().Length; i++)
            {
                parameters[i] = new SqlParameter(parameter[i], type[i]);
                parameters[i].Value = value[i];
            }
            foreach (SqlParameter param in parameters)
            {
                scmd.Parameters.Add(param);
            }
        }

        public static SqlCommand BuildConnectedSqlCommand()
        {
            try
            {
                SqlCommand scmd = null;
                //get the conneting string
                String connStr = ConfigurationManager.ConnectionStrings["RestfulDB"].ConnectionString;
                //create a connected object with conneting string
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                //start transaction
                SqlTransaction sTrans = conn.BeginTransaction();
                //create sqlCommand object with  connetion
                scmd = conn.CreateCommand();
                scmd.Connection = conn;
                scmd.Transaction = sTrans;
                return scmd;
            }
            catch (SqlException ex)
            {
                log.Debug(ex.ToString());
                throw ex;
            }
        }
    }
}