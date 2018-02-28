using MediatR;
using MediatRExample.MediatRRequest;
using MediatRExample.Publishing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRExample.NotificationHandler
{
    public class PingNotificationHandler : INotificationHandler<PingNotification>
    {
        public async Task Handle(PingNotification notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine(notification.ToString());
            await Task.CompletedTask;
        }
    }

    public class Ping2NotificationHandler : INotificationHandler<PingNotification>
    {
        public async Task Handle(PingNotification notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine(notification.ToString() + " 2");
            await Task.CompletedTask;
        }
    }
}
