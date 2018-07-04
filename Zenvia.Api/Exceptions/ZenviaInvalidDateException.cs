using System;

namespace Zenvia.Api.Exceptions
{
    /// <summary>
    /// Classe de excessão específica quando uma data é nula ou vazia.
    /// </summary>
    public class ZenviaInvalidDateException : ZenviaException
    {
        /// <summary>
        /// Método construtor.
        /// </summary>
        public ZenviaInvalidDateException() : base() { }

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="message">Mensagem de erro.</param>
        public ZenviaInvalidDateException(string message) : base(message) { }

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="innerException">Causa da falha.</param>
        public ZenviaInvalidDateException(Exception innerException) : base(string.Empty, innerException) { }

        /// <summary>
        /// Método construtor
        /// </summary>
        /// <param name="message">Mensagem de erro.</param>
        /// <param name="innerException">Causa da falha.</param>
        public ZenviaInvalidDateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
