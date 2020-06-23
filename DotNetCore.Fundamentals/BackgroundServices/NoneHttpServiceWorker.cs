using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCore.Fundamentals.BackgroundServices
{
    /// <summary>
    /// 如果后台工作任务做的是与HTTP无关的，那就应该用 IHost
    /// 否则是 IWebHost
    /// </summary>
    public class NoneHttpServiceWorker : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class ExampleHostedService : IHostedService
    {
        private readonly ILogger<ExampleHostedService> _logger;
        public ExampleHostedService(ILogger<ExampleHostedService> logger)
        {
            _logger = logger;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _logger.LogInformation("example hosted service started!"));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _logger.LogInformation("example hosted service stoped!"));
        }
    }
}
