using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Zenvia.Api.Exceptions;
using Zenvia.Api.Filters;
using Zenvia.Api.Models;
using Zenvia.Api.Models.Enumerators;
using Zenvia.Api.Models.Requests;
using Zenvia.Api.Models.Responses;
using Zenvia.Api.Utils;

namespace Zenvia.Api
{
    /// <summary>
    /// Classe principal que faz a comunicação com a API do Zenvia, enviando as mensagens e recebendo suas respectivas respostas.
    /// </summary>
    public class ZenviaApi
    {
        /// <summary>
        /// Atributo que representa a URL básica do serviço da API Zenvia.
        /// </summary>
        private string BaseUrl;

        /// <summary>
        /// Atributo que representa a URL de envio de mensagens da API Zenvia.
        /// </summary>
        private string SendSmsUrl;

        /// <summary>
        /// Atributo que representa a URL de envio de mensagens múltiplas da API Zenvia.
        /// </summary>
        private string SendMultipleSmsUrl;

        /// <summary>
        /// Atributo que representa a URL cancelamento de mensagens da API Zenvia.
        /// </summary>
        private string CancelSmsUrl;

        /// <summary>
        /// Atributo que representa a URL requisição de status da API Zenvia.
        /// </summary>
        private string GetSmsStatusUrl;

        /// <summary>
        /// Atributo que representa a URL requisição de mensagens não lidas da API Zenvia.
        /// </summary>
        private string ListUnreadMessagesUrl;

        /// <summary>
        /// Atributo que representa a URL de busca mensagens da API Zenvia.
        /// </summary>
        private string SearchReceivedMessagesUrl;

        /// <summary>
        /// Atributo que representa o nome de usuário da API.
        /// </summary>
        public string Username { private get; set; }

        /// <summary>
        /// Atributo que representa a senha de usuário da API.
        /// </summary>
        public string Password { private get; set; }

        /// <summary>
        /// Atributo que determina o modo de operação da API.
        /// </summary>
        internal RuntimeMode RuntimeMode { get; set; }

        /// <summary>
        /// Método Construtor.
        /// </summary>
        /// <param name="runtimeMode">Modo de execução da API. Por padrão, é utilizado o modo de produção, 
        /// porém este exige e consome recursos do serviço. Nos modos voltados ao desenvolvimento, esses dados são irrelevantes e
        /// o serviço não é efetivamente consumido (Mensagens não são enviadas ao destinatário).</param>
        /// <param name="urlCode">Código de URL que altera-se de tempos em tempos nos modos de depuração e testes.</param>
        public ZenviaApi(RuntimeMode runtimeMode = RuntimeMode.PRODUCTION, string urlCode = "")
        {
            this.RuntimeMode = runtimeMode;
            this.BaseUrl = GetBaseUrl(urlCode);
            this.SendSmsUrl = this.BaseUrl + "/services/send-sms";
            this.SendMultipleSmsUrl = this.BaseUrl + "/services/send-sms-multiple";
            this.CancelSmsUrl = this.BaseUrl + "/services/cancel-sms";
            this.GetSmsStatusUrl = this.BaseUrl + "/services/get-sms-status";
            this.ListUnreadMessagesUrl = this.BaseUrl + "/services/received/list";
            this.SearchReceivedMessagesUrl = this.BaseUrl + "/services/received/search";
        }

        private string GetBaseUrl(string urlCode)
        {
            switch (this.RuntimeMode)
            {
                case RuntimeMode.MOCK_SERVER:
                    return Properties.Resources.ZENVIA_MOCK_API_URL.Replace("{URL_CODE}", urlCode);
                case RuntimeMode.PROXY_DEBUG:
                    return Properties.Resources.ZENVIA_DEBUG_PROXY_API_URL.Replace("{URL_CODE}", urlCode);
                default:
                    return Properties.Resources.ZENVIA_PRODUCTION_API_URL;
            }
        }

        /// <summary>
        /// Método que verifica se o nome de usuário e/ou a senha estão em branco.
        /// </summary>
        private void UsernameOrPasswordEmpty()
        {
            if (String.IsNullOrEmpty(this.Username) || String.IsNullOrEmpty(this.Password))
            {
                throw new ZenviaAuthenticationException("ZenviaApi: Nome de usuário/senha são requeridos!");
            }
        }

        /// <summary>
        /// Método que envia uma mensagem SMS à API Zenvia para ser enviada ao destinatário.
        /// </summary>
        /// <param name="messageSms">Objeto do tipo <see cref="SingleMessageSms"/> que contém os dados necessários para o envio.</param>
        /// <returns>Objeto do tipo <see cref="SendSmsResponse"/> contendo a resposta da requisição feita.Pode conter uma mensagem de sucesso, 
        /// confirmação de entrega ou uma mensagem informando uma falha no envio.</returns>
        public async Task<SendSmsResponse> SendSms(SingleMessageSms messageSms)
        {
            UsernameOrPasswordEmpty();
            try
            {
                SendSmsResponse result = await CallPostAsync<SendSmsResponse>(this.SendSmsUrl, messageSms);
                return result;
            }
            catch (ZenviaException ex)
            {
                throw new ZenviaException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Método que envia um conjunto de SMS em uma única requisição. Usada para envio em massa de mensagens.
        /// </summary>
        /// <param name="multipleMessageSms">Objeto do tipo <see cref="MultipleMessageSms"/> que contém os dados necessários para o envio.</param>
        /// <returns>Objeto do tipo <see cref="SendMultipleSmsResponse"/> contendo a resposta da requisição feita.Pode conter uma mensagem de sucesso, 
        /// confirmação de entrega ou uma mensagem informando uma falha no envio.</returns>
        public async Task<SendMultipleSmsResponse> SendMultipleSms(MultipleMessageSms multipleMessageSms)
        {
            UsernameOrPasswordEmpty();
            try
            {
                SendMultipleSmsResponse result = await CallPostAsync<SendMultipleSmsResponse>(this.SendMultipleSmsUrl, multipleMessageSms);
                return result;
            }
            catch (ZenviaException ex)
            {
                throw new ZenviaException("Falha no envio da requisição: " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Método responsável por cancelar uma solicitação de SMS.
        /// </summary>
        /// <param name="id">Id da mensagem SMS que deverá ser cancelada.</param>
        /// <returns>Objeto do tipo <see cref="CancelSmsResponse"/> contendo a resposta da requisição feita. 
        /// Pode conter uma mensagem de sucesso, ou uma mensagem informando uma falha na requisição.</returns>
        public async Task<CancelSmsResponse> CancelSms(String id)
        {
            UsernameOrPasswordEmpty();
            try
            {
                if (String.IsNullOrEmpty(id))
                {
                    throw new ZenviaInvalidIdException("ID não pode ser nulo ou em branco!");
                }
                CancelSmsResponse result = await CallPostAsync<CancelSmsResponse>(this.CancelSmsUrl + "/" + id, null);
                return result;
            }
            catch (Exception ex)
            {
                throw new ZenviaException("Falha no envio da requisição: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Método que obtém o estado atual de um SMS.
        /// </summary>
        /// <param name="id">Id do SMS a ser consultado.</param>
        /// <returns>Objeto do tipo <see cref="GetSmsStatusResponse"/> contendo o estado atual do SMS consultado.</returns>
        public async Task<GetSmsStatusResponse> GetSmsStatus(String id)
        {
            UsernameOrPasswordEmpty();
            try
            {
                if (String.IsNullOrEmpty(id))
                {
                    throw new ZenviaInvalidIdException("ID não pode ser nulo ou em branco!");
                }
                GetSmsStatusResponse result = await CallGetAsync<GetSmsStatusResponse>(this.GetSmsStatusUrl + "/" + id, null);
                return result;
            }
            catch (Exception ex)
            {
                throw new ZenviaException("Falha no envio da requisição: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Método que busca e retorna todas as mensagens SMS recebidas e não lidas pela plataforma. 
        /// Nota: De acordo com a documentação Zenvia, ao buscar estas mensagens as mesmas serão 
        /// consideradas como lidas e não serão mais retornadas por este método.
        /// </summary>
        /// <returns>Objeto do tipo <see cref="ReceivedMessageResponse"/> que contem as mensagens recebidas
        /// até o instante da requisição.</returns>
        public async Task<ReceivedMessageResponse> ListUnreadMessages()
        {
            UsernameOrPasswordEmpty();
            try
            {
                ReceivedMessageResponse result = await CallPostAsync<ReceivedMessageResponse>(this.ListUnreadMessagesUrl, null);
                return result;
            }
            catch (Exception ex)
            {
                throw new ZenviaException("Falha no envio da requisição: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Método responsável pela busca de mensagens recebidas considerando o filtro informado como parâmetros de busca.
        /// </summary>
        /// <param name="filter">Objeto do tipo <see cref="ReceivedMessageFilter"/> contendo os parâmetros usados para a busca.</param>
        /// <returns>Objeto do tipo <see cref="ReceivedMessageResponse"/> contendo as mensagens encontradas na busca.</returns>
        public async Task<ReceivedMessageResponse> SearchReceivedMessages(ReceivedMessageFilter filter)
        {
            UsernameOrPasswordEmpty();
            try
            {
                String url = this.SearchReceivedMessagesUrl + "/" + filter.Start.ToString(Properties.Resources.DATE_PATTERN) + "/" + filter.End.ToString(Properties.Resources.DATE_PATTERN) + "";

                Dictionary<string, string> urlParams = new Dictionary<string, string>();
                if (filter.DefinesReferenceMessageId)
                {
                    urlParams.Add("mtId", filter.ReferenceMessageId);
                }
                if (filter.DefinesMobile)
                {
                    urlParams.Add("mobile", filter.Mobile);
                }

                ReceivedMessageResponse result = await this.CallGetAsync<ReceivedMessageResponse>(url, urlParams);
                return result;
            }
            catch (Exception ex)
            {
                throw new ZenviaException("Falha no envio da requisição: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Método que faz uma requisição do tipo Get à API Zenvia.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto esperado como retorno.</typeparam>
        /// <param name="url">URL de destino da requisição.</param>
        /// <param name="obj">Objeto de tipo definido resultante da requisição.</param>
        /// <returns>Objeto de tipo definido resultante da requisição.</returns>
        private async Task<T> CallGetAsync<T>(string url, Dictionary<string, string> queryParams)
        {
            if ((queryParams != null) && (queryParams.Any()))
            {
                StringBuilder sb = new StringBuilder(url);
                if (!url.EndsWith("?"))
                {
                    sb.Append("?");
                }
                String paramString = ParamsFormat(queryParams);
                sb.Append(paramString);

                url = sb.ToString();
            }
            using (var httpClient = new HttpClient { BaseAddress = new Uri(url) })
            {
                setHeaders(httpClient);
                using (var response = await httpClient.GetAsync(url))
                {
                    return await ProcessHttpResponse<T>(response);
                }
            }
        }

        /// <summary>
        /// Método que faz uma requisição do tipo Post à API Zenvia.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto esperado como retorno.</typeparam>
        /// <param name="url">URL de destino da requisição.</param>
        /// <param name="obj">Objeto à ser serializado e enviado à API.</param>
        /// <returns>Objeto de tipo definido resultante da requisição.</returns>
        private async Task<T> CallPostAsync<T>(string url, BaseObject obj)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(url);
                setHeaders(httpClient);
                StringContent content = null;
                if (obj != null)
                {
                    content = new StringContent(obj.ToJson(), Encoding.Default, "application/json");
                }

                using (var response = await httpClient.PostAsync(url, content))
                {
                    return await ProcessHttpResponse<T>(response);
                }


            }
            catch (ArgumentNullException ex)
            {
                throw new ZenviaException("Falha ao processar requisição: " + ex.Message, ex);
            }
            catch (HttpRequestException ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Falha ao processar requisição:\n");
                sb.Append("Mensagem: " + ex.Message);
                if (ex.InnerException != null)
                {
                    sb.Append("\nDetalhe: " + ex.InnerException.Message);
                }
                throw new ZenviaException(sb.ToString(), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<T> ProcessHttpResponse<T>(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                // Código 200 - Solicitação OK.
                case HttpStatusCode.OK:
                    return Desserialize<T>(await response.Content.ReadAsStringAsync());
                // Código 201 - Criada.
                case HttpStatusCode.Created:
                    return Desserialize<T>(await response.Content.ReadAsStringAsync());
                // Código 401 - Não Autorizado.
                case HttpStatusCode.Unauthorized:
                    throw new ZenviaUnauthorizedException("O servidor da API rejeitou a solicitação: Unauthorized (401)!");
                // Código 403 - Proibido.
                case HttpStatusCode.Forbidden:
                    throw new ZenviaForbiddenException("O servidor da API rejeitou a solicitação: Forbidden (403)!");
                // Código 404 - Não encontrado.
                case HttpStatusCode.NotFound:
                    throw new ZenviaNotFoundException("O servidor da API não foi encontrado: Not Found (404)!");
                // Código 408 - Tempo esgotado.
                case HttpStatusCode.RequestTimeout:
                    throw new ZenviaRequestTimeoutException("O servidor da API não respondeu a solicitação em tempo habil: Request Timeout (408)!");
                // Código 503 - Serviço indisponível.
                case HttpStatusCode.ServiceUnavailable:
                    throw new ZenviaUnavailableException("O serviço solicitado não está disponível: Service Unavailable (503)!");
                // Qualquer outro caso diferente dos demais.
                default:
                    throw new ZenviaException(string.Format("A requisição HTTP não pôde ser atendida! StatusCode: {1}", response.StatusCode));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        private string ParamsFormat(Dictionary<string, string> queryParams)
        {
            bool haveMtId = false;
            StringBuilder sb = new StringBuilder();
            if (queryParams.ContainsKey("mtId"))
            {
                sb.Append("mtId=").Append(queryParams.Where(x => x.Key == "mtId").First().Value);
                haveMtId = true;
            }
            if (queryParams.ContainsKey("mobile"))
            {
                if (haveMtId)
                {
                    sb.Append("&");
                }
                sb.Append("mobile=").Append(queryParams.Where(x => x.Key == "mobile").First().Value);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Método que define o cabeçalho da requisição HTTP para o envio à API Zenvia.
        /// </summary>
        /// <param name="httpClient">Cliente HTTP que processará a requisição.</param>
        private void setHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("authorization", GetAuthorizationHeader(this.Username, this.Password));
        }

        /// <summary>
        /// Método que serializa um objeto para ser enviado à API Zenvia. O resultado é a represenação json do objeto.
        /// </summary>
        /// <param name="obj">Objeto que será serializado.</param>
        /// <returns>Representação json referente ao objeto informado.</returns>
        private string Serialize(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Método que desserializa uma representação json em objeto do tipo definido nos parâmetros.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto que se espera da representação json.</typeparam>
        /// <param name="json">Representação json do objeto.</param>
        /// <returns>Objeto de tipo definido nos parâmetros resultante da representação json informada.</returns>
        private T Desserialize<T>(String json)
        {
            // Contorno de um bug do Zenvia que envia uma aspa '”' ao invés de uma aspa dupla '"' quando o comando para
            // cancelar SMS agendado é chamado.
            json = json.Replace('”', '"');
            var jo = JObject.Parse(json);
            json = jo.First.First.ToString();
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private string GetAuthorizationHeader(String username, String password)
        {
            string usernamePassword = username + ":" + password;
            string encodeBase64String = Base64.Encode(usernamePassword);
            string authorizationHeader = "Basic " + encodeBase64String;
            return authorizationHeader;
        }
    }
}
