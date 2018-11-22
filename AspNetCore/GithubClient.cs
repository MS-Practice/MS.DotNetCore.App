using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCore {
    public class GithubClient {
        public HttpClient Client { get; }

        public GithubClient(HttpClient client) {
            client.BaseAddress = new Uri("https://api.github.com/");
            // GitHub API versioning
            client.DefaultRequestHeaders.Add("Accept",
                "application/vnd.github.v3+json");
            // GitHub requires a user-agent
            client.DefaultRequestHeaders.Add("User-Agent",
                "HttpClientFactory-Sample");

            Client = client;
        }

        public async Task GetAspNetDocsIssues() {
            var response = await Client.GetAsync(
                "/repos/aspnet/docs/issues?state=open&sort=created&direction=desc");

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadAsStringAsync();
            Console.WriteLine("类型化客户端 HttpClient：调用结果=" + result);
        }
    }
}