using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore {
    /// <summary>
    /// 消息处理程序中间件，能到 HttpClient AOP 的效果，处理出栈 HTTP 请求。
    /// 能在请求传递至管道的下一个处理程序之前执行代码
    /// 然后通过 DI 注册
    /// </summary>
    public class ValidateHeaderHandler : DelegatingHandler {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) {
            if (!request.Headers.Contains("X-API-KEY")) {
                Console.WriteLine("处理程序截断，Http Header 不包含 X-API-KEY");
                return new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(
                        "You must supply an API key header called X-API-KEY")
                };
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}