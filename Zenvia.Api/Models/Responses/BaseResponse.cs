using Newtonsoft.Json;
using System;

namespace Zenvia.Api.Models.Responses
{
    /// <summary>
    /// Classe que representa a base de vários tipos de resposta providos pelo serviço Zenvia.
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// Código de estado informado pelo serviço Zenvia.
        /// </summary>
        [JsonProperty("statusCode")]
        public string StatusCode { get; internal set; }

        /// <summary>
        /// Descrição do código de estado informado pelo serviço Zenvia.
        /// </summary>
        [JsonProperty("statusDescription")]
        public string StatusDescription { get; internal set; }

        /// <summary>
        /// Código de detalhe informado pelo serviço Zenvia.
        /// </summary>
        [JsonProperty("detailCode")]
        public string DetailCode { get; internal set; }

        /// <summary>
        /// Descrição do código de detalhe provido pelo serviço Zenvia.
        /// </summary>
        [JsonProperty("detailDescription")]
        public string DetailDescription { get; internal set; }
        
        public override string ToString()
        {
            return String.Format("response: [statusCode:{0}], [statusDescription:{1}], [detailCode:{2}], [detailDescription:{3}]", this.StatusCode, this.StatusDescription, this.DetailCode, this.DetailDescription);
        }
    }
}
