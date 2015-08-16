using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.OracleClient;
using System.Data.ProviderBase;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace GenericDao
{
    [Serializable]
    public class DAOFactory
    {
        #region //** Privates *****************************************//
        private DbProviderFactory objProvider = null;
        private string connectionString = null;
        private IDbConnection objConn = null;
        private IDbTransaction objTransaction = null;
        #endregion

        #region //** Publics ******************************************//
        /// <summary>Instancia a DbFactory "setando a string de conexao"
        /// </summary>
        public DAOFactory(string connectionStringValue, string provider)
        {
            AppSettingsReader apps = new AppSettingsReader();
            objProvider = DbProviderFactories.GetFactory(provider);
            connectionString = connectionStringValue;
            objConn = objProvider.CreateConnection();

        }
        public DAOFactory()
        {
            AppSettingsReader apps = new AppSettingsReader();


            objProvider = DbProviderFactories.GetFactory(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionStringOracle"].ProviderName);

            connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionStringOracle"].ConnectionString;

            objConn = objProvider.CreateConnection();
        }
        #endregion


        #region //** Método para retornar a conexão. ******************//
        /// <summary>
        /// Método para retornar a conexão.
        /// </summary>
        public IDbConnection Connection
        {
            get { return objConn; }
        }
        #endregion

        #region //** Método que executa o "NonQuery" ******************//
        public bool ExecuteNonQuery(IDbCommand cmd)
        {
            IDbCommand objCommand = cmd;
            objCommand.Connection = objConn;
            if (objTransaction != null)
                objCommand.Transaction = objTransaction;
            return Convert.ToBoolean(objCommand.ExecuteNonQuery());
        }
        #endregion

        #region //** Métodos que executam o "ExecuteScalar" ***********//
        /// <summary>Executa o comando scalar.</summary>
        /// <param name="cmd">IdbCommand</param>
        /// <returns>double</returns>
        public int ExecuteScalar(IDbCommand cmd)
        {
            IDbCommand objCommand = cmd;
            objCommand.Connection = objConn;
            if (objTransaction != null)
                objCommand.Transaction = objTransaction;
            return Convert.ToInt32(objCommand.ExecuteScalar());
        }

        public object ExecuteScalar(IDbCommand cmd,bool returnObject)
        {
            var objCommand = cmd;
            objCommand.Connection = objConn;
            if (objTransaction != null)
                objCommand.Transaction = objTransaction;
            return objCommand.ExecuteScalar();
        }



        #endregion

        #region //** Método que executa o "ExecuteReader" *************//
        /// <summary>Executa o reader.</summary>
        /// <param name="cmd">IDbCommand.</param>
        /// <returns>IDbCommand</returns>
        public IDataReader ExecuteReader(IDbCommand cmd)
        {
            IDbCommand objCommand = cmd;
            objCommand.Connection = objConn;
            if (objTransaction != null)
                objCommand.Transaction = objTransaction;
            return objCommand.ExecuteReader();
        }
        #endregion

        #region //** Inicia a conexão *********************************//
        /// <summary>
        /// Inicia a conexão.
        /// </summary>
        /// <param name="requiredTransaction">if set to <c>true</c> [required transaction].</param>
        /// <returns></returns>
        public object startConnection()
        {

            if (string.IsNullOrEmpty(objConn.ConnectionString))
                objConn.ConnectionString = connectionString;
            if (objConn.State == ConnectionState.Closed)
                objConn.Open();

            return objConn;

        }
        #endregion

        #region //** Método para determinar se terá ou não transação **//
        /// <summary>
        /// Método para determinar se terá ou não transação. 
        /// </summary>
        public void hasTransaction()
        {
            beginTransaction();
        }
        #endregion

        #region //** Método para desconectar. *************************//
        /// <summary>
        /// Método para desconectar.
        /// </summary>
        public void closeConnection()
        {
            if (objConn.State == ConnectionState.Open)
            {
                objConn.Close();
            }
        }
        #endregion

        #region //** Método para iniciar transação ********************//
        /// <summary>
        /// Método para iniciar transação.
        /// </summary>
        /// <returns>IDbTransaction</returns>
        private IDbTransaction beginTransaction()
        {
            objTransaction = objConn.BeginTransaction();
            return objTransaction;
        }
        #endregion

        #region //** Método para encerrar transação. ******************//
        /// <summary>
        /// Método para encerrar transação. Method to commits the transaction.
        /// </summary>
        public void commitTransaction()
        {
            if (objTransaction != null)
                objTransaction.Commit();
        }
        #endregion

        #region //** Método para desfazer uma transação. **************//
        /// <summary>
        /// Método para desfazer uma transação.
        /// </summary>
        public void rollbackTransaction()
        {
            if (objTransaction.Connection != null)
                objTransaction.Rollback();
        }
        #endregion

        #region //** Método para criar comando. ***********************//
        /// <summary>
        /// Método para criar comando. 
        /// </summary>
        /// <returns>DbProviderFactory</returns>
        public IDbCommand createCommand()
        {
            IDbCommand cmd = objProvider.CreateCommand();
            ((Oracle.DataAccess.Client.OracleCommand)cmd).BindByName = true;
            return cmd;

            //return objProvider.CreateCommand();
        }
        #endregion

        #region //** Métodos para criar paramêtro. ********************//
        /// <summary>Método para criar paramêtro.</summary>
        /// <returns>DbProviderFactory</returns>
        public IDbDataParameter createParameter()
        {
            return objProvider.CreateParameter();
        }
        /// <summary>
        /// Método para criar parametro. 
        /// </summary>
        /// <param name="name">Nome do parametro(string).</param>
        /// <param name="type">Tipo de dado(DbType).</param>
        /// <param name="value">Valor do parametro(object).</param>
        /// <returns>IDataParameter</returns>
        public IDbDataParameter createParameter(string name, DbType type, object value)
        {
            return createParameter(name, type, value, ParameterDirection.Input);
        }
        /// <summary>
        /// Método para criar parametro. 
        /// </summary>
        /// <param name="name">Nome do parametro(string).</param>
        /// <param name="type">Tipo de dado(DbType).</param>
        /// <param name="direction">ParameterDirection(Ex: Input,InputOutput,Output ou ReturnValue).</param>
        /// <returns>IDataParameter.</returns>
        public IDbDataParameter createParameter(string name, DbType type, ParameterDirection direction)
        {
            return createParameter(name, type, null, direction);
        }
        /// <summary>Método para criar parametro. </summary>
        /// <param name="name">Nome do parametro(string).</param>
        /// <param name="type">Tipo de dado(DbType).</param>
        /// <param name="value">Object.</param>
        /// <param name="direction">ParameterDirection(Ex: Input,InputOutput,Output ou ReturnValue).</param>
        /// <returns>IDataParameter.</returns>
        public IDbDataParameter createParameter(string name, DbType type, object value, ParameterDirection direction)
        {
            IDbDataParameter objParamenter = objProvider.CreateParameter();

            objParamenter.ParameterName = name;
            objParamenter.DbType = type;
            objParamenter.Value = value;
            objParamenter.Direction = direction;

            return objParamenter;
        }

        #endregion

        #region //** Método para destruir o objeto da memória. ********//
        /// <summary>Método para destruir o objeto da memória.</summary>
        public void Dispose()
        {
            if (objConn.State == ConnectionState.Open)
                objConn.Close();
            objConn.Dispose();
        }
        #endregion

        public OracleDataReader odr;
        //public StringBuilder sSQL = new StringBuilder();
    }
}
