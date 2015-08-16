using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RegrasScriptSQL
{
    public class ArquivoTamanho : BaseValidacaoSQL
    {
        public ArquivoTamanho(string listaValidacao)
            : base(listaValidacao)
        {
        }
        
        #region IValidaSQL Members

        public override void validar(ref StringBuilder sbmsg, object validacao)
        {
            var arquivo = (Dictionary<string,object>) validacao;
            if (Convert.ToInt32(arquivo["ContentLength"]) > Int32.Parse(listaValidacao))
                sbmsg.AppendLine("O tamanho do Arquivo " + Path.GetFileName(Convert.ToString(arquivo["FileName"])) + " deve ser menor que "  +  listaValidacao + " Bytes");
        
        }

        #endregion
    }
}
