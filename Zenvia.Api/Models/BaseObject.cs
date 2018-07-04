using Newtonsoft.Json;
using System;

namespace Zenvia.Api.Models
{
    public abstract class BaseObject
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        protected T FromJson<T>(String json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
