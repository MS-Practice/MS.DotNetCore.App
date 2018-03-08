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

        private readonly Func<string, IEnumerable<Policy>> _policyCreator;

        private ConcurrentDictionary<string, PolicyWrap> _policyWrappers;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResilientHttpClient(Func<string, IEnumerable<Policy>> policyCreator, ILogger logger, IHttpContextAccessor httpContextAccessor)
        {
            _client = new HttpClient();
            _logger = logger;
            _policyCreator = policyCreator;
            _policyWrappers = new ConcurrentDictionary<string, PolicyWrap>();
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            var origin = GetOriginFromUri(uri);

            return HttpInvoker(origin, async () =>
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

                SetAuthorizationHeader(requestMessage);

                if (authorizationToken != null)
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }

                if (requestId != null)
                {
                    requestMessage.Headers.Add("x-requestid", requestId);
                }

                return await _client.SendAsync(requestMessage);
            });
        }

        public Task<string> GetStringAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            var origin = GetOriginFromUri(uri);

            return HttpInvoker(origin, async () =>
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

                SetAuthorizationHeader(requestMessage);

                if (authorizationToken != null)
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }

                var response = await _client.SendAsync(requestMessage);

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException();
                }

                return await response.Content.ReadAsStringAsync();
            });
        }

        public Task<HttpResponseMessage> PostAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            return DoPostPutAsync(HttpMethod.Post, uri, item, authorizationToken, requestId, authorizationMethod);
        }

        public Task<HttpResponseMessage> PutAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            return DoPostPutAsync(HttpMethod.Put, uri, item, authorizationToken, requestId, authorizationMethod);
        }

        private Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method, string uri, T item, string authorizationToken, string requestId, string authorizationMethod)
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }
            string originFromUri = GetOriginFromUri(uri);
            return HttpInvoker(originFromUri, async () =>
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, uri);
                SetAuthorizationHeader(httpRequestMessage);
                httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                if (authorizationToken != null)
                {
                    httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }
                if (requestId != null)
                {
                    httpRequestMessage.Headers.Add("x-requestid", requestId);
                }
                var response = await _client.SendAsync(httpRequestMessage);
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException();
                }
                return response;
            });
        }

        private void SetAuthorizationHeader(HttpRequestMessage httpRequestMessage)
        {
            var authorizationHeader = this._httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                httpRequestMessage.Headers.Add("Authorization", new List<string> { authorizationHeader });
            }
        }

        private static string GetOriginFromUri(string uri)
        {
            Uri url = new Uri(uri);
            var origin = $"{url.Scheme}://{url.DnsSafeHost}:{url.Port}";
            return origin;
        }

        private async Task<T> HttpInvoker<T>(string origin, Func<Task<T>> action)
        {
            string normalizedOrigin = NormalizeOrigin(origin);
            if (!_policyWrappers.TryGetValue(normalizedOrigin, out PolicyWrap policyWrap))
            {
                policyWrap = Policy.WrapAsync(_policyCreator(normalizedOrigin).ToArray());
                _policyWrappers.TryAdd(normalizedOrigin, policyWrap);
            }
            //在wrapper定义的策略中运行所有提供的委托
            return await policyWrap.ExecuteAsync(action, new Context(normalizedOrigin));
        }

        private static string NormalizeOrigin(string origin)
        {
            return origin?.Trim()?.ToLower();
        }
    }
}
