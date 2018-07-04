using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zenvia.Api.Models.Responses
{
    /// <summary>
    /// Classe que representa a resposta à requisição de busca por mensagens recebidas.
    /// </summary>
    public class ReceivedMessageResponse : BaseResponse
    {
        /// <summary>
        /// Lista de mensagens recebidas.
        /// </summary>
        public List<ReceivedMessage> ReceivedMessages { get; }

        /// <summary>
        /// Condição se há mensagens recebidas.
        /// </summary>
        public bool HasMessages { get { return this.ReceivedMessages.Any(); } }

        /// <summary>
        /// Método Construtor.
        /// </summary>
        public ReceivedMessageResponse()
        {
            this.ReceivedMessages = new List<ReceivedMessage>();
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());

            if (HasMessages)
            {
                sb.Append(", [messages");
                {
                    foreach(ReceivedMessage msg in ReceivedMessages)
                    {
                        sb.Append("\n");
                        sb.Append("[");
                        sb.Append(msg);
                        sb.Append("]");
                    }
                }
            }
            return sb.ToString();
        }
    }
}
