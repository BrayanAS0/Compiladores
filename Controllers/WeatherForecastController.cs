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

        public ActionResult Analizar(string expresion)
        {
            var tokens = new List<string>();
            var tokenTypes = new List<string>();
            var position = new List<int>();

            int i = 0;
            while (i < expresion.Length)
            {
                char c = expresion[i];
                switch (c)
                {
                    case '+':
                        tokens.Add("+");
                        tokenTypes.Add("Suma");
                        position.Add(i);
                        i++;
                        break;
                    case '*':
                        tokens.Add("*");
                        tokenTypes.Add("Multiplicacion");
                        position.Add(i);
                        i++;
                        break;
                    case ';':
                        tokens.Add(";");
                        tokenTypes.Add("punto y coma");
                        position.Add(i);
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
                            tokenTypes.Add("Numero");
                            position.Add(i);
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
            return  new JsonResult ( new {
                 tokens, 
            position,
            tokenTypes
            
            });
        }



    }
}
