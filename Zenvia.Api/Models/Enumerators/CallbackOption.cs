namespace Zenvia.Api.Models.Enumerators
{
    /// <summary>
    /// Enumerador que define os tipos de respostas às requisições de envio de SMS.
    /// </summary>
    public enum CallbackOption
    {
        /// <summary>
        /// Nenhuma resposta é esperada.
        /// </summary>
        NONE,

        /// <summary>
        /// Somente a resposta final é esperada.
        /// </summary>
        FINAL,

        /// <summary>
        /// Todas as respostas são esperadas.
        /// </summary>
        ALL
    }
}
