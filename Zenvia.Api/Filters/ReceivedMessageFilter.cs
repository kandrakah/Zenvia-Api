using System;

namespace Zenvia.Api.Filters
{
    /// <summary>
    /// Classe que representa o filtro de busca de mensagens recebidas.
    /// </summary>
    public class ReceivedMessageFilter
    {
        /// <summary>
        /// Data inicial do periodo de busca.
        /// </summary>
        public DateTime Start { get; private set; }

        /// <summary>
        /// Data final do periodo de busca.
        /// </summary>
        public DateTime End { get; private set; }

        /// <summary>
        /// Código de referência da mensagem.
        /// </summary>
        public string ReferenceMessageId { get; private set; }

        /// <summary>
        /// Número telefônico do destinatário.
        /// </summary>
        public string Mobile { get; private set; }
        
        /// <summary>
        /// Método Construtor.
        /// </summary>
        /// <param name="builder">Construtor da classe.</param>
        private ReceivedMessageFilter(Builder builder)
        {
            this.Start = builder.Start;
            this.End = builder.End;
            this.ReferenceMessageId = builder.ReferenceMessageId;
            this.Mobile = builder.Mobile;
        }

        public bool DefinesReferenceMessageId { get { return !String.IsNullOrEmpty(this.ReferenceMessageId); } }

        public bool DefinesMobile { get { return !String.IsNullOrEmpty(this.Mobile); } }

        public class Builder
        {
            public DateTime Start { get; private set; }
            public DateTime End { get; private set; }
            public String ReferenceMessageId { get; private set; }
            public String Mobile { get; private set; }

            public Builder(DateTime start, DateTime end)
            {
                if (start == null || end == null || this.Start > this.End)
                {
                    throw new ArgumentException("A data inicial deve ser anterior à data final!");
                }
                this.Start = start;
                this.End = end;
            }

            public ReceivedMessageFilter Build()
            {
                return new ReceivedMessageFilter(this);
            }

            public Builder Starting(DateTime start)
            {
                if (start == null || this.End != null && this.Start > this.End)
                {
                    throw new ArgumentException("A data inicial deve ser anterior à data final!");
                }
                this.Start = start;
                return this;
            }

            public Builder Ending(DateTime end)
            {
                if (end == null || this.Start != null && end < this.Start)
                {
                    throw new ArgumentException("A data final deve ser posterior à data final!");
                }
                this.End = end;
                return this;
            }

            public Builder ByReferenceMessageId(String referenceMessageId)
            {
                this.ReferenceMessageId = referenceMessageId;
                return this;
            }

            public Builder ByMobile(String mobile)
            {
                this.Mobile = mobile;
                return this;
            }
        }
    }
}
