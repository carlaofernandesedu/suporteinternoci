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

namespace WebApplication1
{
    public partial class validascript : System.Web.UI.Page
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
                ddlsistema.Items.Add("todos");
            }

        }

        protected void btndeploy_Click(object sender, EventArgs e)
        {
            ValidarArquivo();
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
                foreach (var regra in colRegras)
                {
                    regra.validar(ref sbmsg, patharquivo);
                }
            }

        }

        protected bool ValidarArquivo()
        {
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
                ValidarInfoArquivo(ref cabec, ref sbmsg);
                cabec.AppendLine("Resultado:");
                if (sbmsg.Length > 0)
                {
                    txt_response.Text = String.Concat(cabec.ToString(), sbmsg.ToString());
                    return false;
                }
                sbmsg.Clear();
                string caminhotemp = ObterPastaTemporaria();
                ValidarInfoConteudo(ref cabec, ref sbmsg, caminhotemp);
                if (sbmsg.Length > 0)
                {
                    txt_response.Text = String.Concat(cabec.ToString(), sbmsg.ToString()); ;
                    return false;
                }

                txt_response.Text = String.Concat(cabec.ToString(), "Arquivo(s) foram validado(s) corretamente");
                return true;

            }
            catch (Exception ex)
            {
                txt_response.ForeColor = System.Drawing.Color.Red;
                txt_response.Text = "Nao foi possível importar o(s) arquivo(s). " + ex.Message;
                return false;
            }

        }
    
    #endregion

        #region "Log Eventos"
        protected void Logar(string sistema, string ambiente, string msgadicional)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string msglog = string.Format("Hora:{0};Operacao:{1};Sistema:{2};Ambiente:{3};IPHost:{4}", DateTime.Now.ToString("yyyyMMdd hh:mm:ss"), "scriptbancodados", sistema, ambiente, Request.UserHostAddress);
                sb.AppendLine(string.Empty);
                sb.AppendLine(msglog);
                sb.AppendLine(msgadicional);
                string nomearquivo = "ScriptBancoDados_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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