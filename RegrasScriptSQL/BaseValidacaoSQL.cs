using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Web;




namespace RegrasScriptSQL
{
    public abstract class BaseValidacaoSQL
    {

        public  string listaValidacao;

        public BaseValidacaoSQL()
        {
        }

        public BaseValidacaoSQL(string plistaValidacao)
        {
            listaValidacao = plistaValidacao; 
        }

        public abstract void validar(ref StringBuilder sbmsg, object validacao);
        
        
    }


}
