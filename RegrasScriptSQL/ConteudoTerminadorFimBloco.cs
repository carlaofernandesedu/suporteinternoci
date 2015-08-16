using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RegrasScriptSQL
{
    public class ConteudoTerminadorFimBloco : BaseValidacaoSQL
    {
                public ConteudoTerminadorFimBloco(string listaValidacao)
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

                    string linha;
                    string ultimainstruc = String.Empty;
                    bool bEncontrouBegin = false;
                    int i = 0;
                    for (; i < linhas.Length; i++)
                    {
                        linha = linhas[i].ToUpper();
                        if (linha.IndexOf("BEGIN") > -1)
                        {
                            bEncontrouBegin = true;
                        }
                        else if (linha.IndexOf("END") > -1 && bEncontrouBegin)
                        {
                            ultimainstruc = linha;
                        }
                    }
                    if (!String.IsNullOrEmpty(ultimainstruc))
                    {
                        if (ultimainstruc.IndexOf(";") == -1 || ultimainstruc.IndexOf("/") == -1)
                        {
                            sbmsg.AppendLine("O Arquivo " + msgfinal + " apresenta clausula BEGIN END mas não apresenta  ; e /  após o END  - linha " + (i + 1).ToString());
                        }
                    }

                }         
            


    }
}
