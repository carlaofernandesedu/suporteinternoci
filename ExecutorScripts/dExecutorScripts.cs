using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GenericDao;
using System.Data;
using Oracle.DataAccess.Client;


namespace WebApplication1
{
    public class dExecutorScripts  : DataAccessObject
    {

        #region "GenericDao"


        public DataTable ObterTabela(string owner, string tabela, bool bBaseref = true)
        {
            var dttabela = new DataTable();
            dttabela.Columns.Add("owner");
            dttabela.Columns.Add("table_name");

            try
            {

                
                var sqlquery = "select owner, table_name from all_tables where UPPER(owner) = :P_OWNER and UPPER(table_name) = :P_TABELA order by table_name";

                QueryParameters.Add(new Oracle.DataAccess.Client.OracleParameter("P_OWNER", OracleDbType.Varchar2, owner.ToUpper(), ParameterDirection.Input));
                QueryParameters.Add(new Oracle.DataAccess.Client.OracleParameter("P_TABELA", OracleDbType.Varchar2,tabela.ToUpper(), ParameterDirection.Input));

                using (OracleDataReader dr = bBaseref == true ?  GenericFactory.executeCommand(sqlquery, System.Data.CommandType.Text, null, TypeCommand.ExecuteReader,
                        ConnectionDB.TypeDB.ORACLE, 0, QueryParameters) as OracleDataReader : DataAccessObject.ExecutarComando(sqlquery,System.Data.CommandType.Text, QueryParameters) )
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                var registro = dttabela.NewRow();
                                registro["owner"] = dr.GetString(0);
                                registro["table_name"] = dr.GetString(1);
                                dttabela.Rows.Add(registro);
                            }
                        }

                        return dttabela;
                    }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataTable ObterTabelas(string owner,string tabela)
        {
            var dttabela = new DataTable();
            dttabela.Columns.Add("owner");
            dttabela.Columns.Add("table_name");

            try
            {

                tabela =  tabela + '%';
                var sqlquery = "select owner, table_name from all_tables where UPPER(owner) = :P_OWNER and UPPER(table_name) LIKE :P_TABELA order by table_name";

                QueryParameters.Add(new Oracle.DataAccess.Client.OracleParameter("P_OWNER", OracleDbType.Varchar2, owner.ToUpper(), ParameterDirection.Input));
                QueryParameters.Add(new Oracle.DataAccess.Client.OracleParameter("P_TABELA", OracleDbType.Varchar2, tabela.ToUpper(), ParameterDirection.Input));

                using (OracleDataReader dr = GenericFactory.executeCommand(sqlquery, System.Data.CommandType.Text, null, TypeCommand.ExecuteReader,
                    ConnectionDB.TypeDB.ORACLE, 0, QueryParameters) as OracleDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            var registro = dttabela.NewRow();
                            registro["owner"] = dr.GetString(0);
                            registro["table_name"] = dr.GetString(1);
                            dttabela.Rows.Add(registro);
                        }
                    }

                    return dttabela;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataTable ObterOwners()
        {
            var dtowner = new DataTable();
            dtowner.Columns.Add("item");

            try
            {
                
                
                //var sqlquery = "select distinct owner from  dba_segments where owner not in ('SYSTEM', 'XDB', 'SYS', 'TSMSYS', 'MDSYS', 'EXFSYS', 'WMSYS', 'ORDSYS', 'OUTLN', 'DBSNMP') order by owner";

                //var sqlquery = "select distinct owner from  dba_segments order by owner";

                var sqlquery = "select distinct owner from ALL_OBJECTS where owner not in ('PUBLIC','ORACLE_OCM','APPQOSSYS','SYSTEM', 'XDB', 'SYS', 'TSMSYS', 'MDSYS', 'EXFSYS', 'WMSYS', 'ORDSYS', 'OUTLN', 'DBSNMP') order by owner "; 

                using (OracleDataReader dr = GenericFactory.executeCommand(sqlquery, System.Data.CommandType.Text, null, TypeCommand.ExecuteReader,
                    ConnectionDB.TypeDB.ORACLE, 0, null) as OracleDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            var registroowner = dtowner.NewRow();
                            registroowner["item"] = dr.GetString(0);
                            dtowner.Rows.Add(registroowner);
                        }
                    }

                    return dtowner;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        
        public DataTable ObterConstraintsdaTabela(string owner, string tabela, bool bBaseRef = true)
        {
            try
            {
                var dtconstraint = new DataTable();
                dtconstraint.Columns.Add("owner");
                dtconstraint.Columns.Add("table_name");
                dtconstraint.Columns.Add("constraint_name");
                dtconstraint.Columns.Add("column_name");
                dtconstraint.Columns.Add("position");
                dtconstraint.Columns.Add("ref_owner");
                dtconstraint.Columns.Add("ref_table");
                
              
                LimparQuery();

                
                QuerySQL.AppendLine(" select ");
                QuerySQL.AppendLine("    cons.owner            as owner, ");
                QuerySQL.AppendLine("    cons.table_name       as table_name, ");
                QuerySQL.AppendLine("    cons.constraint_name     constraint_name, ");
                QuerySQL.AppendLine("    cons.constraint_type     constraint_type, ");
                QuerySQL.AppendLine("    col.owner                ref_owner,  ");
                QuerySQL.AppendLine("    col.table_name           ref_table, ");
                QuerySQL.AppendLine("    colp.column_name         column_name, ");
                QuerySQL.AppendLine("    colp.position ");
                QuerySQL.AppendLine(" from ");
                QuerySQL.AppendLine("    dba_cons_columns      col, ");
                QuerySQL.AppendLine("    dba_constraints       cons, ");
                QuerySQL.AppendLine("    dba_cons_columns      colp ");
                QuerySQL.AppendLine(" where cons.r_owner = col.owner ");
                QuerySQL.AppendLine(" and   cons.r_constraint_name = col.constraint_name ");
                QuerySQL.AppendLine(" and   cons.owner = colp.owner ");
                QuerySQL.AppendLine(" and   cons.constraint_name = colp.constraint_name ");
                QuerySQL.AppendLine(" and   cons.CONSTRAINT_TYPE IN ('R') ");
                QuerySQL.AppendLine(" and   UPPER(cons.table_name) = :P_TABELA ");
                QuerySQL.AppendLine(" and   UPPER(cons.owner) = :P_OWNER ");
                QuerySQL.AppendLine(" UNION ");
                QuerySQL.AppendLine(" SELECT  ");
                QuerySQL.AppendLine("    b.owner               owner,  ");
                QuerySQL.AppendLine("    b.TABLE_NAME          table_name, ");
                QuerySQL.AppendLine("    b.constraint_name     constraint_name, ");
                QuerySQL.AppendLine("    b.constraint_type     constraint_type, ");
                QuerySQL.AppendLine("    ''                    ref_owner,  ");
                QuerySQL.AppendLine("    ''                    ref_table, ");
                QuerySQL.AppendLine("    a.column_name         column_name, ");
                QuerySQL.AppendLine("    a.position ");
                QuerySQL.AppendLine(" FROM DBA_CONS_COLUMNS A, DBA_CONSTRAINTS B ");
                QuerySQL.AppendLine(" WHERE a.CONSTRAINT_NAME = b.CONSTRAINT_NAME ");
                QuerySQL.AppendLine(" and b.CONSTRAINT_TYPE IN ('P') ");
                QuerySQL.AppendLine(" AND a.OWNER = b.OWNER ");
                QuerySQL.AppendLine(" AND UPPER(a.TABLE_NAME) = :P_TABELA ");
                QuerySQL.AppendLine(" AND a.OWNER = :P_OWNER  ");
                QuerySQL.AppendLine(" order by constraint_name, position  ");
                


                QueryParameters.Add(new Oracle.DataAccess.Client.OracleParameter("P_OWNER", OracleDbType.Varchar2, owner.ToUpper(), ParameterDirection.Input));
                QueryParameters.Add(new Oracle.DataAccess.Client.OracleParameter("P_TABELA", OracleDbType.Varchar2, tabela.ToUpper() , ParameterDirection.Input));


                using (OracleDataReader dr = bBaseRef == true ? GenericFactory.executeCommand(QuerySQL.ToString(), System.Data.CommandType.Text, null, TypeCommand.ExecuteReader,
                    ConnectionDB.TypeDB.ORACLE, 0, QueryParameters) as OracleDataReader : DataAccessObject.ExecutarComando(QuerySQL.ToString(),System.Data.CommandType.Text, QueryParameters))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            var registro = dtconstraint.NewRow();
                            registro["owner"] = dr["owner"].ToString();
                            registro["table_name"] = dr["table_name"].ToString();
                            registro["constraint_name"] = dr["constraint_name"].ToString();
                            registro["column_name"] = dr["column_name"].ToString(); 
                            registro["position"] = dr["position"].ToString();
                            registro["ref_owner"] = dr["ref_owner"].ToString();
                            registro["ref_table"] = dr["ref_table"].ToString(); ;
                            dtconstraint.Rows.Add(registro);
                        }
                    }

                    return dtconstraint;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
             
        }

        public DataTable ObterIndicesdaTabela(string owner, string tabela, bool bBaseRef = true)
        {
            
            try
            {
                var dtindice = new DataTable();
                dtindice.Columns.Add("index_name");
                dtindice.Columns.Add("table_name");
                dtindice.Columns.Add("column_name");
                dtindice.Columns.Add("column_position");

                var sqlquery = "select index_name, table_owner as table_name, COLUMN_NAME,COLUMN_POSITION from dba_ind_columns where UPPER(index_owner) = :P_OWNER and UPPER(table_name) = :P_TABELA  order by index_name,column_position";

                QueryParameters.Add(new Oracle.DataAccess.Client.OracleParameter("P_OWNER", OracleDbType.Varchar2, owner.ToUpper(), ParameterDirection.Input));
                QueryParameters.Add(new Oracle.DataAccess.Client.OracleParameter("P_TABELA", OracleDbType.Varchar2, tabela.ToUpper() , ParameterDirection.Input));


                using (OracleDataReader dr = bBaseRef == true ?  GenericFactory.executeCommand(sqlquery, System.Data.CommandType.Text, null, TypeCommand.ExecuteReader,
                    ConnectionDB.TypeDB.ORACLE, 0, QueryParameters) as OracleDataReader : DataAccessObject.ExecutarComando(sqlquery, System.Data.CommandType.Text, QueryParameters))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            var registro = dtindice.NewRow();
                            registro["index_name"] = dr.GetString(0);
                            registro["table_name"] = dr.GetString(1);
                            registro["column_name"] = dr.GetString(2);
                            registro["column_position"] = dr["column_position"].ToString();
                            dtindice.Rows.Add(registro);
                        }
                    }

                    return dtindice; 
                }
            }
            catch(Exception ex ) 
            {
                throw ex;
            }
             
        }



        public DataTable ObterColunasdaTabela(string owner, string tabela, bool bBaseRef = true)
        {
            try
            {
                var dtcoluna = new DataTable();
                dtcoluna.Columns.Add("column_name");
                dtcoluna.Columns.Add("data_type");
                dtcoluna.Columns.Add("nullable");
                dtcoluna.Columns.Add("data_length");
                dtcoluna.Columns.Add("table_name");


                var sqlquery = "select column_name,data_type ,nullable,data_length,table_name from dba_tab_columns where UPPER(owner) = :P_OWNER and UPPER(table_name) = :P_TABELA order by column_name";

                QueryParameters.Add(new Oracle.DataAccess.Client.OracleParameter("P_OWNER", OracleDbType.Varchar2, owner.ToUpper() , ParameterDirection.Input));
                QueryParameters.Add(new Oracle.DataAccess.Client.OracleParameter("P_TABELA", OracleDbType.Varchar2, tabela.ToUpper(), ParameterDirection.Input));


                using (OracleDataReader dr = bBaseRef == true ? GenericFactory.executeCommand(sqlquery, System.Data.CommandType.Text, null, TypeCommand.ExecuteReader,
                    ConnectionDB.TypeDB.ORACLE, 0, QueryParameters) as OracleDataReader: DataAccessObject.ExecutarComando(sqlquery, System.Data.CommandType.Text, QueryParameters))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            var registro = dtcoluna.NewRow();
                            registro["column_name"] = dr.GetString(0);
                            registro["data_type"] = dr.GetString(1);
                            registro["nullable"] = dr.GetString(2) == "Y" ? "S" : "N";
                            registro["data_length"] = dr["data_length"].ToString();
                            registro["table_name"] = dr["table_name"].ToString();
                            dtcoluna.Rows.Add(registro);
                        }
                    }

                    return dtcoluna; 
                }
            }
            catch(Exception ex ) 
            {
                throw ex;
            }

        }


        public DataTable ObterGrantsTabela(string owner, string tabela, bool bBaseRef = true)
        {
            try
            {
                //grantee, owner, table_name,grantor, privilege
                var dtgrant = new DataTable();
                dtgrant.Columns.Add("grantee");
                dtgrant.Columns.Add("owner");
                dtgrant.Columns.Add("grantor");
                dtgrant.Columns.Add("privilege");
                dtgrant.Columns.Add("table_name");

                var sqlquery = "select grantee, owner, grantor, privilege,table_name from dba_tab_privs where UPPER(owner) = :P_OWNER and UPPER(table_name) = :P_TABELA order by  grantor,privilege";

                QueryParameters.Add(new Oracle.DataAccess.Client.OracleParameter("P_OWNER", OracleDbType.Varchar2, owner.ToUpper(), ParameterDirection.Input));
                QueryParameters.Add(new Oracle.DataAccess.Client.OracleParameter("P_TABELA", OracleDbType.Varchar2, tabela.ToUpper(), ParameterDirection.Input));


                using (OracleDataReader dr = bBaseRef == true ? GenericFactory.executeCommand(sqlquery, System.Data.CommandType.Text, null, TypeCommand.ExecuteReader,
                    ConnectionDB.TypeDB.ORACLE, 0, QueryParameters) as OracleDataReader : DataAccessObject.ExecutarComando(sqlquery, System.Data.CommandType.Text, QueryParameters))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            var registro = dtgrant.NewRow();
                            registro["grantee"] = dr.GetString(0);
                            registro["owner"] = dr.GetString(1);
                            registro["grantor"] = dr.GetString(2);
                            registro["privilege"] = dr.GetString(3);
                            registro["table_name"] = dr.GetString(4);
                            dtgrant.Rows.Add(registro);
                        }
                    }

                    return dtgrant;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        
        #endregion

    }
}