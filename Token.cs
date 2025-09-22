
using static Compliadores_Form.Compiladores;

namespace Compliadores_Form
{
    public class Token
    {
        public TokenType Tipo { get; set; }
        public string Lexema { get; set; }
        public int Posicion { get; set; }

        public Token(TokenType tipo, string lexema, int posicion)
        {
            Tipo = tipo;
            Lexema = lexema;
            Posicion = posicion;
        }
    }
}
