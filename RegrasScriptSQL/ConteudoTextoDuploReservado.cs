using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RegrasScriptSQL
{
    public class ConteudoTextoDuploReservado : BaseValidacaoSQL
    {
        public ConteudoTextoDuploReservado(string listaValidacao) : base(listaValidacao)
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
            bool bExiste = false;
            for (int indlinha = 0; indlinha < linhas.Length; indlinha++)
            {
                linha = linhas[indlinha].ToUpper();
                for (int indtexto = 0; indtexto < textos.Length; indtexto += 2)
                {
                    if (linha.IndexOf(textos[indtexto].ToUpper()) > -1 && linha.IndexOf(textos[indtexto + 1].ToUpper()) > -1)
                    {
                        sbmsg.AppendLine("O Arquivo/Conteudo " + msgfinal + " apresenta o texto: " + textos[indtexto] + " linha: " + (indlinha + 1).ToString() + "  . Verifique se não irá gerar erro na execução");
                        bExiste = true;
                        break;
                    }
                }
                if (bExiste)
                    break;
                
            }
        }
    }
}
