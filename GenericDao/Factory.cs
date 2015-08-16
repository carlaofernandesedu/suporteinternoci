using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Oracle.DataAccess.Client;

namespace GenericDao
{
    public class Factory
    {
        public enum TypeDB { ORACLE, SQL };

        static DbConnection conn;
        
        static DbProviderFactory factory;
        
        TypeDB _tp;
        
        string Provider = string.Empty; string ConnetionString = string.Empty;

        public Factory(TypeDB tp){this._tp = tp;}

        public DbDataReader GetData(string Sql)
        {
            try
            {
                GetObjConnection();
                
                DbCommand cmd = factory.CreateCommand();
                cmd.CommandText = Sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                DbDataReader dr;

                dr = cmd.ExecuteReader();

                return dr;

            }
            catch (DbException ex)
            {
                
                throw ex;
            }
        }
        
        public int ExecuteNonQuery(string Sql)
        {
            try
            {
                int iRet = 0;

                GetObjConnection();

                DbCommand cmd = factory.CreateCommand();
                cmd.CommandText = Sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                iRet = cmd.ExecuteNonQuery();

                return iRet;
            }
            catch (DbException ex)
            {
                throw ex;
            }
        }
        
        public DbCommand GetObjCommandWithTransaction()
        {
            try
            {
                if (conn != null)
                {
                    conn.Dispose();
                }

                GetObjConnetionTransaction();

                TestConnection();
                                
                DbCommand cmd = factory.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                return cmd;
            }
            catch (DbException ex)
            {
                conn.Dispose();
                throw ex;
            }
        }

        public DbTransaction GetObjTransaction()
        {
            DbTransaction tr = conn.BeginTransaction();

            return tr;
        }

        public DbTransaction GetObjTransaction(IsolationLevel IsolationLevel)
        {
            DbTransaction tr = conn.BeginTransaction(IsolationLevel);

            return tr;
        }


        public DbCommand ExecuteProcedure()
        {
            try
            {
                GetObjConnection();

                DbCommand cmd = factory.CreateCommand();
                cmd.Connection = conn;

                return cmd;
            }
            catch (DbException ex)
            {
                throw ex;
            }
        }

        protected static void TestConnection()
        {
            if (conn.State == System.Data.ConnectionState.Open ||
                    conn.State == System.Data.ConnectionState.Connecting ||
                    conn.State == System.Data.ConnectionState.Broken)
                conn.Close();

            conn.Open();
        }

        internal void GetObjConnection()
        {
            PopulateProviderConnectionString();

            if ((conn == null) || (conn.State == System.Data.ConnectionState.Closed))
            {
                factory = DbProviderFactories.GetFactory(Provider);
                conn = factory.CreateConnection();
                conn.ConnectionString = ConnetionString;
                conn.Open();
            }
            
        }

        internal void GetObjConnetionTransaction()
        {
            PopulateProviderConnectionString();

            factory = DbProviderFactories.GetFactory(Provider);
            conn = factory.CreateConnection();
            conn.ConnectionString = ConnetionString;
         }

        public DataSet GetObjDataSet(string Sql)
        {
            try
            {
                GetObjConnection();
                DataSet ds = new DataSet();

                DbDataAdapter da = factory.CreateDataAdapter();
                DbCommand cmd = factory.CreateCommand();

                cmd.CommandText = Sql;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                using (da.SelectCommand = cmd)
                {
                    da.Fill(ds);
                }

                return ds;

            }
            catch (DbException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        protected void PopulateProviderConnectionString()
        {
            if (_tp == TypeDB.ORACLE)
            {
                Provider = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionStringOracle"].ProviderName;
                ConnetionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionStringOracle"].ConnectionString;
            }
            else if (_tp == TypeDB.SQL)
            {
                Provider = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionStringSQL"].ProviderName;
                ConnetionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionStringSQL"].ConnectionString;
            }
        }
     }
}
