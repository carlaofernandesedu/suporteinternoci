using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegrasScriptSQL
{
    public class RegrasSQL
    {
        public static List<BaseValidacaoSQL> Carregar(Dictionary<string, string> nomeregras)
        {
            BaseValidacaoSQL regra = null;
            List<BaseValidacaoSQL> colRegras = new List<BaseValidacaoSQL>();

            foreach (var registro in nomeregras)
            {

                switch (registro.Key.ToString().ToLower())
                {
                    case "arquivoextensao":
                        regra = new ArquivoExtensao(registro.Value);
                        break;
                    case "arquivotamanho":
                        regra = new ArquivoTamanho(registro.Value);
                        break;
                    case "conteudoterminador":
                        regra = new ConteudoTerminador(registro.Value);
                        break;
                    case "conteudolinhaembranco":
                        regra = new ConteudoLinhaEmBranco(registro.Value);
                        break;
                    case "conteudoobjetoaspas":
                        regra = new ConteudoObjetoAspas(registro.Value);
                        break;
                    case "conteudotextoreservado":
                        regra = new ConteudoTextoReservado(registro.Value);
                        break;
                    case "conteudotextoduploreservado":
                        regra = new ConteudoTextoDuploReservado(registro.Value);
                        break;
                    case "conteudoddl":
                        regra = new ConteudoDDL(registro.Value);
                        break;
                    case "conteudodml":
                        regra = new ConteudoDML(registro.Value);
                        break;
                    case "conteudoexecucaoduplicada":
                        regra = new ConteudoExecucaoDuplicada(registro.Value);
                        break;
                    case "conteudoterminadorfimbloco":
                        regra = new ConteudoTerminadorFimBloco(registro.Value);
                        break;
                    default:
                        regra = null;
                        break;
                }
                if (regra!=null)
                   colRegras.Add(regra);
            }
            return colRegras;
        } 
    }
}
