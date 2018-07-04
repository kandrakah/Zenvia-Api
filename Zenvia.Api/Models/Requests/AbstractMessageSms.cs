using System;
using Zenvia.Api.Models.Enumerators;

namespace Zenvia.Api.Models.Requests
{
    public abstract class AbstractMessageSms : BaseObject
    {
        public string Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Schedule { get; set; }
        public string Msg { get; set; }
        public CallbackOption CallbackOption { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int TimeToLive { get; set; }

        protected AbstractMessageSms()
        {
            this.CallbackOption = CallbackOption.NONE;
        }
    }
}
