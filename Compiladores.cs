

namespace Compliadores_Form
{


    public class Compiladores
    {
        public enum TokenType
        {
            // Operadores
            Suma,
            Resta,
            Multiplicacion,
            Division,
            Modulo,
            Igual,

            // Simbolos
            PuntoYComa,
            DosPuntos,
            ParentesisIzquierdo,
            ParentesisDerecho,
            CorcheteIzquierdo,
            CorcheteRerecho,
            Punto,
            Coma,
            Asignacion,

            // Literales
            Numero,
            String,
            Identificador,
            Comentario,

            // Palabras clave
            Program,
            Begin,
            End,
            Type,
            Var,
            Procedure,
            Function,
            If,
            Then,
            Else,
            While,
            Do,
            Array,
            Of,
            Integer,
            Boolean,
            True,
            False,


            Error
        }

        private static readonly Dictionary<string, TokenType> PalabrasClave = new Dictionary<string, TokenType>
        {
            { "program", TokenType.Program },
            {"string",TokenType.String },
            { "begin", TokenType.Begin },
            { "end", TokenType.End },
            { "type", TokenType.Type },
            { "var", TokenType.Var },
            { "procedure", TokenType.Procedure },
            { "function", TokenType.Function },
            { "if", TokenType.If },
            { "then", TokenType.Then },
            { "else", TokenType.Else },
            { "while", TokenType.While },
            { "do", TokenType.Do },
            { "array", TokenType.Array },
            { "of", TokenType.Of },
            { "integer", TokenType.Integer },
            { "boolean", TokenType.Boolean },
            { "true", TokenType.True },
            { "false", TokenType.False }
        };
        public List<Token> Analizar(string expresion)
        {
            var lista = new List<Token>();
            int i = 0;

            while (i < expresion.Length)
            {
                if (char.IsWhiteSpace(expresion[i]) || expresion[i] == '\n')
                {
                    i++;
                    continue;
                }

                // Comentarios
                if (i + 1 < expresion.Length && expresion[i] == '/' && expresion[i + 1] == '/')
                {
                    var comentario = LeerComentario(expresion, i);
                    if (comentario != null)
                    {
                        lista.Add(comentario.Value.token);
                        i = comentario.Value.nuevaPos;
                        continue;
                    }
                    else
                    {
                        lista.Add(new Token(TokenType.Error, "Comentario sin cerrar", i));
                        break;
                    }
                }

                // Identificadores y palabras clave
                if (char.IsLetter(expresion[i]))
                {
                    var identificador = LeerIdentificador(expresion, i);
                    lista.Add(identificador.token);
                    i = identificador.nuevaPos;
                    continue;
                }

                // Números
                if (char.IsDigit(expresion[i]))
                {
                    var numero = LeerNumero(expresion, i);
                    lista.Add(numero.token);
                    i = numero.nuevaPos;
                    continue;
                }

                // Strings
                if (expresion[i] == '"')
                {
                    var texto = LeerString(expresion, i);
                    if (texto != null)
                    {
                        lista.Add(texto.Value.token);
                        i = texto.Value.nuevaPos;
                        continue;
                    }
                    else
                    {
                        lista.Add(new Token(TokenType.Error, "String sin cerrar", i));
                        break;
                    }
                }

                // Operadores y símbolos
                char c = expresion[i];
                switch (c)
                {
                    case '+':
                        lista.Add(new Token(TokenType.Suma, "+", i));
                        break;
                    case '=':
                        lista.Add(new Token(TokenType.Igual, "=", i));
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
                    case '%':
                        lista.Add(new Token(TokenType.Modulo, "%", i));
                        break;
                    case ';':
                        lista.Add(new Token(TokenType.PuntoYComa, ";", i));
                        break;
                    case ':':
                        if (i + 1 < expresion.Length && expresion[i + 1] == '=')
                        {
                            lista.Add(new Token(TokenType.Asignacion, ":=", i));
                            i++;
                        }
                        else
                        {
                            lista.Add(new Token(TokenType.DosPuntos, ":", i));
                        }
                        break;
                    case '(':
                        lista.Add(new Token(TokenType.ParentesisIzquierdo, "(", i));
                        break;
                    case ')':
                        lista.Add(new Token(TokenType.ParentesisDerecho, ")", i));
                        break;
                    case '[':
                        lista.Add(new Token(TokenType.CorcheteIzquierdo, "[", i));
                        break;
                    case ']':
                        lista.Add(new Token(TokenType.CorcheteRerecho, "]", i));
                        break;
                    case '.':
                        lista.Add(new Token(TokenType.Punto, ".", i));
                        break;
                    case ',':
                        lista.Add(new Token(TokenType.Coma, ",", i));
                        break;
                    default:
                        lista.Add(new Token(TokenType.Error, $"Caracter no reconocido: {c}", i));
                        return lista;
                }
                i++;
            }

            return lista;
        }

        private (Token token, int nuevaPos)? LeerComentario(string texto, int i)
        {
            int inicio = i;
            int buscarDesde = i + 2;
            int cierre = texto.IndexOf("//", buscarDesde);

            if (cierre == -1)
            {

                return null;

            }
            else
            {
                string comentario = texto.Substring(inicio, cierre + 2 - inicio);
                return (new Token(TokenType.Comentario, comentario, inicio), cierre + 2);
            }
        }
        private (Token token, int nuevaPos) LeerIdentificador(string texto, int i)
        {
            int inicio = i;
            string lexema = "";

            while (i < texto.Length && char.IsLetterOrDigit(texto[i]))
            {
                lexema += texto[i];
                i++;
            }

            //  palabra clave
            string lexemaLower = lexema.ToLower();
            if (PalabrasClave.ContainsKey(lexemaLower))
            {
                return (new Token(PalabrasClave[lexemaLower], lexema, inicio), i);
            }
            else
            {
                return (new Token(TokenType.Identificador, lexema, inicio), i);
            }
        }

        private (Token token, int nuevaPos) LeerNumero(string texto, int i)
        {
            int inicio = i;
            string numero = "";

            while (i < texto.Length && char.IsDigit(texto[i]))
            {
                numero += texto[i];
                i++;
            }

            if (i < texto.Length && texto[i] == '.')
            {
                numero += '.';
                i++;
                while (i < texto.Length && char.IsDigit(texto[i]))
                {
                    numero += texto[i];
                    i++;
                }
            }

            if (i < texto.Length && (texto[i] == 'e' || texto[i] == 'E'))
            {
                numero += texto[i];
                i++;
                if (i < texto.Length && (texto[i] == '+' || texto[i] == '-'))
                {
                    numero += texto[i];
                    i++;
                }
                while (i < texto.Length && char.IsDigit(texto[i]))
                {
                    numero += texto[i];
                    i++;
                }
            }

            return (new Token(TokenType.Numero, numero, inicio), i);
        }


        private (Token token, int nuevaPos)? LeerString(string texto, int i)
        {
            int inicio = i;
            i++; 
            string valor = "";

            while (i < texto.Length && texto[i] != '"')
            {
                valor += texto[i];
                i++;
            }

            if (i >= texto.Length)
            {
                return null;
            }

            i++; 
            return (new Token(TokenType.String, valor, inicio), i);
        }
    }
}
