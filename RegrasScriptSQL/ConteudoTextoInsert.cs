using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RegrasScriptSQL
{
    public class ConteudoTextoInsert : BaseValidacaoSQL
    {
        public ConteudoTextoInsert(string listaValidacao) : base(listaValidacao)
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
            bool bExiste = false; 
            for (int i = 0; i < linhas.Length; i++)
            {
                linha = linhas[i].ToUpper();
                if (linha.IndexOf("INSERT") > -1 && linha.IndexOf("VALUES") > -1) 
                {
                    if (linha.IndexOf("INTO") > -1)
                    {
                        sbmsg.AppendLine("O Arquivo/Conteudo " + msgfinal + " não apresenta o texto:  INTO  linha: " + (i + 1).ToString() + "  . Verifique se não irá gerar erro na execução");
                        bExiste = true;
                    }

                    if (linha.IndexOf(";") > -1)
                    {
                        sbmsg.AppendLine("O Arquivo/Conteudo " + msgfinal + " não apresenta o texto:  ;  linha: " + (i + 1).ToString() + "  . Verifique se não irá gerar erro na execução");
                        bExiste = true;
                    }

                }
                if (bExiste)
                    break;
                
            }
        }
    }
}
