namespace ResilientHttpClientByPolly
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json;
    using Polly;
    using Polly.Wrap;
    using ResilientHttpClientByPolly.Abtracts;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class ResilientHttpClient : IHttpClient
    {
        private readonly HttpClient _client;

        private readonly ILogger _logger;

        private readonly Func<string, IEnumerable<AsyncPolicy>> _policyCreator;

        private ConcurrentDictionary<string, AsyncPolicyWrap> _policyWrappers;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResilientHttpClient(Func<string, IEnumerable<AsyncPolicy>> policyCreator, ILogger logger, IHttpContextAccessor httpContextAccessor)
        {
            this._client = new HttpClient();
            this._logger = logger;
            this._policyCreator = policyCreator;
            this._policyWrappers = new ConcurrentDictionary<string, AsyncPolicyWrap>();
            this._httpContextAccessor = httpContextAccessor;
        }

        public Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            throw new NotImplementedException();
        }

        public Task<string> GetStringAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> PostAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            return DoPostPutAsync(HttpMethod.Post, uri, item, authorizationToken, requestId, authorizationMethod);
        }

        public Task<HttpResponseMessage> PutAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            throw new NotImplementedException();
        }

        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method, string uri, T item, string authorizationToken, string requestId, string authorizationMethod)
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", "method");
            }
            string originFromUri = GetOriginFromUri(uri);
            return await HttpInvoker(originFromUri, (context) =>
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, uri);
                SetAuthorizationHeader(httpRequestMessage);
                httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject((object)(T)item), Encoding.UTF8, "application/json");
                if (authorizationToken != null)
                {
                    httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }
                if (requestId != null)
                {
                    httpRequestMessage.Headers.Add("x-requestid", requestId);
                }
                var obj = this._client.SendAsync(httpRequestMessage);
                if (obj.Result.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException();
                }
                return obj;
            });
        }

        private void SetAuthorizationHeader(HttpRequestMessage httpRequestMessage)
        {
            StringValues val = this._httpContextAccessor.HttpContext.Request.Headers.GetCommaSeparatedValues("Authorization");
            if (!string.IsNullOrEmpty(val))
            {
                httpRequestMessage.Headers.Add("Authorization", new List<string>
                {

                });
            }
        }

        private static string GetOriginFromUri(string uri)
        {
            Uri uri2 = new Uri(uri);
            return string.Format("{0}://{1}:{2}", uri2.Scheme, uri2.DnsSafeHost, uri2.Port);
        }

        private async Task<T> HttpInvoker<T>(string origin, Func<Context,Task<T>> action)
        {
            string text = NormalizeOrigin(origin);
            if (!this._policyWrappers.TryGetValue(text, out AsyncPolicyWrap val))
            {
                val = Policy.WrapAsync((IAsyncPolicy[])Enumerable.ToArray(_policyCreator(text)));
                this._policyWrappers.TryAdd(text, val);
            }
            return await val.ExecuteAsync<T>(action, new Context(text));
        }

        private static string NormalizeOrigin(string origin)
        {
            if (origin == null)
            {
                return null;
            }
            string text = origin.Trim();
            if (text == null)
            {
                return null;
            }
            return text.ToLower();
        }
    }
}
