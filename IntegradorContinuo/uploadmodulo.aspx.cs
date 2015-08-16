using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Data;
using System.Configuration;



namespace WebApplication1
{
    public partial class Default : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var dataset = ObterTabelasParametrizacao();
                CarregarListaAmbiente(dataset); 
                ddlsistema.Items.Add("portalnet");
                chkenvio.Enabled = false;
            }
        }



        protected void btndeploy_Click(object sender, EventArgs e)
        {
            string sistema = ddlsistema.Text;
            string ambiente = ddlambiente.Text;
            lblresultado.Text = "Processando..";
            txt_response.Text = "";
            txt_response.Visible = false;
            if (ImportarPacote(sistema, ambiente))
            {
                txt_response.Visible = true; 
                ExecutarMSBUILD(sistema, ambiente);
            }
        }

        protected void ddlambiente_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dataset = ObterTabelasParametrizacao();
            CarregarListaSistema(dataset);
            chkenvio.Enabled = HabilitarEnvio(ddlambiente.Text, ddlsistema.Text);
        }

        protected void ddlsistema_SelectedIndexChanged(object sender, EventArgs e)
        {

            chkenvio.Enabled = HabilitarEnvio(ddlambiente.Text, ddlsistema.Text);

        }


        protected bool ImportarPacote(string sistema,string ambiente)
        {
            if (!fuparquivo.HasFile)
            {
                lblresultado.ForeColor = System.Drawing.Color.Red;
                lblresultado.Text = "Arquivo para importação não foi informado!";
                return false; 
            }
            //octeto or zip
            if (System.IO.Path.GetExtension(fuparquivo.FileName).ToLower() != ".zip")   
            {
                lblresultado.ForeColor = System.Drawing.Color.Red;
                lblresultado.Text = "Tipo " + fuparquivo.PostedFile.ContentType + " incompatível para realizar a importação.Favor encaminhar arquivo zipado(.zip)!" ;
                return false; 
            }
            if (fuparquivo.PostedFile.FileName.ToLower().IndexOf(sistema) < 0)
            {
                lblresultado.ForeColor = System.Drawing.Color.Red;
                lblresultado.Text = "Arquivo para o sistema " + sistema + " foi informado com nome incorreto! Renomeie para " + sistema + ".zip" ;
                return false; 
            }

                try
                {
                    string caminho = Path.Combine(ConfigurationManager.AppSettings["uploadpath"],ambiente,fuparquivo.FileName); 
                    fuparquivo.SaveAs(caminho);
                    lblresultado.ForeColor = System.Drawing.Color.Blue;
                    lblresultado.Text = "Arquivo: " + fuparquivo.PostedFile.FileName + ",Tipo:" + fuparquivo.PostedFile.ContentType + ", Tamanho:" + fuparquivo.PostedFile.ContentLength + " bytes.<BR>";
                    caminho = Path.Combine(ConfigurationManager.AppSettings["uploadpath"], ambiente,sistema + ".zip"); 
                    if (!File.Exists(caminho))
                    {
                        lblresultado.ForeColor = System.Drawing.Color.Red;
                        lblresultado.Text = "Arquivo" + sistema + ".zip não está disponibilizado no servidor";
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    lblresultado.ForeColor = System.Drawing.Color.Red;  
                    lblresultado.Text = "Nao foi possível importar o arquivo. " + ex.Message;
                    return false;
                }
            
            

            
        }

        protected void ExecutarMSBUILD(string sistema,string ambiente)
        {
            txt_response.ReadOnly = false; 
            txt_response.Text = "";
            try
            { 
                StringBuilder sb = null;
                string line;
                using (Process p = new Process())
                {
                    string param1 = Path.Combine(ConfigurationManager.AppSettings["scriptspath"], "MSUnzip.proj"); ;
                    string param2 = "/t:DeployZip;StopService;BkpApp;CopyToApp;MoveZip;StartService";
                    string param3 = "/p:sistema=" + sistema;
                    string param4 = "/p:ambiente=" + ambiente;
                    string param5 = "";
                    if (chkenvio.Checked)
                    {
                        param5 = "/p:pacotegmud=1";
                    }
                    string command = string.Format("MSBUILD.exe {0} {1} {2} {3} {4}", param1, param2,param3,param4,param5);
                    ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
                    procStartInfo.WorkingDirectory = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319";
                    procStartInfo.RedirectStandardOutput = true;
                    procStartInfo.CreateNoWindow = false;
                    procStartInfo.UseShellExecute = false;
                    procStartInfo.RedirectStandardInput = true;
                    procStartInfo.RedirectStandardOutput = true;
                    procStartInfo.RedirectStandardError = true;
                    p.StartInfo = procStartInfo; 
                    p.Start();

                    sb = new StringBuilder(); 
                    while ((line = p.StandardOutput.ReadLine()) != null) 
                    {
                        sb.AppendLine(line); 
                        
                    }

                    txt_response.Text = sb.ToString();
                    lblresultado.ForeColor = System.Drawing.Color.Blue;
                    lblresultado.Text = lblresultado.Text + "Processo executado em:" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") +  ". VERIFIQUE OS LOGS DE SUA EXECUÇÂO ABAIXO!";
                    Logar(sistema, ambiente, sb.ToString());
                    
                }
            }
            catch(Exception ex)
            {
                lblresultado.ForeColor = System.Drawing.Color.Red;  
                lblresultado.Text = ex.Message;
                Logar(sistema,ambiente,"Erro:" +ex.Message);
            }
            txt_response.ReadOnly = true;

        }

        protected void Logar(string sistema, string ambiente, string msgadicional)
        {
            try
            {
                StringBuilder sb = new StringBuilder(); 
                string msglog = string.Format("Hora:{0};Operacao:{1};Sistema:{2};Ambiente:{3};IPHost:{4}", DateTime.Now.ToString("yyyyMMdd hh:mm:ss"), "importarpacote", sistema, ambiente, Request.UserHostAddress);
                sb.AppendLine(string.Empty);
                sb.AppendLine(msglog);
                sb.AppendLine(msgadicional);  
                string nomearquivo = "ImportarPacote_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string caminholog = Path.Combine(Request.PhysicalApplicationPath,"logs",nomearquivo);
                using (StreamWriter sw = new StreamWriter(caminholog,true))
                { 
                    sw.WriteLine(sb.ToString());   
                    sw.Close(); 
                }
 
            }
            catch
            {
            }
        }


        protected bool HabilitarEnvio(string ambiente,string modulo)
        {
            bool bretorno = false;
            var dataset = ObterTabelasParametrizacao(); 

            DataRow[] rows = dataset.Tables[ambiente].Select();
            chkenvio.Enabled = false;
            foreach (DataRow orow in rows)
            {
                if (orow["modulo"].ToString() == modulo && Convert.ToByte(orow["enviapacote"]) == 1)
                    bretorno = true;
            }
            return bretorno;
        }

        protected DataSet ObterTabelasParametrizacao()
        {
            
            if (Session["dsSistema"] == null)
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml(Path.Combine(Request.PhysicalApplicationPath, "sistemas.xml"));
                Session["dsSistema"] = dataset;
            }

            return (DataSet)Session["dsSistema"];
        }

        protected void CarregarListaAmbiente(DataSet dataset)
        {

            foreach (DataTable dataTable in dataset.Tables)
            {
                ddlambiente.Items.Add(dataTable.TableName);
            }
        }

        protected void CarregarListaSistema(DataSet dataset)
        {
            DataRow[] rows = dataset.Tables[ddlambiente.Text].Select();
            ddlsistema.Items.Clear();
            foreach (DataRow orow in rows)
            {
                ddlsistema.Items.Add(orow["modulo"].ToString());
            }
        }
    }
}