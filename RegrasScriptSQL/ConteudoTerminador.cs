using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RegrasScriptSQL
{
    public class ConteudoTerminador : BaseValidacaoSQL
    {
        public ConteudoTerminador(string listaValidacao)
            : base(listaValidacao)
        {
        }
        
        public override void validar(ref StringBuilder sbmsg, object validacao)
        {
            var arquivo = (string)validacao;
            string readText = "";
            if (File.Exists(arquivo))
            {
                readText = File.ReadAllText(arquivo);
            }
            else
            {
                readText = arquivo;
            }
            char carac = ' ';
            /*
                      9         \t        09
                      10        \n        0A
                      11        \v        0B
                      12        \f        0C
                      13        \r        0D
                      32        space     20
                      59        ;         3B
                      47        /         2F
             */
            bool bvalida = false;
            bool bstop = false;
            for (int i = readText.Length - 1; i > 0; i--)
            {
                carac = readText[i];
                switch (carac)
                {
                    case '\r':
                    case '\n':
                    case '\t':
                    case ' ':
                        break;
                    case ';':
                    case '/':
                        bvalida = true;
                        bstop = true;
                        break;
                    default:
                        bstop = true;
                        break;
                }
                if (bstop)
                    break;
            }
            if (!bvalida)
            {
                sbmsg.AppendLine("O Arquivo " + Path.GetFileName(arquivo) + " não apresenta (; ou /) como terminador do texto.");
            }
        }

        
    }
}
