using System;

namespace Zenvia.Api.Models.Responses
{
    /// <summary>
    /// Classe que representa uma mensagem recebida pelo serviço Zenvia.
    /// </summary>
    public class ReceivedMessage
    {
        /// <summary>
        /// Índice único informado pelo serviço Zenvia.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Código único de referência de uma mensagem enviada informado pelo remetente.
        /// </summary>
        public string MtId { get; set; }

        /// <summary>
        /// Data em que a mensagem foi recebida.
        /// </summary>
        public DateTime DateReceived { get; set; }

        /// <summary>
        /// Número do remetente da mensagem.
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Corpo da mensagem.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Número do destinatário da mensagem.
        /// </summary>
        public string Shortcode { get; set; }

        /// <summary>
        /// Nome da operadora telefônica do destinatário.
        /// </summary>
        public string MobileOperatorName { get; set; }
        
        public override string ToString()
        {
            return String.Format("receivedMessage: [id:{0}], [dateReceived:{1}], [mobile:{2}], [body:{3}], [shortCode:{4}], [mobileOperatorName:{5}], [mtId:{6}]", this.Id, this.DateReceived, this.Mobile, this.Body, this.Shortcode, this.MobileOperatorName, this.MtId);
        }
    }
}
