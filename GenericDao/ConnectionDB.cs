using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDao
{
    public static class ConnectionDB
    {
        public enum TypeDB { ORACLE, SQL };
              
        static ConnectionDB()
        {
           
        }

        public static TypeDB TipoDB;

        /// 
        /// Field String de Conexão
        /// 
        private static string connectionString;

        /// 
        /// Field Nome do Provedor
        /// 
        private static string providerName;

        /// 
        /// Propriedade que apenas informa a String de Conexão
        /// 
        public static string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }
        
        /// 
        /// Propriedade que apenas informa o Nome do Provedor
        /// 
        public static string ProviderName
        {
            get
            {
                return providerName;
            }
        }

        public static void Build()
        {
            try
            {
                string Db = string.Empty;
                if (TipoDB == TypeDB.ORACLE)
                    Db = "ConnectionStringOracle";
                else if (TipoDB == TypeDB.SQL)
                    Db = "ConnectionStringSQL";

                // Recebe do arquivo de configuração Web.Config a string de conexão e o nome do provedor
                connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[Db].ConnectionString;
                providerName = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[Db].ProviderName;
            }
            catch
            {
                throw new Exception("Erro ao receber dados da Conexão. Por favor verifique se a string de conexão está declarada corretamente.");
            }   
        }
    }
}
