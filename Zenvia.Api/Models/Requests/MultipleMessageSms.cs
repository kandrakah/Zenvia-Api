using Newtonsoft.Json;
using System.Collections.Generic;

namespace Zenvia.Api.Models.Requests
{
    /// <summary>
    /// 
    /// </summary>
    public class MultipleMessageSms : AbstractMessageSms
    {
        public int AggregateId { get; set; }

        [JsonProperty(PropertyName = "sendSmsRequestList")]
        public List<MessageSmsElement> Messages { get; }

        public MultipleMessageSms()
        {
            this.Messages = new List<MessageSmsElement>();
        }
    }
}
