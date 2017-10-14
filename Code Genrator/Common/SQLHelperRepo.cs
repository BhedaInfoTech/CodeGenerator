using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient;

namespace Code_Genrator.Common
{
    public class SQLHelperRepo
    {
        private string _sqlCon = string.Empty;

        public SQLHelperRepo()
        {
            _sqlCon = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        }

        public DataSet ExecuteDataSet(List<MySql.Data.MySqlClient.MySqlParameter> sqlParams, string sqlQuery, CommandType cmdType)
        {
            DataSet ds = new DataSet();
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(_sqlCon))
                {
                    MySql.Data.MySqlClient.MySqlCommand sqlCmd = new MySql.Data.MySqlClient.MySqlCommand(sqlQuery, con);
                    sqlCmd.CommandType = cmdType;
                    sqlCmd.CommandTimeout = 300;
                    if (sqlParams != null)
                    {
                        foreach (MySql.Data.MySqlClient.MySqlParameter sqlPrm in sqlParams)
                        {
                            if (sqlPrm.Value == null)
                                sqlPrm.Value = DBNull.Value;
                        }
                        sqlCmd.Parameters.AddRange(sqlParams.ToArray());
                    }

                    MySql.Data.MySqlClient.MySqlDataAdapter sqlDA = new MySql.Data.MySqlClient.MySqlDataAdapter(sqlCmd);
                    sqlDA.Fill(ds);
                }
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        public DataTable ExecuteDataTable(List<MySql.Data.MySqlClient.MySqlParameter> sqlParams, string sqlQuery, CommandType cmdType)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(_sqlCon))
                {
                    MySql.Data.MySqlClient.MySqlCommand sqlCmd = new MySql.Data.MySqlClient.MySqlCommand(sqlQuery, con);
                    sqlCmd.CommandType = cmdType;
                    sqlCmd.CommandTimeout = 300;
                    if (sqlParams != null)
                    {
                        foreach (MySql.Data.MySqlClient.MySqlParameter sqlPrm in sqlParams)
                        {
                            if (sqlPrm.Value == null)
                                //sqlPrm.Value = DBNull.Value;
                                sqlPrm.Value = SqlString.Null;
                        }
                        sqlCmd.Parameters.AddRange(sqlParams.ToArray());
                    }

                    MySql.Data.MySqlClient.MySqlDataAdapter sqlDA = new MySql.Data.MySqlClient.MySqlDataAdapter(sqlCmd);
                    sqlDA.Fill(dt);
                }
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public MySql.Data.MySqlClient.MySqlDataReader ExecuteDataReader(List<MySql.Data.MySqlClient.MySqlParameter> sqlParams, string sqlQuery, CommandType cmdType)
        {
            MySql.Data.MySqlClient.MySqlDataReader dr;

            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(_sqlCon))
                {
                    //con.Open();
                    MySql.Data.MySqlClient.MySqlCommand sqlCmd = new MySql.Data.MySqlClient.MySqlCommand(sqlQuery, con);
                    sqlCmd.CommandType = cmdType;
                    sqlCmd.CommandTimeout = 300;

                    if (sqlParams != null)
                    {
                        foreach (MySql.Data.MySqlClient.MySqlParameter sqlPrm in sqlParams)
                        {
                            if (sqlPrm.Value == null)
                                sqlPrm.Value = DBNull.Value;
                        }
                        sqlCmd.Parameters.AddRange(sqlParams.ToArray());
                    }

                    dr = sqlCmd.ExecuteReader();
                    //con.Close();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dr;
        }

        public void ExecuteNonQuery(List<MySql.Data.MySqlClient.MySqlParameter> sqlParams, string sqlQuery, CommandType cmdType)
        {
            MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection();
            try
            {
                using (con = new MySql.Data.MySqlClient.MySqlConnection(_sqlCon))
                {
                    con.Open();
                    MySql.Data.MySqlClient.MySqlCommand sqlCmd = new MySql.Data.MySqlClient.MySqlCommand(sqlQuery, con);
                    sqlCmd.CommandType = cmdType;

                    if (sqlParams != null)
                    {
                        foreach (MySql.Data.MySqlClient.MySqlParameter sqlPrm in sqlParams)
                        {
                            if (sqlPrm.Value == null)
                                sqlPrm.Value = DBNull.Value;
                        }
                        sqlCmd.Parameters.AddRange(sqlParams.ToArray());
                    }

                    sqlCmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException sqlEx)
            {
                if (con != null)
                    con.Close();

                throw sqlEx;
            }
            catch (Exception ex)
            {
                if (con != null)
                    con.Close();

                throw ex;
            }
        }

        public object ExecuteScalerObj(List<MySql.Data.MySqlClient.MySqlParameter> sqlParams, string sqlQuery, CommandType cmdType)
        {
            MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection();

            object result = 0;
            try
            {
                using (con = new MySql.Data.MySqlClient.MySqlConnection(_sqlCon))
                {
                    con.Open();
                    MySql.Data.MySqlClient.MySqlCommand sqlCmd = new MySql.Data.MySqlClient.MySqlCommand(sqlQuery, con);
                    sqlCmd.CommandType = cmdType;

                    if (sqlParams != null)
                    {
                        foreach (MySql.Data.MySqlClient.MySqlParameter sqlPrm in sqlParams)
                        {
                            if (sqlPrm.Value == null)
                                sqlPrm.Value = DBNull.Value;
                        }
                        sqlCmd.Parameters.AddRange(sqlParams.ToArray());
                    }

                    result = sqlCmd.ExecuteScalar();
                    con.Close();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException sqlEx)
            {
                if (con != null)
                    con.Close();

                throw sqlEx;
            }
            catch (Exception ex)
            {
                if (con != null)
                    con.Close();

                throw ex;
            }
            return result;
        }

        public void ExecuteNonQueryWithTransaction(List<MySql.Data.MySqlClient.MySqlParameter> sqlParams, string sqlQuery, CommandType cmdType, MySqlTransaction transaction, MySql.Data.MySqlClient.MySqlConnection con)
        {
            //MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection();
            try
            {
                MySql.Data.MySqlClient.MySqlCommand sqlCmd = new MySql.Data.MySqlClient.MySqlCommand(sqlQuery, con, transaction);
                sqlCmd.CommandType = cmdType;

                if (sqlParams != null)
                {
                    foreach (MySql.Data.MySqlClient.MySqlParameter sqlPrm in sqlParams)
                    {
                        if (sqlPrm.Value == null)
                            sqlPrm.Value = DBNull.Value;
                    }
                    sqlCmd.Parameters.AddRange(sqlParams.ToArray());
                }

                sqlCmd.ExecuteNonQuery();
            }
            catch (MySql.Data.MySqlClient.MySqlException sqlEx)
            {
                if (con != null)
                    //con.Close();

                    throw sqlEx;
            }
            catch (Exception ex)
            {
                if (con != null)
                    //con.Close();

                    throw ex;
            }
        }

        public object ExecuteScalerObjWithTransaction(List<MySql.Data.MySqlClient.MySqlParameter> sqlParams, string sqlQuery, CommandType cmdType, MySqlTransaction transaction, MySql.Data.MySqlClient.MySqlConnection con)
        {
            //MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection();

            object result = 0;
            try
            {
                MySql.Data.MySqlClient.MySqlCommand sqlCmd = new MySql.Data.MySqlClient.MySqlCommand(sqlQuery, con, transaction);
                sqlCmd.CommandType = cmdType;

                if (sqlParams != null)
                {
                    foreach (MySql.Data.MySqlClient.MySqlParameter sqlPrm in sqlParams)
                    {
                        if (sqlPrm.Value == null)
                            sqlPrm.Value = DBNull.Value;
                    }
                    sqlCmd.Parameters.AddRange(sqlParams.ToArray());
                }

                result = sqlCmd.ExecuteScalar();
            }
            catch (MySql.Data.MySqlClient.MySqlException sqlEx)
            {
                if (con != null)
                    //con.Close();

                    throw sqlEx;
            }
            catch (Exception ex)
            {
                if (con != null)
                    //con.Close();

                    throw ex;
            }
            return result;
        }
        
    }
}