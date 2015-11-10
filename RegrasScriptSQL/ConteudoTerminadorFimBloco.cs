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
                    int linhaend = 0;
                    for (; i < linhas.Length; i++)
                    {
                        linha = linhas[i].ToUpper();
                        if (linha.IndexOf("BEGIN") > -1)
                        {
                            bEncontrouBegin = true;
                        }
                        else if (linha.IndexOf("END") > -1 && bEncontrouBegin)
                        {
                            linhaend = i;
                            break; 
                        }
                    }
                    if (bEncontrouBegin && linhaend > 0)
                    {
                        bool bEncontrouAspas = false;
                        bool bEncontrouBarra = false; 
                        for (i = linhaend; i < linhas.Length; i++)
                        {
                            linha = linhas[i].ToUpper();
                            if (linha.IndexOf(";") > -1 || linha.IndexOf("/") > -1)
                            {
                                if (linha.IndexOf(";") > -1)
                                    bEncontrouAspas = true;

                                if (linha.IndexOf("/") > -1)
                                    bEncontrouBarra = true;
                            }
                            
                        }   
                        if (!(bEncontrouAspas && bEncontrouBarra))
                        {
                            sbmsg.AppendLine("O Arquivo " + msgfinal + " apresenta clausula BEGIN END mas não apresenta  ; e /  após o END  - linha " + (i + 1).ToString());
                        }
                    }

                }         
            


    }
}
