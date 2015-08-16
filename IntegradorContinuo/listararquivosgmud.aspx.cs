using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Configuration; 

namespace WebApplication1
{
    public partial class listararquivosgmud : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblNoRows.Text = "Arquivos visualizados em " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            var DataFiles = CarregarListaArquivos();
            SortFiles(ref DataFiles, ""); 
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

        protected FileInfo[] CarregarListaArquivos()
        {

            FileInfo[] files = null;
            DirectoryInfo dirInfo = new DirectoryInfo(ConfigurationManager.AppSettings["pathgmud"]);
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

    }

    
}