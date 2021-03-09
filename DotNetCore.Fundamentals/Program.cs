using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DotNetCore.Fundamentals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
#if CONFIGRE_KESTREL
                // 关于 Kestrel 的终结点配置详见:https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/servers/kestrel/endpoints?view=aspnetcore-5.0
                .UseKestrel()
                .ConfigureKestrel(serverOptions => {
                    serverOptions.ConfigureHttpsDefaults(listenOptions => {
                        // 设置证实
                        listenOptions.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2();
                    });
                })
#endif
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .ConfigureLogging(logger =>
                {
                    logger.AddConsole();
                    logger.SetMinimumLevel(LogLevel.Information);
                })
                .UseStartup<Startup>()
                .Build();

        public static IWebHostBuilder KestrelConfiguration(IWebHostBuilder builder) {
            var webHostBuilder = builder.UseKestrel((context, serverOptions) =>
            {
                serverOptions.Configure(context.Configuration.GetSection("Kestrel"))
                .Endpoint("HTTPS", listenOptions =>
                {
                    listenOptions.HttpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                });
            });
            // 设置 Kestrel 一般限制
            webHostBuilder.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Limits.MaxConcurrentConnections = 100;
                serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
                // 请求体大小
                serverOptions.Limits.MaxRequestBodySize = 1024 * 10;
                // 生存期时间
                serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                //...
            });

            return webHostBuilder;
        }
    }
}
