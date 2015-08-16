using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RegrasScriptSQL
{
    public class ConteudoObjetoAspas : BaseValidacaoSQL
    {
        public ConteudoObjetoAspas(string listaValidacao)
            : base(listaValidacao)
        {
        }

        public override void validar(ref StringBuilder sbmsg, object validacao)
        {
            var arquivo = (string)validacao;
            string[] linhas = null;
            string msgfinal = "";

            if (File.Exists(arquivo))
            {
                linhas = File.ReadAllLines(arquivo);
                msgfinal = Path.GetFileName(arquivo);
            }
            else
            {
                linhas = arquivo.Split('\r');
            }

            string linha = null;
            string[] textos = listaValidacao.Split(';');
            for (int i = 0; i < linhas.Length; i++)
            {
                linha = linhas[i].ToUpper();
                if ((linha.IndexOf("CREATE") == 0 || linha.IndexOf("ALTER") == 0 || linha.IndexOf(" CREATE ") > -1 || linha.IndexOf(" ALTER ") > -1) && (linha.IndexOf("\"") > -1 || linha.IndexOf("'") > -1))
                {
                    sbmsg.AppendLine("O Arquivo " + msgfinal + " apresenta aspas no nomes do objeto - linha " + (i+1).ToString());
                    break;
                }
                
            }
        }

    }
}
