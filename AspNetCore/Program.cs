using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using DeepCopyCore.Models;
using Microsoft.Extensions.DependencyInjection;
using Nito.AsyncEx;

namespace AspNetCore {
    class Program {
        private static readonly ServiceCollection m_services = new ServiceCollection();

        public static ServiceCollection Services => m_services;

        static void Main(string[] args) {
            SeletedByVersion();
            //Startup startup = new Startup();
            //var methods = startup.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            //var methodInfo = methods.FirstOrDefault(m => m.Name == "Configure");
            ////获取参数
            //var parameterInfos = methodInfo.GetParameters();
            //var parameters = new object[parameterInfos.Length];
            //for (var index = 0; index < parameterInfos.Length; index++)
            //{
            //    var type = parameterInfos[index].ParameterType;
            //    switch (type.ToString())
            //    {
            //        case "System.String":
            //            parameters[index] = "methodName";
            //            break;
            //        case "System.Int32":
            //            parameters[index] = 100;
            //            break;
            //    }
            //}

            //methodInfo.Invoke(startup, parameters);
            //startup.Dispose();
            //GC.Collect();

            //var services = new ServiceCollection();

            //services.AddScoped<Startup>();
            //IServiceProvider serviceProvider = new DefaultServiceProviderFactory().CreateServiceProvider(services);
            ////注册
            //var scope = serviceProvider.CreateScope();
            //var startup = scope.ServiceProvider.GetService<Startup>();
            //startup.Configure("name", "value");
            //scope.Dispose();

            //IEnumerable<string> enumerable = new[] { "xxx", "eee", "rrr", "ttt" };
            //Random rd = new Random(10000);
            //var result = enumerable.RandomEnumerableValue(rd);
            //Console.Write(result);
            // DeepCopyMethod_Test();

            Installer();
            var serviceProvider = Services.BuildServiceProvider();
            var githubClient = serviceProvider.GetService<GithubClient>();
            var client = serviceProvider.GetService<IHttpClientFactory>();
            var clientStudy = new HttpClientFactoryStudy(client);
            var externalClient = client.CreateClient("externalClient");
            AsyncContext.Run(() => clientStudy.OnGet());
            AsyncContext.Run(() => clientStudy.OnGetSpecifiedHttpClient());
            AsyncContext.Run(() => githubClient.GetAspNetDocsIssues());
            // AsyncContext.Run(() => externalClient.GetStringAsync("repos/aspnet/docs/branches"));
            Console.ReadLine();
        }

        private static void Installer() {
            //注册 HttpClient
            // Services.AddHttpClient();
            //给 httpclient 命名客户端来配置相关的配置，处理用户特定的逻辑
            Services.AddHttpClient("github", client => {
                client.BaseAddress = new Uri("https://api.github.com/");
                //添加 github 接口版本
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                //添加 user-agent
                client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            });
            //类型化客户端 HttpClient
            Services.AddHttpClient<GithubClient>();

            //通过注册处理程序来达到 AOP 的效果，在出战请求（消息传递管道中下一个处理程序之前）结束前调用
            Services.AddTransient<ValidateHeaderHandler>();
            Services.AddHttpClient("externalClient", c => {
                    c.BaseAddress = new Uri("https://api.github.com/");
                })
                .AddHttpMessageHandler<ValidateHeaderHandler>(); //可以注册多个处理程序
            // .AddHttpMessageHandler<...>();
        }

        private static void DeepCopyMethod_Test() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000000; i++) {
                Student s = new Student {
                    Age = 25,
                    Id = 1,
                    Name = "MarsonShine"
                };
                var dto = DeepCopyCore.DeepCopyByRelection.TransRelection<Student, StudentDto>(s);
            }
            sw.Stop();
            Console.WriteLine("TransRelection :" + sw.ElapsedMilliseconds + " ms");

            sw.Restart();
            for (int i = 0; i < 1000000; i++) {
                Student s = new Student {
                    Age = 25,
                    Id = 1,
                    Name = "MarsonShine"
                };
                var dto = DeepCopyCore.DeepCopyBySerialization.TransSerialization<Student, StudentDto>(s);
            }
            sw.Stop();
            Console.WriteLine("TransSerialization :" + sw.ElapsedMilliseconds + " ms");

            sw.Restart();
            for (int i = 0; i < 1000000; i++) {
                Student s = new Student {
                    Age = 25,
                    Id = 1,
                    Name = "MarsonShine"
                };
                var dto = DeepCopyCore.DeepCopyByExpression.TransExp<Student, StudentDto>(s);
            }
            sw.Stop();
            Console.WriteLine("TransExpression :" + sw.ElapsedMilliseconds + " ms");

            sw.Restart();
            for (int i = 0; i < 1000000; i++) {
                Student s = new Student {
                    Age = 25,
                    Id = 1,
                    Name = "MarsonShine"
                };
                var dto = DeepCopyCore.TransExpByGeneric<Student, StudentDto>.Trans(s);
            }
            sw.Stop();
            Console.WriteLine("TransExpByGeneric :" + sw.ElapsedMilliseconds + " ms");
        }

        private static void SeletedByVersion() {
#if NET45
            Console.WriteLine("NET45");
#elif NETCOREAPP2_1
            Console.WriteLine("NETCOREAPP2_1");
#elif NETCOREAPP2_0
            Console.WriteLine("NETCOREAPP2_0");
#endif
        }
    }
}