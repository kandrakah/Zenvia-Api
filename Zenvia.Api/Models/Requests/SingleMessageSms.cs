using Newtonsoft.Json;

namespace Zenvia.Api.Models.Requests
{
    public class SingleMessageSms : AbstractMessageSms
    {
        [JsonProperty(PropertyName = "aggregateId")]
        public int AggregatorId { get; set; }

        public SingleMessageSms FromJson(string json)
        {
            return FromJson<SingleMessageSms>(json);
        }
    }
}
