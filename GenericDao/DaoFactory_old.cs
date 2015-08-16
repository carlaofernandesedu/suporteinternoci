using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;
namespace GenericDao
{
    public class FactoryOra
    {
        private static OracleCommand _command = new OracleCommand();

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

        public static OracleCommand Command
        {
            get { return _command; }
            set { _command = value; }
        }

        public static OracleConnection Connection
        {
            get { return DaoConnection.Connection; }
            set { DaoConnection.Connection = value; }
        }

        public static bool CloseConnection
        {
            get;
            set;
        }

        public static void BeginTransaction()
        {
            //Command.Transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public static void Commit()
        {
            Command.Transaction.Commit();
        }

        public static void RollBack()
        {
            Command.Transaction.Rollback();
        }

        public static OracleDataReader GetData(string sql)
        {
            Status = StatusEnum.Begining;
            MessageError = "";
            
            try
            {
                OracleDataReader dr;
                
                Command.CommandText = sql;
                Command.Connection = Connection;
                Command.CommandType = System.Data.CommandType.Text;
                
                dr = Command.ExecuteReader();
                
                if (CloseConnection)
                {
                    Command.Connection.Close();
                    Command.Connection.Dispose();
                }
                Status = StatusEnum.Success;

                return dr;
            }
            catch (OracleException ex)
            {
                Status = StatusEnum.Error;
                MessageError = ex.Message;
                throw ex;

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
                Command.CommandType = System.Data.CommandType.Text;

                RowsAffecteds = Command.ExecuteNonQuery();

                if (CloseConnection)
                {
                    Command.Connection.Close();
                    Command.Connection.Dispose();
                }
                Status = StatusEnum.Success;
                return RowsAffecteds;
            }
            catch (OracleException ex)
            {
                Status = StatusEnum.Error;
                MessageError = ex.Message;
                throw ex;
            }
        }

        public static void ExecuteStoreProcedure(string sql)
        {
            Status = StatusEnum.Begining;
            MessageError = "";

            try
            {
                Command.CommandText = sql;
                Command.Connection = Connection;
                Command.CommandType = System.Data.CommandType.StoredProcedure;

                Command.ExecuteNonQuery();

                if (CloseConnection)
                {
                    Command.Connection.Close();
                    Command.Connection.Dispose();
                }

                Status = StatusEnum.Success;

            }
            catch (OracleException ex)
            {
                Status = StatusEnum.Error;
                MessageError = ex.Message;
                throw ex;

            }
        }
        
        public static void CloseGetDataConnection()
        {
            if ((Command.Connection.State != ConnectionState.Closed)
               && (Command.Connection.State != ConnectionState.Broken))
            {
                Command.Connection.Close();
                Command.Connection.Dispose();   
            }
        }

        #region PrepareCommand(NOVO)
        private static void PrepareCommand(DbCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            
            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (DbParameter parm in cmdParms)
                {
                    if (cmd.Parameters.Contains(parm))
                        cmd.Parameters[parm.ParameterName] = parm;
                    else
                        cmd.Parameters.Add(parm);
                }
            }
        }

        private static void PrepareCommand(DbCommand cmd, OracleConnection conn, CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (DbParameter parm in cmdParms)
                {
                    if (cmd.Parameters.Contains(parm))
                        cmd.Parameters[parm.ParameterName] = parm;
                    else
                        cmd.Parameters.Add(parm);
                }
            }
        }
        #endregion

        #region ExecuteNonQuery(NOVO)
        /// <summary>
        /// Método utilizado para executar um comando de escrita no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Inteiro com o resultado do método original.</returns>
        public static int ExecuteNonQuery1(CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            OracleCommand cmd = new OracleCommand();
            using (OracleConnection conn = Connection)
            {
                try
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
        }

        /// <summary>
        /// Método utilizado para executar um comando de escrita no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Inteiro com o resultado do método original.</returns>
        public static int ExecuteNonQuery1(OracleTransaction trans, CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        #endregion

        #region ExecuteNonQueryCmd(NOVO)

        /// <summary>
        /// Método utilizado para executar um comando de escrita no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Comando SQL executado.</returns>
        public static DbCommand ExecuteNonQueryCmd1(CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            DbCommand cmd = new OracleCommand();
            using (OracleConnection conn = new OracleConnection(Connection.ConnectionString))
            {
                try
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    int val = cmd.ExecuteNonQuery();
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
            return cmd;
        }
        /// <summary>
        /// Método utilizado para executar um comando de escrita no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Comando SQL executado.</returns>
        public static DbCommand ExecuteNonQueryCmd1(OracleTransaction trans, CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            DbCommand cmd = new OracleCommand();
                      
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            return cmd;
        }
        
        #endregion

        #region ExecuteReader(NOVO)
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>SqlDataReader de retorno da consulta.</returns>
        public static DbDataReader ExecuteReader1(CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            DbDataReader dr = null;
            OracleConnection conn = Connection;
            try
            {
                DbCommand cmd = new OracleCommand();
                PrepareCommand(cmd, conn,cmdType, cmdText, cmdParms);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                conn.Close();
                conn.Dispose();
                throw;
            }
            return dr;
        }
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>SqlDataReader de retorno da consulta.</returns>
        public static OracleDataReader ExecuteReader1(OracleTransaction trans, CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            OracleDataReader dr = null;
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }
        #endregion

        #region ExecuteReaderDs(NOVO)
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>DataSet de retorno da consulta.</returns>
        public static DataSet ExecuteReaderDs1(CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            DataSet ds = new DataSet();
            
            using (OracleConnection conn = Connection)
            {
                try
                {
                    OracleCommand cmd = new OracleCommand();
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(ds);
                    cmd.Parameters.Clear();
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
            return ds;
        }
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>DataSet de retorno da consulta.</returns>
        public static DataSet ExecuteReaderDs(OracleTransaction trans, CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
        }
        #endregion

        #region ExecuteScalar(NOVO)
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Objeto de retorno da consulta.</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            object obj = new object();
            using (OracleConnection conn = Connection)
            {
                try
                {
                    OracleCommand cmd = new OracleCommand();
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
            return obj;
        }
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Objeto de retorno da consulta.</returns>
        public static object ExecuteScalar(OracleTransaction trans, CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            object obj = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return obj;
        }
        #endregion
    }
}
