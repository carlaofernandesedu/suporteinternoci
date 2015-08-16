using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using RegrasScriptSQL;
using System.Data;

namespace ExecutorScript
{
    public partial class execscript : System.Web.UI.Page
    {

    #region "Parametrizacoes e Suporte"
        protected DataSet ObterParametrosRegras()
        {

            if (Session["dsRegras"] == null)
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml(Path.Combine(Request.PhysicalApplicationPath, "regras.xml"));
                Session["dsRegras"] = dataset;
            }

            return (DataSet)Session["dsRegras"];
        }

        protected bool ValidarCamposObrigatorios()
        {
            bool retorno = true;
            if (String.IsNullOrWhiteSpace(txtUsuario.Text.Trim()) || String.IsNullOrWhiteSpace(txtsenha.Text.Trim()))
            {
                txt_response.ForeColor = System.Drawing.Color.Red;
                txt_response.Text = "Owner e/ou senha não informados";
                retorno = false;
            }
            return retorno;
        }

        protected string ObterPastaTemporaria()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "").Substring(1, 4);
            string caminhotemp = Path.Combine(ConfigurationManager.AppSettings["scriptspathtemp"], String.Concat(DateTime.Now.ToString("yyyyMMdd_hhmmss"), guid));
            if (!Directory.Exists(caminhotemp))
                Directory.CreateDirectory(caminhotemp);
            return caminhotemp;
        }

    #endregion

    #region "Regras SQL Carga"

        List<BaseValidacaoSQL> colRegras = null;

        protected void CarregarRegrasSQL(string grupo)
        {
            var dsTabelas = ObterParametrosRegras();
            var arRegras = dsTabelas.Tables["Regras"].Select("tipo='" + grupo + "'");
            Dictionary<string, string> dicRegras = new Dictionary<string, string>();
            foreach (var registro in arRegras)
            {
                dicRegras.Add(registro["nome"].ToString(), registro["argumento"].ToString());
            }
            colRegras = RegrasScriptSQL.RegrasSQL.Carregar(dicRegras);
        }


    #endregion

    #region "Eventos"
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
            }

        }

       


        protected void btndeploy_Click(object sender, EventArgs e)
        {
            try
            {
                txt_response.ReadOnly = false;
                if (ValidarCamposObrigatorios())
                {
                    if (PersistirArquivo())
                    {
                        if (Session["ArquivoExecucao"] != null)
                            ExecutarShellPLSQL(txtUsuario.Text, txtsenha.Text, "GDAE-PROD", Convert.ToString(Session["ArquivoExecucao"]));
                    }
                }
            }
            catch (Exception ex)
            {
                txt_response.ForeColor = System.Drawing.Color.Red;
                txt_response.Text = "Erro:" + ex.Message;
            }
            finally
            {
                txt_response.ReadOnly = true;
            }
        }

    #endregion
    
    #region "Validações do Arquivo"

        protected void ValidarInfoArquivo(ref StringBuilder cabec, ref StringBuilder sbmsg)
        {
            //Validacao Caracteristicas Arquivo
            CarregarRegrasSQL("arquivo");
            Dictionary<string, object> dadosArquivo = new Dictionary<string, object>();
            dadosArquivo.Add("FileName", null);
            dadosArquivo.Add("ContentLength", null);
            foreach (var arquivo in fuparquivo.PostedFiles)
            {
                cabec.AppendLine(Path.GetFileName(arquivo.FileName));
                dadosArquivo["FileName"] = arquivo.FileName;
                dadosArquivo["ContentLength"] = arquivo.ContentLength;
                foreach (var regra in colRegras)
                {
                    regra.validar(ref sbmsg, dadosArquivo);
                }
            }
        }

        protected void ValidarInfoConteudo(ref StringBuilder cabec, ref StringBuilder sbmsg, string caminhotemp)
        {
            //Validacao Caracteristicas Arquivo
            CarregarRegrasSQL("conteudo");
            foreach (var arquivo in fuparquivo.PostedFiles)
            {
                cabec.AppendLine(Path.GetFileName(arquivo.FileName));
                var patharquivo = Path.Combine(caminhotemp, Path.GetFileName(arquivo.FileName));
                arquivo.SaveAs(patharquivo);
                Session["ArquivoExecucao"] = patharquivo;
                foreach (var regra in colRegras)
                {
                    regra.validar(ref sbmsg, patharquivo);
                }
            }

        }

        protected bool PersistirArquivo()
        {
            string nomearquivolog = string.Empty;
            if (!fuparquivo.HasFile)
            {
                txt_response.ForeColor = System.Drawing.Color.Red;
                txt_response.Text = "Arquivo para importação não foi informado.";
                return false;
            }
            try
            {
                StringBuilder sbmsg = new StringBuilder();
                StringBuilder cabec = new StringBuilder();
                cabec.AppendLine("Execução ocorrida em " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " para o(s) arquivo(s) abaixo:");
                ValidarInfoArquivo(ref cabec,ref sbmsg);
                cabec.AppendLine("Resultado:");
                /*
                if (sbmsg.Length > 0)
                {
                    txt_response.Text = String.Concat(cabec.ToString(),sbmsg.ToString());
                    return false;
                }
                */
                bool bretorno = true;
                Session["PathArquivoExecucao"] = ObterPastaTemporaria();
                ValidarInfoConteudo(ref cabec, ref sbmsg, Convert.ToString(Session["PathArquivoExecucao"])); 
                if (sbmsg.Length > 0)
                {
                    txt_response.Text = String.Concat(cabec.ToString(), sbmsg.ToString()); ;
                    //indica falta de terminador
                    if (sbmsg.ToString().ToLower().IndexOf("(; ou /)") > -1)
                    {
                        txt_response.Text +="\n A ausencia de um terminador (; ou /) impede a execução do script!";
                        bretorno = false;
                    }
                }


                Logar(Path.Combine(Convert.ToString(Session["PathArquivoExecucao"]),  "execucao.log"), txt_response.Text);
                return bretorno;

            }
            catch (Exception ex)
            {
                txt_response.ForeColor = System.Drawing.Color.Red;
                txt_response.Text = "Nao foi possível importar o(s) arquivo(s). " + ex.Message;
                return false;
            }

        }

#endregion

    #region "Execucao Script"
        protected void ExecutarShellPLSQL(string owner, string senha,  string ambiente, string pathnomearq)
        {
            
            try
            {
                StringBuilder sb = null;
                string line;
                
                string pathexecucao = Convert.ToString(Session["PathArquivoExecucao"]);
                string textotemplate = File.ReadAllText(Path.Combine(Request.PhysicalApplicationPath,"template.sql"));
                textotemplate = textotemplate.Replace("template", pathnomearq);
                string pathnomearqtemplate = Path.Combine(pathexecucao,"template.sql");
                File.WriteAllText(pathnomearqtemplate, textotemplate);

                Logar(Path.Combine(Convert.ToString(Session["PathArquivoExecucao"]), "execucao.log"), "inicio execucao");
             
                using (Process p = new Process())
                {
                    string param1 = owner + "/" + senha + "@" + ambiente;
                    string param2 = " @" + pathnomearqtemplate;
                    string param3 = "";
                    // EXIT SQL.SQLCODE
                    string command = string.Format("sqlplus {0} {1} {2}", param1, param2, param3);
                    var pathsqlplus = Convert.ToString(ConfigurationManager.AppSettings["execsqlpath"]);
                    ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
                    procStartInfo.WorkingDirectory = pathsqlplus;
                    procStartInfo.RedirectStandardOutput = true;
                    procStartInfo.CreateNoWindow = false;
                    procStartInfo.UseShellExecute = false;
                    procStartInfo.RedirectStandardInput = true;
                    procStartInfo.RedirectStandardOutput = true;
                    procStartInfo.RedirectStandardError = true;
                    p.StartInfo = procStartInfo;
                    try
                    {
                        p.Start();

                        sb = new StringBuilder();
                        while ((line = p.StandardOutput.ReadLine()) != null)
                        {
                            sb.AppendLine(line);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logar(owner, ambiente, sb.ToString() + ex.ToString());
                    }

                    txt_response.ForeColor = System.Drawing.Color.Blue;
                    txt_response.Text = txt_response.Text + "\nScript executado no SQL PLUS. Favor verifique o log da execução abaixo\n";
                    txt_response.Text += sb.ToString();
                    Logar(owner, ambiente, sb.ToString());
                    Logar(Path.Combine(Convert.ToString(Session["PathArquivoExecucao"]), "execucao.log"), sb.ToString());

                }
            }
            catch (Exception ex)
            {
                txt_response.ForeColor = System.Drawing.Color.Red;
                txt_response.Text = ex.Message;
                Logar(owner, ambiente, "Erro:" + ex.Message);
            }

        }

#endregion 

    #region "Log Eventos"

        protected void Logar(string pathnomearquivo, string msgadicional)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string msglog = string.Format("Hora:{0};Operacao:{1};IPHost:{2}", DateTime.Now.ToString("yyyyMMdd hh:mm:ss"), "scriptbancodados", Request.UserHostAddress);
                sb.AppendLine(string.Empty);
                sb.AppendLine(msglog);
                sb.AppendLine(msgadicional);
                string nomearquivo = pathnomearquivo.Replace(".txt",".log");
                using (StreamWriter sw = new StreamWriter(pathnomearquivo, true))
                {
                    sw.WriteLine(sb.ToString());
                    sw.Close();
                }

            }
            catch
            {
            }
        }
        protected void Logar(string sistema, string ambiente, string msgadicional)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string msglog = string.Format("Hora:{0};Operacao:{1};Sistema:{2};Ambiente:{3};IPHost:{4}", DateTime.Now.ToString("yyyyMMdd hh:mm:ss"), "scriptbancodados", sistema, ambiente, Request.UserHostAddress);
                sb.AppendLine(string.Empty);
                sb.AppendLine(msglog);
                sb.AppendLine(msgadicional);
                string nomearquivo = "ExecScriptBancoDados_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string caminholog = Path.Combine(Request.PhysicalApplicationPath, "logs", nomearquivo);
                using (StreamWriter sw = new StreamWriter(caminholog, true))
                {
                    sw.WriteLine(sb.ToString());
                    sw.Close();
                }

            }
            catch
            {
            }
        }
#endregion  

       
 
    }
}