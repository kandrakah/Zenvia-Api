namespace Zenvia.Api.Models.Enumerators
{
    /// <summary>
    /// Enumerador que indica o tipo de execução da API Zenvia.
    /// </summary>
    public enum RuntimeMode
    {
        /// <summary>
        /// Modo de produção. O serviço é consumido e as mensagens são efetivamente enviadas à seus destinatários.
        /// </summary>
        PRODUCTION,

        /// <summary>
        /// Modo de desenvolvimento. O serviço envia e recebe requisições, más não envia efetivamente as mensagens ao destinatário.
        /// Utilizado para testes de API.
        /// </summary>
        MOCK_SERVER,

        /// <summary>
        /// Modo de desenvolvimento.
        /// </summary>
        PROXY_DEBUG
    }
}
