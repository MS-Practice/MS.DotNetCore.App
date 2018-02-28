using MediatR;
using MediatRExample.MediatRRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRExample.MediatRRequestHandler
{
    //public class PingHandler : IRequestHandler<Ping,string>
    //{
    //    public async Task<string> Handle(Ping request, CancellationToken cancellationToken)
    //    {
    //        return await Task.FromResult(request.ToString());
    //    }
    //}

    //异步非Cancellation token模式
    public class AsyncNoCancellation : AsyncRequestHandler<Ping, string>
    {
        protected override async Task<string> HandleCore(Ping request)
        {
            return await Task.FromResult(request.ToString());
        }
    }
    //同步模式
    public class SyncHandler : RequestHandler<Ping, string>
    {
        protected override string HandleCore(Ping request)
        {
            return request.ToString();
        }
    }
}
