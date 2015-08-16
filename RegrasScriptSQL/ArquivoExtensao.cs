using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;




namespace RegrasScriptSQL
{
    public class ArquivoExtensao : BaseValidacaoSQL 
    {

        public ArquivoExtensao(string listaValidacao)
            : base(listaValidacao)
        {
        }


        #region IValidaSQL Members

        public override void validar(ref StringBuilder sbmsg, object validacao)
        {
            //ConfigurationManager.AppSettings["extensao_permitida"]
            var arExtensoes = listaValidacao.Split(';');
            var arquivo = Convert.ToString(((Dictionary<string,object>) validacao)["FileName"]);
            

            var extensao = System.IO.Path.GetExtension(arquivo).ToLower().Replace(".", "");

            if (!arExtensoes.Contains(extensao))
                sbmsg.AppendLine("A extensão do Arquivo " + Path.GetFileName(arquivo) + " é incompatível para realizar a importação.Extensoes permitidas (" + listaValidacao + ")");
        }

        #endregion
    }
}
