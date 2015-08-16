using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;
using System.Collections;  

namespace RegrasScriptSQL
{
    public class ConteudoLinhaEmBranco : BaseValidacaoSQL
    {
        public ConteudoLinhaEmBranco(string listaValidacao) : base(listaValidacao)
        {
        }

        public override void  validar(ref StringBuilder sbmsg, object validacao)
        {
            var arquivo = (string)validacao;
            string[] linhas = null;
            string msgfinal = "";
            
            if (File.Exists(arquivo))
            {
              // readText  = File.ReadAllText(arquivo).ToUpper();
                //CREATE PROC CREATE OR BEGIN
                //BEGIN AND END 
               linhas = File.ReadAllLines(arquivo);
               msgfinal = Path.GetFileName(arquivo); 
            }
            else 
            {
                linhas = arquivo.Split('\r');
            }

            Dictionary<string, int> registro = new Dictionary<string, int>();
            string linha = null;

            for (int i = 0; i < linhas.Length; i++)
            {
                linha = linhas[i].ToUpper();
                if (linha.IndexOf("CREATE PROC") > -1 || linha.IndexOf("CREATE OR") > -1)
                {
                    if (!registro.ContainsKey("create"))
                        registro.Add("create", i);
                }
                else if (linha.IndexOf("BEGIN") > -1)
                {
                    if (!registro.ContainsKey("begin"))
                        registro.Add("begin", i);
                }
                else if (linha.IndexOf(";") > -1 || linha.IndexOf("/") > -1)
                {
                    if (!registro.ContainsKey("terminador"))
                        registro.Add("terminador", i);

                    break;
                }
                else if (linha.Trim() == "")
                {
                    if (!registro.ContainsKey("branco"))
                        registro.Add("branco", i);

                    break;
                }
             
            }

            var bvalida = false;
            if (registro.Count == 0 || !(registro.ContainsKey("branco")))
            {
                bvalida = true;
            }
            else if (registro.ContainsKey("branco") && registro["branco"] > 0)
            {
                var inicio = registro.Values.First();
                var fim = registro["branco"];
                if (inicio < fim)
                    bvalida = true;
            }

            if (!bvalida)
            {

              sbmsg.AppendLine("O Arquivo/Conteudo " + msgfinal + " apresenta linha em branco - linha:" + registro["branco"].ToString()  + ". Verifique se não irá gerar erro na execução");
            }
            
        }

        
    }
}
