
using Compliadores_Form;

public class Token
{
    public Compiladores.TokenType Tipo { get; }
    public string Lexema { get; }
    public int Posicion { get; }
    public Token(Compiladores.TokenType tipo, string lexema, int posicion)
    {
        Tipo = tipo;
        Lexema = lexema;
        Posicion = posicion;
    }
    public override string ToString()
    {
        return $"Tipo: {Tipo}, Lexema: '{Lexema}', Posición: {Posicion}";
    }
}