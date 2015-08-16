using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data.OracleClient;
using System.Web;
using System.Configuration;
using Oracle.DataAccess.Client;
namespace GenericDao
{
    public class DaoConnection
    {
        private static OracleConnection _connection;

        public DaoConnection() { }

        internal static OracleConnection Connection
        {
            get { return getConnection(); }
            set { _connection = value; }
        }

        internal static OracleConnection getConnection()
        {
            try
            {
                if (!VerifyStatusConnection(_connection))
                {
                    _connection = new OracleConnection();
                    _connection.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    _connection.Open();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return _connection;
        }

        internal static bool VerifyStatusConnection(OracleConnection connection)
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
