using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Compliadores_Form
{


    public class Compiladores
    {
        public enum TokenType
        {
            Suma, Resta, Multiplicacion, Division, Modulo,
            Igual,
            PuntoYComa, DosPuntos, ParentesisIzquierdo, ParentesisDerecho,
            CorcheteIzquierdo, CorcheteRerecho, Punto, Coma, Asignacion,
            Or,
            And,
            EqualEqual,
            NotEqual,
            Less,
            LessEqual,
            Greater,
            GreaterEqual,
            Numero, String, Identificador, Comentario,
            Program, Begin, End, Type, Var, Procedure, Function,
            If, Then, Else, While, Do, Array, Of, Integer, Boolean,
            True, False,
            EOF,
            Error
        }

        private string expresion;
        private int i;
        private DataGridView dgv;

        public class Parser
        {
            private Compiladores scanner;
            private Token tokenActual;

            public Parser(Compiladores scanner)
            {
                this.scanner = scanner;
                this.tokenActual = scanner.GetNextToken();
            }

            private TokenType peek()
            {
                return tokenActual.Tipo;
            }

            private void match(TokenType tipoEsperado)
            {
                if (tokenActual.Tipo == tipoEsperado)
                {
                    this.tokenActual = scanner.GetNextToken();
                }
                else
                {
                    string mensaje = $"Se esperaba '{tipoEsperado}' pero se encontró '{tokenActual.Tipo}'";
                    throw new Exception($"{tokenActual.Posicion}|{mensaje}");
                }
            }

            public void ParseProgram()
            {
                match(TokenType.Program); match(TokenType.Identificador); match(TokenType.PuntoYComa);
                ParseDeclSopt(); match(TokenType.Begin); ParseStmtList();
                match(TokenType.End); match(TokenType.Punto); match(TokenType.EOF);
            }
            private void ParseDeclSopt()
            {
                if (peek() == TokenType.Type || peek() == TokenType.Var ||
                  peek() == TokenType.Procedure || peek() == TokenType.Function) { ParseDeclS(); }
                else if (peek() == TokenType.Begin) { }
                else { throw new Exception($"{tokenActual.Posicion}|Se esperaba 'type', 'var', 'procedure', 'function' o 'begin'"); }
            }
            private void ParseDeclS()

            { 

                ParseDecl();

                while (peek() == TokenType.Type || peek() == TokenType.Var ||

                       peek() == TokenType.Procedure || peek() == TokenType.Function) { ParseDecl(); }

            }
            private void ParseDecl()
            {
                if (peek() == TokenType.Type) { ParseTypeSection(); }
                else if (peek() == TokenType.Var) { ParseVarSection(); }
                else if (peek() == TokenType.Procedure || peek() == TokenType.Function) { ParseSubprogDecl(); }
                else { throw new Exception($"{tokenActual.Posicion}|Se esperaba una declaración (type, var, procedure, function)."); }
            }
            private void ParseTypeSection() { match(TokenType.Type); ParseTypeDefList(); }
            private void ParseTypeDefList()
            {
                ParseTypeDef();
                while (peek() == TokenType.PuntoYComa)
                {
                    match(TokenType.PuntoYComa);
                    if (peek() == TokenType.Identificador) { ParseTypeDef(); }
                    else { break; }
                }
            }
            private void ParseTypeDef() { match(TokenType.Identificador); match(TokenType.Igual); ParseType(); }
            private void ParseVarSection() { match(TokenType.Var); ParseVarDefList(); }
            private void ParseVarDefList()
            {
                ParseVarDef();
                while (peek() == TokenType.PuntoYComa)
                {
                    match(TokenType.PuntoYComa);
                    if (peek() == TokenType.Identificador) { ParseVarDef(); }
                    else { break; }
                }
            }
            private void ParseVarDef() { ParseIdList(); match(TokenType.DosPuntos); ParseType(); }
            private void ParseIdList()
            {
                match(TokenType.Identificador);
                while (peek() == TokenType.Coma)
                {
                    match(TokenType.Coma);
                    match(TokenType.Identificador);
                }
            }
            private void ParseSubprogDecl()
            {
                if (peek() == TokenType.Procedure) { ParseProcedureDecl(); }
                else if (peek() == TokenType.Function) { ParseFunctionDecl(); }
                else { throw new Exception($"{tokenActual.Posicion}|Se esperaba 'procedure' o 'function'."); }
            }

            private void ParseStmtList()
            {
                if (peek() == TokenType.End) { return; }
                ParseStmt();
                while (peek() == TokenType.PuntoYComa)
                {
                    match(TokenType.PuntoYComa);
                    if (peek() == TokenType.End || peek() == TokenType.Else) { break; }
                    ParseStmt();
                }
            }
            private void ParseStmt()
            {
                if (peek() == TokenType.If) { ParseIfStmt(); }
                else if (peek() == TokenType.While) { ParseWhileStmt(); }
                else if (peek() == TokenType.Identificador)
                {
                    match(TokenType.Identificador);
                    if (peek() == TokenType.ParentesisIzquierdo) { ParseCallStmt_Tail(); }
                    else { ParseAssignmentStmt_Tail(); }
                }
                else if (peek() != TokenType.End && peek() != TokenType.Else)
                {
                    throw new Exception($"{tokenActual.Posicion}|Se esperaba una sentencia (ID, if, while) o el fin de bloque (end, else)");
                }
            }
            private void ParseCallStmt_Tail()
            {
                match(TokenType.ParentesisIzquierdo);
                ParseArgListOpt();
                match(TokenType.ParentesisDerecho);
            }
            private void ParseAssignmentStmt_Tail()
            {
                ParseVariableTail();
                match(TokenType.Asignacion);
                ParseExpr();
            }
            private void ParseVariableTail()
            {
                if (peek() == TokenType.CorcheteIzquierdo)
                {
                    match(TokenType.CorcheteIzquierdo);
                    ParseExpr();
                    match(TokenType.CorcheteRerecho);
                    ParseVariableTail();
                }
            }
            private void ParseIfStmt()
            {
                match(TokenType.If); ParseExpr();
                match(TokenType.Then); ParseStmt();
                ParseElseOpt();
            }
            private void ParseElseOpt()
            {
                if (peek() == TokenType.Else)
                {
                    match(TokenType.Else);
                    ParseStmt();
                }
            }
            private void ParseWhileStmt()
            {
                match(TokenType.While); ParseExpr();
                match(TokenType.Do); ParseStmt();
            }
            private void ParseType()
            {
                if (peek() == TokenType.Integer) { match(TokenType.Integer); }
                else if (peek() == TokenType.Boolean) { match(TokenType.Boolean); }
                else if (peek() == TokenType.Identificador) { match(TokenType.Identificador); }
                else if (peek() == TokenType.Array)
                {
                    match(TokenType.Array);
                    Console.WriteLine("ADVERTENCIA: ParseType() solo acepta 'array' simple. Faltan [..] of ..");
                }
                else { throw new Exception($"{tokenActual.Posicion}|Se esperaba un tipo (Integer, Boolean, ID o Array)."); }
            }

            private void ParseProcedureDecl()
            {
                match(TokenType.Procedure);
                match(TokenType.Identificador);
                match(TokenType.PuntoYComa);
            }


            private void ParseFunctionDecl()
    
         {
                match(TokenType.Function);
            match(TokenType.Identificador);
            match(TokenType.PuntoYComa);
        }


        private void ParseExpr()
        {
            ParseOr();
        }


        private void ParseOr()
        {
            ParseAnd();
            ParseOrTail();
        }


        private void ParseOrTail()
        {
            if (peek() == TokenType.Or)
            {
                match(TokenType.Or);
                ParseAnd();
                ParseOrTail();
            }
        }


        private void ParseAnd()
        {
            ParseEq();
            ParseAndTail();
        }


        private void ParseAndTail()
        {
            if (peek() == TokenType.And)
            {
                match(TokenType.And);
                ParseEq();
                ParseAndTail();
            }
        }


        private void ParseEq()
        {
            ParseRel();
            ParseEqTail();
        }


        private void ParseEqTail()
        {
            if (peek() == TokenType.EqualEqual)
            {
                match(TokenType.EqualEqual);
                ParseRel();
                ParseEqTail();
            }
            else if (peek() == TokenType.NotEqual)
            {
                match(TokenType.NotEqual);
                ParseRel();
                ParseEqTail();
                        }
        }


        private void ParseRel()
        {
            ParseAdd();
            ParseRelTail();
        }


        private void ParseRelTail()
        {
            if (peek() == TokenType.Less)
            {
                match(TokenType.Less);
                ParseAdd();
                ParseRelTail();
            }
            else if (peek() == TokenType.LessEqual)
            {
                match(TokenType.LessEqual);
                ParseAdd();
                ParseRelTail();
            }
            else if (peek() == TokenType.Greater)
            {
                match(TokenType.Greater);
                ParseAdd();
                ParseRelTail();
            }
            else if (peek() == TokenType.GreaterEqual)
            {
                match(TokenType.GreaterEqual);
                ParseAdd();
                ParseRelTail();
            }
        }


        private void ParseAdd()
        {
            ParseMul();
            ParseAddTail();
        }


        private void ParseAddTail()
        {
            if (peek() == TokenType.Suma)
            {
                match(TokenType.Suma);
                ParseMul();
                ParseAddTail();
            }
            else if (peek() == TokenType.Resta)
            {
                match(TokenType.Resta);
                ParseMul();
                ParseAddTail();
                   }
        }



        private void ParseMul()
        {
            ParseUnary(); ParseMulTail();
        }
        private void ParseMulTail()
        {
            if (peek() == TokenType.Multiplicacion)
            {
                match(TokenType.Multiplicacion); ParseUnary(); ParseMulTail();
            }
            else if (peek() == TokenType.Division)
            {
                match(TokenType.Division); ParseUnary(); ParseMulTail();
            }
            else if (peek() == TokenType.Modulo)
            {
                match(TokenType.Modulo); ParseUnary(); ParseMulTail();
            }
        }
        private void ParseUnary()
        {
            if (peek() == TokenType.Resta)
            {
                match(TokenType.Resta); ParseUnary();
            }
            else { ParsePostfix(); }
        }
        private void ParsePostfix()
        {
            ParsePrimary(); ParsePostfixTail();
        }
        private void ParsePostfixTail()
        {
            if (peek() == TokenType.ParentesisIzquierdo)
            {
                match(TokenType.ParentesisIzquierdo); ParseArgListOpt();
                match(TokenType.ParentesisDerecho); ParsePostfixTail();
            }
            else if (peek() == TokenType.CorcheteIzquierdo)
            {
                match(TokenType.CorcheteIzquierdo); ParseExpr();
                match(TokenType.CorcheteRerecho); ParsePostfixTail();
            }
            else if (peek() == TokenType.Punto)
                    {
                match(TokenType.Punto); match(TokenType.Identificador);
                ParsePostfixTail();
            }
             }
        private void ParsePrimary()
        {
            if (peek() == TokenType.Identificador) { match(TokenType.Identificador); }
            else if (peek() == TokenType.Numero) { match(TokenType.Numero); }
            else if (peek() == TokenType.String) { match(TokenType.String); }
            else if (peek() == TokenType.True) { match(TokenType.True); }
            else if (peek() == TokenType.False) { match(TokenType.False); }
            else if (peek() == TokenType.ParentesisIzquierdo)
            {
                match(TokenType.ParentesisIzquierdo);
               ParseExpr();
                match(TokenType.ParentesisDerecho);
            }
            else
            {
                throw new Exception($"{tokenActual.Posicion}|Error Primario: Se esperaba ID, Numero, String, true, false o '('.");
            }
        }
        private void ParseArgListOpt()
        {
            if (peek() == TokenType.Identificador || peek() == TokenType.Numero ||
              peek() == TokenType.String || peek() == TokenType.True ||
              peek() == TokenType.False || peek() == TokenType.ParentesisIzquierdo ||
              peek() == TokenType.Resta)
            {
                ParseArgList();
            }
        }
        private void ParseArgList()
        {
            ParseExpr(); ParseArgListTail();
        }
        private void ParseArgListTail()
        {
            if (peek() == TokenType.Coma)
            {
                match(TokenType.Coma);
           ParseExpr();
                ParseArgListTail();
                }
        }

        } 


        
        private static readonly Dictionary<string, TokenType> PalabrasClave = new Dictionary<string, TokenType>
    {
      { "program", TokenType.Program }, {"string",TokenType.String }, { "begin", TokenType.Begin },
      { "end", TokenType.End }, { "type", TokenType.Type }, { "var", TokenType.Var },
      { "procedure", TokenType.Procedure }, { "function", TokenType.Function }, { "if", TokenType.If },
      { "then", TokenType.Then }, { "else", TokenType.Else }, { "while", TokenType.While },
      { "do", TokenType.Do }, { "array", TokenType.Array }, { "of", TokenType.Of },
      { "integer", TokenType.Integer }, { "boolean", TokenType.Boolean }, { "true", TokenType.True },
      { "false", TokenType.False }
    };


        public Token GetNextToken()
        {
            while (i < expresion.Length)
            {
                if (char.IsWhiteSpace(expresion[i]) || expresion[i] == '\n') { i++; continue; }


                if (i + 1 < expresion.Length && expresion[i] == '/' && expresion[i + 1] == '/')
                {
                    var comentario = LeerComentario(expresion, i);
                    Token tokenCom;
                    if (comentario != null)
                    {
                        tokenCom = comentario.Value.token; i = comentario.Value.nuevaPos;
                    }
                    else
                    {
                        tokenCom = new Token(TokenType.Error, "Comentario sin cerrar", i); i = expresion.Length;
                                 }
                    dgv.Rows.Add(tokenCom.Posicion, tokenCom.Lexema, tokenCom.Tipo);
                    if (tokenCom.Tipo == TokenType.Error) return tokenCom;
                    continue;
                }


                if (char.IsLetter(expresion[i]))
                {
                    var (token, nuevaPos) = LeerIdentificador(expresion, i);
                    i = nuevaPos;
                    dgv.Rows.Add(token.Posicion, token.Lexema, token.Tipo);
                    return token;
                }


                if (char.IsDigit(expresion[i]))
                {
                    var (token, nuevaPos) = LeerNumero(expresion, i);
           i = nuevaPos;
                    dgv.Rows.Add(token.Posicion, token.Lexema, token.Tipo);
                    return token;
                }


                if (expresion[i] == '"')
                {
                    var texto = LeerString(expresion, i);
                    Token tokenStr;
                    if (texto != null)
                              {
                        tokenStr = texto.Value.token; i = texto.Value.nuevaPos;
                    }

          else
                    {
                        tokenStr = new Token(TokenType.Error, "String sin cerrar", i); i = expresion.Length;
                    }
                    dgv.Rows.Add(tokenStr.Posicion, tokenStr.Lexema, tokenStr.Tipo);
                    return tokenStr;
                }


                char c = expresion[i];
                Token tokenSimbolo;
                int pos = i;
                i++;

                switch (c)
                {

                    case '+': tokenSimbolo = new Token(TokenType.Suma, "+", pos); break;
                    case '-': tokenSimbolo = new Token(TokenType.Resta, "-", pos); break;
                    case '*': tokenSimbolo = new Token(TokenType.Multiplicacion, "*", pos); break;
                    case '/': tokenSimbolo = new Token(TokenType.Division, "/", pos); break;
                    case '%': tokenSimbolo = new Token(TokenType.Modulo, "%", pos); break;
                    case ';': tokenSimbolo = new Token(TokenType.PuntoYComa, ";", pos); break;
                    case '(': tokenSimbolo = new Token(TokenType.ParentesisIzquierdo, "(", pos); break;
                    case ')': tokenSimbolo = new Token(TokenType.ParentesisDerecho, ")", pos); break;
                    case '[': tokenSimbolo = new Token(TokenType.CorcheteIzquierdo, "[", pos); break;
                    case ']': tokenSimbolo = new Token(TokenType.CorcheteRerecho, "]", pos); break;
                    case '.': tokenSimbolo = new Token(TokenType.Punto, ".", pos); break;
                    case ',': tokenSimbolo = new Token(TokenType.Coma, ",", pos); break;


                    case ':':
                        if (i < expresion.Length && expresion[i] == '=')
                        {
                            tokenSimbolo = new Token(TokenType.Asignacion, ":=", pos); i++;
                        }
                        else { tokenSimbolo = new Token(TokenType.DosPuntos, ":", pos); }
                        break;
                    case '=':
                        if (i < expresion.Length && expresion[i] == '=')
                                {
                            tokenSimbolo = new Token(TokenType.EqualEqual, "==", pos); i++;
                        }

            else { tokenSimbolo = new Token(TokenType.Igual, "=", pos); }
                        break;
                    case '!':
                        if (i < expresion.Length && expresion[i] == '=')
                        {
                            tokenSimbolo = new Token(TokenType.NotEqual, "!=", pos); i++;
                       }
                        else
                        {
                            tokenSimbolo = new Token(TokenType.Error, "Caracter no reconocido: ! (sólo '!=')", pos);
                        }
                        break;
                    case '<':
                        if (i < expresion.Length && expresion[i] == '=')
                        {
                            tokenSimbolo = new Token(TokenType.LessEqual, "<=", pos); i++;
                        }
                        else { tokenSimbolo = new Token(TokenType.Less, "<", pos); }
                            break;
                    case '>':
                        if (i < expresion.Length && expresion[i] == '=')
                        {
                                tokenSimbolo = new Token(TokenType.GreaterEqual, ">=", pos); i++;
                        }
                        else { tokenSimbolo = new Token(TokenType.Greater, ">", pos); }
                        break;
                    case '&':
                        if (i < expresion.Length && expresion[i] == '&')
                            {
                            tokenSimbolo = new Token(TokenType.And, "&&", pos); i++;
                        }

            else
                        {
                            tokenSimbolo = new Token(TokenType.Error, "Caracter no reconocido: & (sólo '&&')", pos);
                        }
                        break;
                    case '|':
                        if (i < expresion.Length && expresion[i] == '|')
                                   {
                            tokenSimbolo = new Token(TokenType.Or, "||", pos); i++;
                                 }

            else
                        {
                            tokenSimbolo = new Token(TokenType.Error, "Caracter no reconocido: | (sólo '||')", pos);
                        }
                        break;


                    default:
                        tokenSimbolo = new Token(TokenType.Error, $"Caracter no reconocido: {c}", pos);
                        i = expresion.Length;
                        dgv.Rows.Add(tokenSimbolo.Posicion, tokenSimbolo.Lexema, tokenSimbolo.Tipo);
                        return tokenSimbolo;
                }


                dgv.Rows.Add(tokenSimbolo.Posicion, tokenSimbolo.Lexema, tokenSimbolo.Tipo);
                return tokenSimbolo;
            }


            return new Token(TokenType.EOF, "EOF", i);
        }


        private (Token token, int nuevaPos)? LeerComentario(string texto, int i)
        {
            int inicio = i; int buscarDesde = i + 2; int cierre = texto.IndexOf("//", buscarDesde);
            if (cierre == -1) { return null; }
            else { string comentario = texto.Substring(inicio, cierre + 2 - inicio); return (new Token(TokenType.Comentario, comentario, inicio), cierre + 2); }
        }
        private (Token token, int nuevaPos) LeerIdentificador(string texto, int i)
        {
            int inicio = i; string lexema = "";
            while (i < texto.Length && char.IsLetterOrDigit(texto[i])) { lexema += texto[i]; i++; }
            string lexemaLower = lexema.ToLower();
            if (PalabrasClave.ContainsKey(lexemaLower)) { return (new Token(PalabrasClave[lexemaLower], lexema, inicio), i); }
            else { return (new Token(TokenType.Identificador, lexema, inicio), i); }
        }
        private (Token token, int nuevaPos) LeerNumero(string texto, int i)
        {
            int inicio = i; string numero = "";
            while (i < texto.Length && char.IsDigit(texto[i])) { numero += texto[i]; i++; }
          if (i < texto.Length && texto[i] == '.')
            {
                numero += '.'; i++;
                while (i < texto.Length && char.IsDigit(texto[i])) { numero += texto[i]; i++; }
            }
            if (i < texto.Length && (texto[i] == 'e' || texto[i] == 'E'))
             {
                numero += texto[i]; i++;
                if (i < texto.Length && (texto[i] == '+' || texto[i] == '-')) { numero += texto[i]; i++; }
                while (i < texto.Length && char.IsDigit(texto[i])) { numero += texto[i]; i++; }
            }
            return (new Token(TokenType.Numero, numero, inicio), i);
        }
        private (Token token, int nuevaPos)? LeerString(string texto, int i)
        {
            int inicio = i; i++; string valor = "";
            while (i < texto.Length && texto[i] != '"') { valor += texto[i]; i++; }
               if (i >= texto.Length) { return null; }
            i++; return (new Token(TokenType.String, valor, inicio), i);
        }



        public void AnalizarYMostrarEnTabla(string expresion, DataGridView dgv)
        {

            dgv.Rows.Clear();
            this.expresion = expresion;
            this.i = 0;
            this.dgv = dgv;

            try
            {
                Parser miParser = new Parser(this);
                miParser.ParseProgram();
                dgv.Rows.Add    (TokenType.EOF, "EOF", i);

            }
            catch (Exception exSintax)
            {
                string[] partesError = exSintax.Message.Split('|');
                string posicionError = "Error";
                string mensajeError = exSintax.Message;

                if (partesError.Length == 2)
                {
                    posicionError = partesError[0];
                     mensajeError = partesError[1];
                }

                dgv.Rows.Add(posicionError, "error", mensajeError);
            }
        }

    }
}