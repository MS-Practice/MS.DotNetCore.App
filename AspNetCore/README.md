## ASP.NET Core 2.1 HttpClientFactory

## 2021-01-15 更新：HttpClient 错误异常处理的优化

在使用 HttpClient 的时候，有一个 TaskCanceledException 错误，这个表示任务取消，但是这里面有个问题就是，超时也会导致任务取消错误，所以抛出来的错误类型也是 TaskCanceledException。这就会给业务带来了困惑，到底是任务取消导致的异常还是因其它原因导致超时。

在微软团队更新之前，我们只能通过传递 token 判断是否是真正取消：

```c#
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
```

现在可以直接通过判断类型即可

```c#
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
```

HTTP2.0 多连接

我们知道 HTTP2.0 规范里表明没建立一个连接是一个长连接，多个请求在同一个连接中传输这是有利于性能的，因为少了建立连接的 TCP 握手消耗。但是有些情况也特殊，比如在极短时间的峰值很高，想很少量的服务器发送传输包，这个时候如果还共享一个连接，那么这是反而性能不好了，影响了吞吐量。所以这个时候我们可以手动设置多个连接来提高性能

```c#
class Program
{
    private static readonly HttpClient _client = new HttpClient(new SocketsHttpHandler()
    {
        // 开启 HTTP2 多链接
        EnableMultipleHttp2Connections = true,

        // 这里会打印多条日志，如果把 EnableMultipleHttp2Connections 注释，则下面只会打印一条日志
        // Log each newly created connection and create the connection the same way as it would be without the callback.
        ConnectCallback = async (context, token) =>
        {
            Console.WriteLine(
                $"New connection to {context.DnsEndPoint} with request:{Environment.NewLine}{context.InitialRequestMessage}");

            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
            await socket.ConnectAsync(context.DnsEndPoint, token).ConfigureAwait(false);
            return new NetworkStream(socket, ownsSocket: true);
        },
    })
    {
        // Allow only HTTP/2, no downgrades or upgrades.
        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact,
        DefaultRequestVersion = HttpVersion.Version20
    };

    static async Task Main()
    {
        // Burst send 2000 requests in parallel.
        var tasks = new Task[2000];
        for (int i = 0; i < tasks.Length; ++i)
        {
            tasks[i] = _client.GetAsync("http://localhost:5001/");
        }
        await Task.WhenAll(tasks);
    }
}
```

## HTTP3 支持（目前不建议上生产）

这是在传输层之下的标准（QUIC）,QUIC 是一种新的基于 udp 的传输，与基于 TCP 的连接相比，它提供了一些好处：

1. —使用 TLS 建立安全连接时，握手速度更快。
2. 在单个连接上对多个请求进行更**可靠的多路复用**，消除了当线路头部阻塞时，数据包被丢弃时的问题。
3. 连接迁移使移动客户端网络之间的转换更加平滑，例如，Wi-Fi 到 LTE 再返回

实现依赖的库是 [MsQuic](https://github.com/dotnet/runtimelab/tree/feature/System.Net.Experimental.MsQuic#usage)

目前状态，详见 https://devblogs.microsoft.com/dotnet/net-5-new-networking-improvements/#http-3：

1. 只能在 windows 下特定通道使用 quic

2. 必须要引用 msquic 库，这个库目前是实验性的

3. httpclent 要支持 http3 要开启 `DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP3DRAFTSUPPORT` 环境变量：`AppContext.SetSwitch("System.Net.SocketsHttpHandler.Http3DraftSupport", true);`

4. 必须显式设置请求的版本信息

   ```c#
   new HttpRequestMessage
   {
       // Request HTTP/3 version.
       Version = new Version(3, 0),
       // Only HTTP/3 is allowed, no version downgrades should happen.
       VersionPolicy = HttpVersionPolicy.RequestVersionExact
   }
   ```

   

### 参考资料

- [HttpClientFactory in ASP.NET Core 2.1 (Part 1)](https://www.stevejgordon.co.uk/introduction-to-httpclientfactory-aspnetcore)
- [YOU'RE USING HTTPCLIENT WRONG AND IT IS DESTABILIZING YOUR SOFTWARE](https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/)
- [启动 HTTP 请求](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/http-requests?view=aspnetcore-2.1)
- [HTTPClient NET5 更新优化](https://devblogs.microsoft.com/dotnet/net-5-new-networking-improvements/#multiple-connections-with-http-2)

