namespace ResilientHttpClientByPolly
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Polly;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    public class ResilientHttpClientFactory : IResilientHttpClientFactory
    {
        private readonly ILogger<ResilientHttpClient> _logger;
        private readonly int _retryCount;
        private readonly int _exceptionsAllowedBeforeBreaking;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResilientHttpClientFactory(
            ILogger<ResilientHttpClient> logger,
            int retryCount,
            int exceptionsAllowedBeforeBreaking,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _logger = logger;
            _retryCount = retryCount;
            _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            _httpContextAccessor = httpContextAccessor;
        }

        public ResilientHttpClient CreateResilientHttpClient() => new ResilientHttpClient((origin) => CreatePolicies(), _logger, _httpContextAccessor);

        private IEnumerable<Policy> CreatePolicies()
        {
            return new Policy[]
            {
                Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(
                    //重试次数
                    _retryCount,
                    //重试间隔时间
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)),
                    //重试执行方法
                    (exception,timespan,retryCount,context)=>{
                        var msg = $"重试次数： {retryCount}， " +
                            $" {context.PolicyKey}， " +
                            $" {context.ExecutionKey}， " +
                            $"原因： {exception}。";
                        _logger.LogWarning(msg);
                        _logger.LogDebug(msg);
                    }),
                Policy.Handle<HttpRequestException>()
                .CircuitBreakerAsync(
                    // 熔断允许的最大次数
                    _exceptionsAllowedBeforeBreaking,
                    // 熔断后多长时间再允许重试
                    TimeSpan.FromMinutes(1),
                    (exception,duration)=>{
                        //熔断
                        _logger.LogTrace("启用熔断。");
                    },
                    () =>
                   {
                        // 取消熔断
                        _logger.LogTrace("熔断取消。");
                   })
            };
        }
    }
}
