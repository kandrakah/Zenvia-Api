using Newtonsoft.Json;
using System;

namespace Zenvia.Api.Models.Responses
{
    /// <summary>
    /// Classe que representa a resposta à solicitação de estado de uma mensagem SMS.
    /// </summary>
    public class GetSmsStatusResponse : BaseResponse
    {
        /// <summary>
        /// Índice único informado pelo serviço Zenvia.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// Data de recebimento da mensagem SMS pelo destinatário.
        /// </summary>
        [JsonProperty("received")]
        public DateTime Received { get; internal set; }

        /// <summary>
        /// Número curto usado como referência telefônica.
        /// </summary>
        [JsonProperty("shortcode")]
        public string Shortcode { get; internal set; }

        /// <summary>
        /// Nome da operadora telefônica do destinatário.
        /// </summary>
        [JsonProperty("mobileOperatorName")]
        public string MobileOperatorName { get; internal set; }

        public override string ToString()
        {
            return String.Format("smsStatus: [id:{0}], [received:{1}], [shortCode:{2}], [mobileOperatorName:{3}], [statusCode:{4}], [statusDescription:{5}], [detailCode:{6}], [detailDescription:{7}]", this.Id, this.Received, this.Shortcode, this.MobileOperatorName, this.StatusCode, this.StatusDescription, this.DetailCode, this.DetailDescription);
        }
    }
}
