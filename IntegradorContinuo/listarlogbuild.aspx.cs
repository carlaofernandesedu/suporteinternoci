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
    public partial class listarlogbuild : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["p_arquivo"] != null || Request["hid_parquivo"]!= null)
            {
                string nomearquivo =  Request["p_arquivo"]!=null ? Request["p_arquivo"].ToString() : Request["hid_parquivo"].ToString();
                var dtFiles = CarregarLogArquivo(nomearquivo);
                hid_arquivo.Value = nomearquivo;
                dtFiles.DefaultView.Sort = "LastWriteTime DESC"; 
                dgarquivo.DataSource = dtFiles;
                dgarquivo.DataBind();
            }
            else 
            {
                var DataFiles = CarregarListaArquivos();
                SortFiles(ref DataFiles, "");
                articleList.DataSource = DataFiles;
                articleList.DataBind();
            }
           
        }


        private DataTable  CarregarLogArquivo(string arquivo)
        {
            DataTable dt = new DataTable("arquivos");
            dt.Columns.Add("Name", typeof(String));
            dt.Columns.Add("LastWriteTime", typeof(String));
            dt.Columns.Add("Length", typeof(long));
            
            var nomearq = Path.Combine(ConfigurationManager.AppSettings["pathbuild"],arquivo);
            if (File.Exists(nomearq))
            {
                string line = null;
                using (System.IO.StreamReader file = new System.IO.StreamReader(nomearq))
                {
                    while((line = file.ReadLine()) != null)
                    {
                        var orow = dt.NewRow();
                        orow["Name"] = line;
                        orow["Length"] = 0L;
                        if (File.Exists(line))
                        {
                            var ofile = new FileInfo(line);
                            orow["Name"] = ofile.FullName;
                            orow["LastWriteTime"] = ofile.LastWriteTime;
                            orow["Length"] = ofile.Length;

                        }
                        else if(Directory.Exists(line)) 
                        {
                            DirectoryInfo odir = new DirectoryInfo(line);
                            orow["Name"] = odir.FullName;
                            orow["LastWriteTime"] = odir.LastWriteTime;
                        }
                        dt.Rows.Add(orow);  
                    }
                }
            }
            return dt;
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


        protected void SortFiles(ref DataTable files, string comparacao)
        {
            if (files != null)
            {
                if (comparacao == "Name")
                {
                    files.DefaultView.Sort = "Name ASC";     
                }
                else
                {
                    files.DefaultView.Sort = "LastWriteTime DESC";     
                }
            }
        }



        protected FileInfo[] CarregarListaArquivos()
        {

            FileInfo[] files = null;
            DirectoryInfo dirInfo = new DirectoryInfo(ConfigurationManager.AppSettings["pathbuild"]);
            files = dirInfo.GetFiles();
            return files;
        }


        protected void articleList_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            FileInfo[] DataFiles = null;
            DataFiles = CarregarListaArquivos();
            SortFiles(ref DataFiles, e.SortExpression);
            articleList.DataSource = DataFiles;
            articleList.DataBind();
        }


        protected void dgarquivo_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            DataTable dtFiles = null;
            dtFiles = CarregarLogArquivo(hid_arquivo.Value);
            SortFiles(ref dtFiles, e.SortExpression);
            dgarquivo.DataSource = dtFiles;
            dgarquivo.DataBind();
        }
    }
}