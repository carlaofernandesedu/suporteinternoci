using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Data;

namespace WebApplication1
{
    public partial class comparatabelas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarOwner();

                dvtabelas.Visible = false;
                dvcolunas.Visible = false;
                dvindices.Visible = false;
                dvconstraint.Visible = false;
                dvgrant.Visible = false;

                if (Request["p_tabela"] != null  && Request["p_owner"] != null)
                {
                    string tabela = Convert.ToString(Request["p_tabela"]);
                    string owner  = Convert.ToString( Request["p_owner"]);
                    CarregarDados(owner,tabela);
                }
            }
            else
            {
         
            }
        }

        protected void CarregarOwner()
        {
            dropowner.DataSource = new dExecutorScripts().ObterOwners();
            dropowner.DataTextField = "item";
            dropowner.DataBind();
        }

        
        protected void btnsearch_Click(object sender, ImageClickEventArgs e)
        {
            var owner = dropowner.Text;
            var tabela = txtsearch.Value;

            CarregarDados(owner, tabela); 
            
        }


        protected bool CarregaTabelasPrincipais(String owner, String tabela)
        {
            bool bIndicaMaisdeUmRegistro = false;

            var DataTables = new dExecutorScripts().ObterTabela(owner, tabela);

            if (DataTables.Rows.Count == 0)
            {
                bIndicaMaisdeUmRegistro = true;
                dvtabelas.Visible = true; 
                AssociarDadosGrid(dgtabelas, DataTables, lbltabelas);
            }
            return bIndicaMaisdeUmRegistro; 
        }
        
        protected void CarregarDados(String owner, String tabela)
        {
            
            if (tabela.Length < 4 || tabela.Trim() == String.Empty)
            {
                lblvalidacao.Text = "Informe pelo menos 4 caracteres!";
                return;
            }

            dvtabelas.Visible = false;
            dvcolunas.Visible = false;
            dvindices.Visible = false;
            dvconstraint.Visible = false;
            dvgrant.Visible = false;

            if (!CarregaTabelasPrincipais(owner, tabela))
            {
                dvcolunas.Visible = true;
                var DataColumnsTabelaRef = new dExecutorScripts().ObterColunasdaTabela(owner, tabela);
                var DataColumnsTabelaDesenv = new dExecutorScripts().ObterColunasdaTabela(owner, tabela, false);
                var DataColumns = CompareRows(DataColumnsTabelaRef, DataColumnsTabelaDesenv);
                AssociarDadosGrid(dgcolunas, DataColumns, lblcoluna);

                dvindices.Visible = true;
                var DataIndicesTabelaRef = new dExecutorScripts().ObterIndicesdaTabela(owner, tabela);
                var DataIndicesTabelaDesenv = new dExecutorScripts().ObterIndicesdaTabela(owner, tabela,false);
                var DataIndices = CompareRows(DataIndicesTabelaRef, DataIndicesTabelaDesenv);
                AssociarDadosGrid(dgindices, DataIndices, lblindice);

               
                dvconstraint.Visible = true;
                var DataConstraintsTabelaRef = new dExecutorScripts().ObterConstraintsdaTabela(owner, tabela);
                var DataConstraintsTabelaDesenv = new dExecutorScripts().ObterConstraintsdaTabela(owner, tabela,false);
                var DataConstraints = CompareRows(DataConstraintsTabelaRef, DataConstraintsTabelaDesenv);
                AssociarDadosGrid(dgconstraint, DataConstraints, lblconstraint);


                dvgrant.Visible = true;
                var DataGrantsTabelaRef = new dExecutorScripts().ObterGrantsTabela(owner, tabela);
                var DataGrantsTabelaDesenv = new dExecutorScripts().ObterGrantsTabela(owner, tabela, false);
                var DataGrants = CompareRows(DataGrantsTabelaRef, DataGrantsTabelaDesenv);
                AssociarDadosGrid(dggrant, DataGrants, lblgrant);

            }
 
        }

        protected void AssociarDadosGrid(DataGrid dgview, DataTable dtdados, Label lblresultado)
        {
            if (dtdados.Rows.Count > 0)
            {
                lblresultado.Visible = false;
                dgview.DataSource = dtdados;
                dgview.DataBind();
            }
            else
            {
                lblresultado.Visible = true;
                lblresultado.Text = "Nenhum registro encontrado";
                dgview.DataSource = null;
                dgview.DataBind();
            }
        }

        private DataTable CompareRows(DataTable table1, DataTable table2)
        {
            DataTable dtResultado = new DataTable();
            List<string> Colunas = new List<string>();
            DataRow registro = null;

            foreach (DataColumn coluna in table1.Columns)
            {
                dtResultado.Columns.Add(coluna.ColumnName);
                Colunas.Add(coluna.ColumnName);
            }
            dtResultado.Columns.Add("table1");
            dtResultado.Columns.Add("table2");

            foreach (DataRow row1 in table1.Rows)
            {
                bool bRowIgual = false;
                DataRow rowIgual = null;


                foreach (DataRow row2 in table2.Rows)
                {
                    var array1 = row1.ItemArray;
                    var array2 = row2.ItemArray;

                    if (array1.SequenceEqual(array2))
                    {
                        bRowIgual = true;
                        rowIgual = row2;
                        break;
                    }
                }

                registro = dtResultado.NewRow();
                foreach (var coluna in Colunas)
                {
                    registro[coluna] = row1[coluna];
                }

                if (bRowIgual)
                {
                    registro["table1"] = "x";
                    registro["table2"] = "x";
                    table2.Rows.Remove(rowIgual);
                }
                else
                {
                    registro["table1"] = "x";
                    registro["table2"] = "";
                }

                dtResultado.Rows.Add(registro);

            }

            foreach (DataRow row2 in table2.Rows)
            {
                registro = dtResultado.NewRow();
                foreach (var coluna in Colunas)
                {
                    registro[coluna] = row2[coluna];
                }
                registro["table1"] = "";
                registro["table2"] = "x";
                dtResultado.Rows.Add(registro);
            }

            return dtResultado;
        }

    }
}