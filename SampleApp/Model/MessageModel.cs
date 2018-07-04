using System;
using Zenvia.Api.Models.Requests;
using Zenvia.Api.Models.Responses;

namespace SampleApp.Model
{
    public enum MessageType
    {
        Enviada,
        Recebida
    }

    public class MessageModel
    {
        public int Id { get; set; }

        public string MtId { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string Message { get; set; }

        public MessageType Type { get; set; }

        public MessageModel() { }

        public MessageModel(SingleMessageSms msg)
        {
            this.Sender = msg.From;
            this.Receiver = msg.To;
            this.Message = msg.Msg;
            this.Type = MessageType.Enviada;
        }

        public MessageModel(ReceivedMessage msg)
        {
            this.MtId = msg.MtId;
            this.Sender = msg.Mobile;
            this.Receiver = "Eu";
            this.Message = msg.Body;
            this.Type = MessageType.Recebida;
        }
    }
}
