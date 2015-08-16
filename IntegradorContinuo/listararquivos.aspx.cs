using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

namespace WebApplication1
{
    public partial class listararquivos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            {

                DataSet dataset = new DataSet();
                dataset.ReadXml(Path.Combine(Request.PhysicalApplicationPath, "sistemas.xml"));
                Session["dsSistema"] = dataset;

                foreach (DataTable dataTable in dataset.Tables)
                {
                    ddlambiente.Items.Add(dataTable.TableName);
                }
                ddlsistema.Items.Add("portalnet");
            }
        }

        protected void ddlambiente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["dsSistema"] == null)
                Session["dsSistema"] = new DataSet().ReadXml(Path.Combine(Request.PhysicalApplicationPath, "sistemas.xml"));

            DataRow[] rows = ((DataSet)Session["dsSistema"]).Tables[ddlambiente.Text].Select();
            ddlsistema.Items.Clear();
            foreach (DataRow orow in rows)
            {
                ddlsistema.Items.Add(orow["modulo"].ToString());
            }
            


        }

       

        protected void btndeploy_Click(object sender, EventArgs e)
        {
            var DataFiles = CarregarListaArquivos(ddlambiente.Text, ddlsistema.Text);
            lblNoRows.Visible = DataFiles == null ? true : false;
            SortFiles(ref DataFiles, "");
            Session["DataFiles"] = DataFiles;
            articleList.DataSource = DataFiles;
            articleList.DataBind();
        }

        protected void SortFiles(ref FileInfo[] files, string comparacao)
        {
            if (files != null)
            {
                if (comparacao == "Name")
                {
                    Array.Sort(files, delegate(FileInfo f1, FileInfo f2)
                    {
                        return f1.Name.CompareTo(f2.Name);
                    });
                }
                else
                {
                    Array.Sort(files, delegate(FileInfo f1, FileInfo f2)
                    {
                        return f1.LastWriteTime.CompareTo(f2.LastWriteTime);
                    });
                    Array.Reverse(files);
                }
            }
        }

        protected FileInfo[] CarregarListaArquivos(string ambiente,string modulo)
        {

            FileInfo[] files = null;
            if (Session["dsSistema"] == null)
                Session["dsSistema"] = new DataSet().ReadXml(Path.Combine(Request.PhysicalApplicationPath, "sistemas.xml"));

            DataRow[] rows = ((DataSet)Session["dsSistema"]).Tables[ambiente].Select();
            foreach (DataRow orow in rows)
            {
                if (orow["modulo"].ToString() == modulo)
                {
                    string diretorio = orow["pathapp"].ToString();
                    if (!String.IsNullOrEmpty(diretorio) && System.IO.Directory.Exists(diretorio))
                    {
                       DirectoryInfo dirInfo = new DirectoryInfo(diretorio);
                       files = dirInfo.GetFiles();
                       break;
                    }
                }
            }
            return files;
            
        }

        
        protected void articleList_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            FileInfo[] DataFiles = null; 
            if (Session["DataFiles"]==null)
            {
                DataFiles = CarregarListaArquivos(ddlambiente.Text, ddlsistema.Text);
                Session["DataFiles"] = DataFiles;
            }
            else 
            {
                DataFiles = (FileInfo[]) Session["DataFiles"];
            }

            SortFiles(ref DataFiles, e.SortExpression);
            articleList.DataSource = DataFiles;
            articleList.DataBind();
        }

    }

    
}