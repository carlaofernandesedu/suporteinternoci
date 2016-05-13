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
    public partial class listararquivospages : System.Web.UI.Page
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
            //if (Session["dsSistema"] == null)
            //    Session["dsSistema"] = new DataSet().ReadXml(Path.Combine(Request.PhysicalApplicationPath, "sistemas.xml"));

            //DataRow[] rows = ((DataSet)Session["dsSistema"]).Tables[ddlambiente.Text].Select();
            //ddlsistema.Items.Clear();
            //foreach (DataRow orow in rows)
            //{
            //    ddlsistema.Items.Add(orow["modulo"].ToString());
            //}
        }

       

        protected void btndeploy_Click(object sender, EventArgs e)
        {
            var path = CarregarListaArquivos(ddlambiente.Text, ddlsistema.Text, txtcaminhosubpasta.Text);
            lblNoRows.Visible = path == String.Empty ? true : false;
            TreeView1.Visible = false;
            
        
            if (!String.IsNullOrEmpty(path))
            {
                if (TreeView1.Nodes.Count > 0)
                {
                    TreeView1.Nodes.Clear();
                }
                DirectoryInfo rootInfo = new DirectoryInfo(path);

                var oroot = new TreeNode(){Text=rootInfo.Name,Value=rootInfo.FullName};
                TreeView1.Nodes.Add(oroot);
                foreach (FileInfo file in rootInfo.GetFiles())
                {
                    //Add each file as Child Node.
                    TreeNode fileNode = new TreeNode
                    {
                        Text = file.Name + " " + file.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss"),
                        Value = file.FullName
                    };
                    oroot.ChildNodes.Add(fileNode);
                }
                                 
                this.PopulateTreeView(rootInfo, oroot);

                TreeView1.Visible = true;

            }
        }

        

        protected string  CarregarListaArquivos(string ambiente,string modulo, string caminhosubpasta)
        {

            string path = String.Empty;
            if (string.IsNullOrEmpty(caminhosubpasta))
                return path;
            
            if (Session["dsSistema"] == null)
                Session["dsSistema"] = new DataSet().ReadXml(Path.Combine(Request.PhysicalApplicationPath, "sistemas.xml"));

            DataRow[] rows = ((DataSet)Session["dsSistema"]).Tables[ambiente].Select();
            foreach (DataRow orow in rows)
            {
                if (orow["modulo"].ToString() == modulo)
                {
                    string diretorio = orow["pathapp"].ToString().ToLower().Replace("bin", "");
                    if (!String.IsNullOrEmpty(diretorio) && System.IO.Directory.Exists(diretorio))
                    {
                       if (System.IO.Directory.Exists(Path.Combine(diretorio,"paginas",caminhosubpasta)))
                       {
                           path = Path.Combine(diretorio, "paginas", caminhosubpasta);
                       }
                       break;
                    }
                }
            }
            return path;
            
        }

        protected void PopulateTreeView(DirectoryInfo dirInfo, TreeNode treeNode)
        {
            foreach (DirectoryInfo directory in dirInfo.GetDirectories())
            {
                TreeNode directoryNode = new TreeNode
                {
                    Text = directory.Name,
                    Value = directory.FullName
                };

                treeNode.ChildNodes.Add(directoryNode);
        
                //Get all files in the Directory.
                foreach (FileInfo file in directory.GetFiles())
                {
                    //Add each file as Child Node.
                    TreeNode fileNode = new TreeNode
                    {
                        Text = file.Name + " " + file.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss"),
                        Value = file.FullName
                    };
                    directoryNode.ChildNodes.Add(fileNode);
                }

                PopulateTreeView(directory,directoryNode);
            }
        }
       

    }

    
}