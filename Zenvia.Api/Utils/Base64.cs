using System;
using System.Text;

namespace Zenvia.Api.Utils
{
    /// <summary>
    /// Classe que simplifica a codificação e descodificação em Base64.
    /// </summary>
    public class Base64
    {
        /// <summary>
        /// Método que codifica um texto em Base64.
        /// </summary>
        /// <param name="texto">Texto que será codificado.</param>
        /// <returns>Texto codificado em Base64.</returns>
        public static string Encode(string texto)
        {
            var valor = Encoding.UTF8.GetBytes(texto);
            return Convert.ToBase64String(valor);
        }

        /// <summary>
        /// Método que descodifica um texto em Base64.
        /// </summary>
        /// <param name="texto">Texto que será descodificado.</param>
        /// <returns>Texto descodificado.</returns>
        public static string Decode(string texto)
        {
            try
            {
                var valor = Convert.FromBase64String(texto);
                return Encoding.UTF8.GetString(valor);
            }
            catch
            {
                return string.Empty;
            }

        }
    }
}
