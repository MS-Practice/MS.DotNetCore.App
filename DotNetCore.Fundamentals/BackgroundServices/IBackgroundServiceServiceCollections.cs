using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Fundamentals.BackgroundServices
{
    public static class IBackgroundServiceServiceCollections
    {
        public static void AddBackgroudWorker(this IServiceCollection services)
        {
            services.AddHostedService<ExampleHostedService>();
        }
    }
}
