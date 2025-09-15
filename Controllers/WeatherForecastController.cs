using Microsoft.AspNetCore.Mvc;

namespace compilador.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        public enum TokenType
        {
            SUMA,           // +
            MULT,           // *
            PUNTOYCOMA,     // ;
            PARENTESIS_IZQ, // (
            PARENTESIS_DER, // )
            NUMERO,         
            IDENTIFICADOR,  
            FIN,           
            DESCONOCIDO     
        }

        [HttpGet]

        public List<string> Analizar(string expresion)
        {
            var tokens = new List<string>();
            int i = 0;
            while (i < expresion.Length)
            {
                char c = expresion[i];
                switch (c)
                {
                    case '+':
                        tokens.Add("+");
                        i++;
                        break;
                    case '*':
                        tokens.Add("*");
                        i++;
                        break;
                    case ';':
                        tokens.Add(";");
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
                            while (i < expresion.Length && char.IsDigit(expresion[i]))
                            {
                                number += expresion[i];
                                i++;
                            }
                            tokens.Add(number);
                        }
                        else
                        {
                            //tokens.Add(TokenType.DESCONOCIDO);
                            i++;
                        }
                        break;
                }
            }
            return tokens;
        }



    }
}
