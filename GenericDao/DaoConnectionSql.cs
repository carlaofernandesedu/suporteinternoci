using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;
using System.Data;

namespace GenericDao
{
    public class DaoConnectionSql
    {
        private static SqlConnection _connection;

        public DaoConnectionSql() { }

        internal static SqlConnection Connection
        {
            get { return getConnection(); }
            set { _connection = value; }
        }

        internal static SqlConnection getConnection()
        {
            try
            {
                _connection = new SqlConnection("Password=prodesp01;Persist Security Info=True;User ID=sa;Initial Catalog=DB_Bonus;Data Source=10.200.9.74;");
                _connection.Open();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return _connection;
        }

        internal static bool VerifyStatusConnection(SqlConnection connection)
        {
            if ((connection == null) || (connection.State == System.Data.ConnectionState.Closed))
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
