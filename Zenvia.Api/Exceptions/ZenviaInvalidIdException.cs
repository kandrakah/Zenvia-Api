using System;

namespace Zenvia.Api.Exceptions
{
    /// <summary>
    /// Classe de excessão específica quando o Id é nulo ou vazio.
    /// </summary>
    public class ZenviaInvalidIdException : ZenviaException
    {
        /// <summary>
        /// Método construtor.
        /// </summary>
        public ZenviaInvalidIdException() : base() { }

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="message">Mensagem de erro.</param>
        public ZenviaInvalidIdException(string message) : base(message) { }

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="innerException">Causa da falha.</param>
        public ZenviaInvalidIdException(Exception innerException) : base(string.Empty, innerException) { }

        /// <summary>
        /// Método construtor
        /// </summary>
        /// <param name="message">Mensagem de erro.</param>
        /// <param name="innerException">Causa da falha.</param>
        public ZenviaInvalidIdException(string message, Exception innerException) : base(message, innerException) { }
    }
}
