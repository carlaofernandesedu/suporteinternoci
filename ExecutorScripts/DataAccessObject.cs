using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Text;
using System.Data;
//
using Oracle.DataAccess.Client;

namespace WebApplication1
{
    public class DataAccessObject
    {
        protected StringBuilder QuerySQL { get; private set; }
        protected List<OracleParameter> QueryParameters { get; private set; }        
        
        public DataAccessObject()
        {
            QuerySQL = new StringBuilder();
            QueryParameters = new List<OracleParameter>();
        }
        
        public void LimparQuery()
        {
            QuerySQL.Remove(0, QuerySQL.Length);
            QueryParameters.Clear();            
        }


        public static OracleDataReader ExecutarComando(String cmmdText, CommandType cmmdType, List<OracleParameter> listParameter)
        {
            // Cria comando com os dados passado por parâmetro
            OracleCommand command = CriarComando(cmmdText, cmmdType, listParameter);
            
            command.InitialLONGFetchSize = 0;
            

            // Cria objeto de retorno
            OracleDataReader  objRetorno = null;
            try
            {
                // Abre a Conexão com o banco de dados
                command.Connection.Open();
                 // Retorna um DbDataReader
                objRetorno = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                    // Sempre fecha a conexão com o BD
                    //command.Connection.Close();
                    //command.Connection.Dispose();
                    //command.Dispose();
                    //command = null;
            }
            return objRetorno;
        }

        private static OracleCommand CriarComando(String cmmdText, CommandType cmmdType, List<OracleParameter> listParameter)
        {
            try
            {
                var conn = new OracleConnection();
                conn.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionStringOracleDesenv"].ConnectionString;
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = cmmdText;
                cmd.CommandType = cmmdType;

                if (listParameter != null)
                {
                    cmd.BindByName = true;
                    cmd.Parameters.AddRange(listParameter.ToArray());
                }
                // Retorna o comando criado
                return cmd;
            }
            catch (DbException ex)
            {
                throw ex;
            }
        }


        ~DataAccessObject()
        { }
    }
}