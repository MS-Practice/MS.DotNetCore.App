using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCore {
    public class HttpClientFactoryStudy {
        private readonly IHttpClientFactory _clientFactory;
        public HttpClientFactoryStudy(IHttpClientFactory clientFactory) {
            _clientFactory = clientFactory;
        }

        public async Task OnGet() {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/aspnet/docs/branches");
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _clientFactory.CreateClient(); //由连接池自己管理新建client
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode) {
                Console.WriteLine("client 调用成功");
            } else {
                Console.WriteLine("client 请求失败");
            }
        }

        public async Task OnGetSpecifiedHttpClient() {
            var request = new HttpRequestMessage(HttpMethod.Get, "repos/aspnet/docs/branches");
            var client = _clientFactory.CreateClient("github");
            Console.WriteLine("获取特定的HttpClient：");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode) {
                Console.WriteLine("client 调用成功");
            } else {
                Console.WriteLine("client 请求失败");
            }
        }
    }
}