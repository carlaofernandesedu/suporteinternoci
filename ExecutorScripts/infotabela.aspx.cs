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
    public partial class infotabela : System.Web.UI.Page
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
                    dropowner.Text = owner; 
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


        protected bool CarregaTabelasPrincipais(string owner, ref string tabela)
        {
            bool bIndicaMaisdeUmRegistro = false;

            var DataTables = new dExecutorScripts().ObterTabela(owner, tabela);
            if (DataTables.Rows.Count == 0)
            {
               DataTables = new dExecutorScripts().ObterTabelas(owner, tabela);
               if (DataTables.Rows.Count > 1 || DataTables.Rows.Count == 0)
               {
                   bIndicaMaisdeUmRegistro = true;
                   dvtabelas.Visible = true;
                   AssociarDadosGrid(dgtabelas, DataTables, lbltabelas);
               }
               else 
               {
                   //indica que encontrou somente um parecido
                   tabela = (string)DataTables.Rows[0]["table_name"];
               }
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

            if (!CarregaTabelasPrincipais(owner, ref tabela))
            {
                dvcolunas.Visible = true;
                dvindices.Visible = true;
                dvconstraint.Visible = true;
                dvgrant.Visible = true;

                var DataColumns = new dExecutorScripts().ObterColunasdaTabela(owner, tabela);
                var DataIndices = new dExecutorScripts().ObterIndicesdaTabela(owner, tabela);
                var DataConstraints = new dExecutorScripts().ObterConstraintsdaTabela(owner, tabela);
                var DataGrants = new dExecutorScripts().ObterGrantsTabela(owner, tabela);

                AssociarDadosGrid(dgcolunas, DataColumns, lblcoluna);
                AssociarDadosGrid(dgindices, DataIndices, lblindice);
                AssociarDadosGrid(dgconstraint, DataConstraints, lblconstraint);
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

    }
}