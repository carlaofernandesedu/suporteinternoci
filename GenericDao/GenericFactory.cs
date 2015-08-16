using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using System.Data.Common;
using Oracle.DataAccess.Client;

namespace GenericDao
{
    public enum TypeCommand { ExecuteNonQuery, ExecuteReader, ExecuteScalar, ExecuteDataTable }
    
    public static class GenericFactory
    {
        public static DbCommand createCommand(String cmmdText, CommandType cmmdType, List<DbParameter> listParameter, ConnectionDB.TypeDB TpDB)
        {
            try
            {
                ConnectionDB.TipoDB = TpDB;
                ConnectionDB.Build();
                DbProviderFactory factory = DbProviderFactories.GetFactory(ConnectionDB.ProviderName);
                DbConnection conn = factory.CreateConnection();
                conn.ConnectionString = ConnectionDB.ConnectionString;                        
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = cmmdText;
                cmd.CommandType = cmmdType;

                if (listParameter != null)
                {
                    foreach (DbParameter param in listParameter)
                    {
                        // Adicionando o parâmetro
                        cmd.Parameters.Add(param);
                    }
                }
                // Retorna o comando criado
                return cmd;
            }
            catch (DbException ex)
            {
                throw ex;
            }
        }

        public static DbParameter createParameter(String nameParameter, DbType typeParameter, Object valueParameter, ConnectionDB.TypeDB TpDB)
        {
            // Cria um novo factories de acordo com o nome do provedor
            ConnectionDB.TipoDB = TpDB;
            ConnectionDB.Build();
            DbProviderFactory factory = DbProviderFactories.GetFactory(ConnectionDB.ProviderName);

            // Cria o Parâmetro e add seu valores
            DbParameter param = factory.CreateParameter();
            param.ParameterName = nameParameter;
            param.DbType = typeParameter;
            param.Value = valueParameter;

            // Retorna o Parâmetro criado
            return param;
        }

        /// String SQL ou StoredProcedure
        /// Tipo de Commando (Text ou Stored Procedure
        /// Lista de parâmetros
        /// Comando a ser executado (ExecuteNonQuery, ExecuteReader, ExecuteScalar, ExecuteDataTable)
        /// Object
        public static Object executeCommand(String cmmdText, CommandType cmmdType, List<DbParameter> listParameter, TypeCommand typeCmmd, ConnectionDB.TypeDB TpDB)
        {
            // Cria comando com os dados passado por parâmetro
            DbCommand command = createCommand(cmmdText, cmmdType, listParameter, TpDB);

            // Cria objeto de retorno
            Object objRetorno = null;

            try
            {
                // Abre a Conexão com o banco de dados
                command.Connection.Open();

                switch (typeCmmd)
                {
                    case TypeCommand.ExecuteNonQuery:
                        // Retorna o número de linhas afetadas
                        objRetorno = command.ExecuteNonQuery();
                        break;
                    case TypeCommand.ExecuteReader:
                        // Retorna um DbDataReader
                        objRetorno = command.ExecuteReader(CommandBehavior.CloseConnection);
                        break;
                    case TypeCommand.ExecuteScalar:
                        // Retorna um objeto
                        objRetorno = command.ExecuteScalar();
                        break;
                    case TypeCommand.ExecuteDataTable:
                        // Cria uma tabela
                        DataTable table = new DataTable();
                        // Executa o comando e salva os dados na tabela
                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            table.Load(reader);
                        }
                        // Retorna a tabela
                        objRetorno = table;

                        break;
                }
            }
            catch (Exception ex)
            {
                 throw ex;
            }
            finally
            {
                if (typeCmmd != TypeCommand.ExecuteReader)
                {
                    // Sempre fecha a conexão com o BD
                    command.Connection.Close();
                    command.Connection.Dispose();
                    command.Dispose();
                    command = null;
                }
            }
            return objRetorno;
        }

		public static Object executeCommand(String cmmdText, CommandType cmmdType, List<DbParameter> listParameter, TypeCommand typeCmmd, ConnectionDB.TypeDB TpDB, int _longFetchSize, List<OracleParameter> _listParameter)
		{
			// Cria comando com os dados passado por parâmetro
			OracleCommand command = (OracleCommand)createCommand(cmmdText, cmmdType, listParameter, TpDB);
			command.InitialLONGFetchSize = _longFetchSize;
			if (_listParameter != null)
			{
				command.BindByName = true;
				command.Parameters.AddRange(_listParameter.ToArray());
			}

			// Cria objeto de retorno
			Object objRetorno = null;

			try
			{
				// Abre a Conexão com o banco de dados
                command.Connection.Open();

				switch (typeCmmd)
				{
					case TypeCommand.ExecuteNonQuery:
						// Retorna o número de linhas afetadas
						objRetorno = command.ExecuteNonQuery();
						break;
					case TypeCommand.ExecuteReader:
						// Retorna um DbDataReader
						objRetorno = command.ExecuteReader(CommandBehavior.CloseConnection);
						break;
					case TypeCommand.ExecuteScalar:
						// Retorna um objeto
						objRetorno = command.ExecuteScalar();
						break;
					case TypeCommand.ExecuteDataTable:
						// Cria uma tabela
						DataTable table = new DataTable();
						// Executa o comando e salva os dados na tabela
						using (DbDataReader reader = command.ExecuteReader())
						{
							table.Load(reader);
						}
						// Retorna a tabela
						objRetorno = table;

						break;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (typeCmmd != TypeCommand.ExecuteReader)
				{
					// Sempre fecha a conexão com o BD
                    command.Connection.Close();
                    command.Connection.Dispose();
                    command.Dispose();
                    command = null;
				}
			}
			return objRetorno;
		}

        public static int executeNonQueryWithTransaction(String cmmdText, CommandType cmmdType, List<DbParameter> listParameter, TypeCommand typeCmmd, ConnectionDB.TypeDB TpDB)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                int iRet = 0;
                DbCommand cmd = createCommand(cmmdText, cmmdType, listParameter, TpDB);
                try
                {
                    cmd.Connection.Open();
                    iRet = cmd.ExecuteNonQuery();
                    ts.Complete();
                    return iRet;
                }
                catch (DbException ex)
                {
                    throw ex;
                }
                finally
                {
                    cmd.Connection.Close();
                    ts.Dispose();
                }
            }
        }
    }
}