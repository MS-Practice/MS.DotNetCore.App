using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ResilientHttpClientByPolly.Abtracts;
using System;

namespace ResilientHttpClientByPolly
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        static void Starting()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<IResilientHttpClientFactory, ResilientHttpClientFactory>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ResilientHttpClient>>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var retryCount = 6;
                var exceptionsAllowedBeforeBreaking = 5;

                return new ResilientHttpClientFactory(logger, retryCount, exceptionsAllowedBeforeBreaking, httpContextAccessor);
            });

            services.AddSingleton<IHttpClient, ResilientHttpClient>(sp => sp.GetRequiredService<IResilientHttpClientFactory>().CreateResilientHttpClient());
        }
    }
}
