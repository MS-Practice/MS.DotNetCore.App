using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore
{
    internal class HttpClientUpdateStudy
    {
        private static readonly HttpClient _client = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        public static async Task Start()
        {
            // 原来只能通过传递 token 来辨别是真超时还是取消操作
            await Original();
            // 现在
            await NowTaskCancel();
            await NewHttpStatusCode();
            // .NET5 Http'Client 支持手动选择 HTTP 版本以及策略
            await NewHttpVersionManuallySelect();
            // 增加了PING功能，对于长时间空闲的连接会被自动释放，所以需要PING来保持激活状态
            // PING 是不会发送任何数据的
            await NewPING();
            // HTTP3特性
            // 
        }

        private static async Task NewPING()
        {
            var client = new HttpClient(new SocketsHttpHandler() { 
                KeepAlivePingDelay = TimeSpan.FromSeconds(60)
            });
            await Task.CompletedTask;
        }

        private static async Task NewHttpVersionManuallySelect()
        {
            var client = new HttpClient()
            {
                // 只支持 HTTP2.0
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact,
                DefaultRequestVersion = HttpVersion.Version20
            };
            await client.GetStringAsync("");
        }

        /// <summary>
        /// HttpRequestException 新增了 StatusCode 属性，可以用于异常过滤
        /// 此特性要在NET5及以上
        /// </summary>
        /// <returns></returns>
        private static async Task NewHttpStatusCode()
        {
            try
            {
                using var response = await _client.GetAsync("https://localhost:5001/doesNotExists");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Handle 404
                Console.WriteLine("Not found: " + ex.Message);
                throw;
            }
            // 要注意，只有返回 HttpResponseMessage 时要调用 EnsureSuccessStatusCode 方法
            // 而调用 GetStringAsync, GetByteArrayAsync 和 GetStreamAsync，这些方法不是返回 HttpResponseMessage，它们内部调用了 EnsureSuccessStatusCode 方法，如
            try
            {
                using var response = await _client.GetStreamAsync("https://localhost:5001/doesNotExists");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // handle
            }
        }

        private static async Task NowTaskCancel()
        {
            try
            {
                using var response = await _client.GetAsync("http://localhost:5001/sleepFor?seconds=100");
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                await Console.Out.WriteAsync("是真超时,不是取消");
            }
            catch (TaskCanceledException ex)
            {
                await Console.Out.WriteAsync("取消操作：" + ex.Message);
            }
        }

        private static async Task Original()
        {
            var cts = new CancellationToken();
            try
            {
                using var response = await _client.GetAsync("http://localhost:5001/sleepFor?seconds=100", cts);
            }
            catch (TaskCanceledException) when (cts.IsCancellationRequested)
            {
                await Console.Out.WriteAsync("取消操作，并不是超时");
            }
            catch (TaskCanceledException ex)
            {
                await Console.Out.WriteAsync("超时：" + ex.Message);
            }
        }
    }
}