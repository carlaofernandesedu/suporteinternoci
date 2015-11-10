using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Xml;


namespace RegrasScriptSQLTest
{
    public static class BaseTest
    {
        private static Dictionary<string, string> nomeRegras;

        private static string[] CarregarArquivos(string nomeClasse , bool bTesteValidaArquivocomFalha  = false)
        {
            string nomearquivo = "_correto";
            
            if (bTesteValidaArquivocomFalha)
                nomearquivo = nomeClasse + "_falha";
            
            var nomeArquivos = System.IO.Directory.GetFiles(@"c:\validador\files", nomearquivo.ToLower());
            return nomeArquivos;

        }

        private static void CarregarRegras()
        {
            if (nomeRegras==null)
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml(Path.Combine(@"c:\validador\config", "regras.xml"));
                var arRegras = dataset.Tables["Regras"];
                nomeRegras = new Dictionary<string, string>();
                foreach (DataRow registro in arRegras.Rows)
                {
                    var nome = Convert.ToString(registro["nome"]);
                    var argumento = Convert.ToString(registro["argumento"]);
                    nomeRegras.Add(nome, argumento);
                }


            }
        }

        public static string Validar(string nomeClasse , bool bTesteValidaArquivocomFalha = false)
        {
            StringBuilder retorno = new StringBuilder();

            BaseValidacaoSQL regra = null;

            CarregarRegras();

            var valor  = nomeRegras[nomeClasse];
        
            switch (nomeClasse)
            {
                case "arquivoextensao":
                    regra = new ArquivoExtensao(valor);
                    break;
                case "arquivotamanho":
                    regra = new ArquivoTamanho(valor);
                    break;
                case "conteudoterminador":
                    regra = new ConteudoTerminador(valor);
                    break;
                case "conteudolinhaembranco":
                    regra = new ConteudoLinhaEmBranco(valor);
                    break;
                case "conteudoobjetoaspas":
                    regra = new ConteudoObjetoAspas(valor);
                    break;
                case "conteudotextoreservado":
                    regra = new ConteudoTextoReservado(valor);
                    break;
                case "conteudotextoduploreservado":
                    regra = new ConteudoTextoDuploReservado(valor);
                    break;
                case "conteudoddl":
                    regra = new ConteudoDDL(valor);
                    break;
                case "conteudodml":
                    regra = new ConteudoDML(valor);
                    break;
                case "conteudoexecucaoduplicada":
                    regra = new ConteudoExecucaoDuplicada(valor);
                    break;
                case "conteudoterminadorfimbloco":
                    regra = new ConteudoTerminadorFimBloco(valor);
                    break;
                default:
                    regra = null;
                    break;
            }

            var arquivos = CarregarArquivos(nomeClasse,bTesteValidaArquivocomFalha);
            foreach (var item in arquivos)
            {
               regra.validar(ref retorno, arquivos);
            }

            return retorno.ToString();
        }
    }
}
