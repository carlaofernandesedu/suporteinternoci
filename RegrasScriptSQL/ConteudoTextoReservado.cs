﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RegrasScriptSQL
{
    public class ConteudoTextoReservado : BaseValidacaoSQL
    {
        public ConteudoTextoReservado(string listaValidacao)
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
            bool bExiste = false;
            for (int i = 0; i < linhas.Length; i++)
            {
                linha = linhas[i].ToUpper();
                foreach (var texto in textos)
                {
                    if (linha.IndexOf(texto.ToUpper()) > -1)
                    {
                        sbmsg.AppendLine("O Arquivo/Conteudo " + msgfinal + " apresenta o texto: " + texto + " linha: " + (i + 1).ToString() + "  . Verifique se não irá gerar erro na execução");
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
