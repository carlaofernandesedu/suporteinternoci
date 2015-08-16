using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace GenericDao
{
    public class DaoFactorySql
    {
        private static SqlCommand _command = new SqlCommand();

        public enum StatusEnum
        {
            Error = -1,
            Begining = 0,
            Success = 1,
        }

        public static string MessageError
        {
            get;
            set;
        }

        public static StatusEnum Status
        {
            get;
            set;
        }

        public static SqlCommand Command
        {
            get { return _command; }
            set { _command = value; }
        }

        public static SqlConnection Connection
        {
            get { return DaoConnectionSql.getConnection(); }
            set { DaoConnectionSql.Connection = value; }
        }

        public static bool CloseConnection
        {
            get;
            set;
        }

        public static void BeginTransaction()
        {
            Command.Transaction = Connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        public static void Commit()
        {
            Command.Transaction.Commit();
        }

        public static void RollBack()
        {
            Command.Transaction.Rollback();
        }

        public static SqlDataReader GetData(string sql)
        {
            Status = StatusEnum.Begining;
            MessageError = "";
            
            try
            {
                SqlDataReader dr;
                Command.CommandText = sql;
                Command.Connection = DaoConnectionSql.getConnection();
                //Command.Connection = Connection;

                dr = Command.ExecuteReader();
                if (CloseConnection)
                {

                    Command.Connection.Close();
                    Command.Connection.Dispose();
                }
                Status = StatusEnum.Success;
                
                return dr;
            }
            catch (Exception ex)
            {
                Status = StatusEnum.Error;
                MessageError = ex.Message.ToString();
                throw new Exception(ex.Message);
                
            }
        }

        public static int Execute(string sql)
        {
            Status = StatusEnum.Begining;
            MessageError = "";

            try
            {
                int RowsAffecteds;

                Command.CommandText = sql;
                Command.Connection = Connection;

                RowsAffecteds = Command.ExecuteNonQuery();

                if (CloseConnection)
                {
                    Command.Connection.Close();
                    Command.Connection.Dispose();
                }
                Status = StatusEnum.Success;
                return RowsAffecteds;
            }
            catch (Exception ex)
            {
                Status = StatusEnum.Error;
                MessageError = ex.Message.ToString();
                throw new Exception(ex.Message);
            }
        }
    }
}
