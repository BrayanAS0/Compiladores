
namespace Compliadores_Form
{
    public class Compiladores
    {
        public enum TokenType
        {
            Suma,
            Resta,
            Multiplicacion,
            Division,
            PuntoYComa,
            ParentesisIzquierdo,
            ParentesisDerecho,
            Numero,
            Comentario,
            Error
        }


        public List<Token> Analizar(string expresion)
        {
            var lista = new List<Token>();
            int i = 0;
            while (i < expresion.Length)
            {
                if (i + 1 < expresion.Length && expresion[i] == '/' && expresion[i + 1] == '/')
                {
                    int inicio = i;
                    int buscarDesde = i + 2;
                    int cierre = expresion.IndexOf("//", buscarDesde);
                    if (cierre == -1)
                    {
                        string comentario = expresion.Substring(inicio);
                        lista.Add(new Token(TokenType.Error, "Error", inicio));
                        break;
                    }
                    else
                    {
                        string comentario = expresion.Substring(inicio, cierre + 2 - inicio);
                        lista.Add(new Token(TokenType.Comentario, comentario, inicio));
                        i = cierre + 2;
                        continue;
                    }
                }
                char c = expresion[i];
                switch (c)
                {
                    case '+':
                        lista.Add(new Token(TokenType.Suma, "+", i));
                        break;
                    case '-':
                        lista.Add(new Token(TokenType.Resta, "-", i));
                        break;
                    case '*':
                        lista.Add(new Token(TokenType.Multiplicacion, "*", i));
                        break;
                    case '/':
                        lista.Add(new Token(TokenType.Division, "/", i));
                        break;
                    case ';':
                        lista.Add(new Token(TokenType.PuntoYComa, ";", i));
                        break;
                    case '(':
                        lista.Add(new Token(TokenType.ParentesisIzquierdo, "(", i));
                        break;
                    case ')':
                        lista.Add(new Token(TokenType.ParentesisDerecho, ")", i));
                        break;
                    case ' ':
                        break; // ignorar espacios
                    default:
                        if (char.IsDigit(c))
                        {
                            int start = i;
                            string number = "";
                            while (i < expresion.Length && char.IsDigit(expresion[i]))
                            {
                                number += expresion[i];
                                i++;
                            }
                            lista.Add(new Token(TokenType.Numero, number, start));
                            continue; 
                        }
                        break; 
                }
                i++;
            }
            return lista;
        }
    }

}