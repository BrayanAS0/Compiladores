using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliadores_Form
{
    public class Compiladores
    {


        public List<List<object>> Analizar(string expresion)
        {
            //var tokens = new List<string>();
            //var tokenTypes = new List<string>();
            //var position = new List<int>();
            var lista = new List<List<object>>();

            int i = 0;
            while (i < expresion.Length)
            {
                char c = expresion[i];
                switch (c)
                {
                    case '+':
                        lista.Add(new List<object> { '+', "Suma", i });
                        //tokens.Add("+");
                        //tokenTypes.Add("Suma");
                        //position.Add(i);
                        i++;
                        break;
                    case '*':
                        lista.Add(new List<object> { '*', "Multiplicacion", i });

                        //tokens.Add("*");
                        //tokenTypes.Add("Multiplicacion");
                        //position.Add(i);
                        i++;
                        break;
                    case ';':
                        lista.Add(new List<object> { ';', "punto y coma", i });

                        //tokens.Add(";");
                        //tokenTypes.Add("punto y coma");
                        //position.Add(i);
                        i++;
                        break;
                    case '(':
                        lista.Add(new List<object> { ')', "Parentesis izquierdo", i });

                        //tokens.Add("(");
                        //tokenTypes.Add("Parentesis izquierdo");
                        //position.Add(i);
                        i++;
                        break;
                    case ')':
                        lista.Add(new List<object> { '(', "Parentesis Derecho", i });

                        //tokens.Add(")");
                        //tokenTypes.Add("parentesis derecho");
                        //position.Add(i);
                        i++;
                        break;
                    case '-':
                        lista.Add(new List<object> { '-', "Resta", i });

                        //tokens.Add("-");
                        //tokenTypes.Add("Resta");
                        //position.Add(i);
                        i++;
                        break;
                    case ' ':
                        i++;
                        break;
                    default:
                        if (char.IsDigit(c))
                        {

                            int start = i;
                            string number = "";
                            lista.Add(new List<object> { "", "Numero", i });

                            //tokenTypes.Add("Numero");
                            //position.Add(i);
                            while (i < expresion.Length && char.IsDigit(expresion[i]))
                            {

                                number += expresion[i];
                                i++;
                            }

                            //tokens.Add(number);
                            lista.Last()[0] = number;
                        }
                        else
                        {
                            i++;
                        }
                        break;
                }
            }
            //return  new JsonResult ( new {
            //     tokens, 
            //position,
            //tokenTypes
            //});
            return lista;

        }

    }
}
