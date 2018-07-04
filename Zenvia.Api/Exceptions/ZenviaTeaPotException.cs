using System;

namespace Zenvia.Api.Exceptions
{
    /// <summary>
    /// Classe de excessão específica que indica um easter egg do serviço HTTP (418).
    /// </summary>
    public class ZenviaTeaPotException : ZenviaException
    {
        /// <summary>
        /// Método construtor.
        /// </summary>
        public ZenviaTeaPotException() : base() { }

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="message">Mensagem de erro.</param>
        public ZenviaTeaPotException(string message) : base(message) { }

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="innerException">Causa da falha.</param>
        public ZenviaTeaPotException(Exception innerException) : base(string.Empty, innerException) { }

        /// <summary>
        /// Método construtor
        /// </summary>
        /// <param name="message">Mensagem de erro.</param>
        /// <param name="innerException">Causa da falha.</param>
        public ZenviaTeaPotException(string message, Exception innerException) : base(message, innerException) { }
    }
}
