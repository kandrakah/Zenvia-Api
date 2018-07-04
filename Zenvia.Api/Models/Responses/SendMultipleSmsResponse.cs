using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace Zenvia.Api.Models.Responses
{
    /// <summary>
    /// Classe que representa a resposta à requisição de envio de várias mensagens SMS simultâneas.
    /// </summary>
    public class SendMultipleSmsResponse
    {
        /// <summary>
        /// Lista de respostas às mensagens enviadas.
        /// </summary>
        [JsonProperty(PropertyName = "sendSmsResponseList")]
        public List<SendSmsResponse> Responses { get; }

        /// <summary>
        /// Método Construtor.
        /// </summary>
        public SendMultipleSmsResponse()
        {
            this.Responses = new List<SendSmsResponse>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("multipleSmsRespose");

            foreach(SendSmsResponse resp in Responses)
            {
                sb.Append(resp.ToString());
            }

            return sb.ToString();
        }
    }
}
