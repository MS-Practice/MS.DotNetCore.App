using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatRExample.Publishing
{
    public class PingNotification : INotification
    {
        public PingNotification(string message)
        {
            Message = message;
        }

        public override string ToString()
        {
            return this.Message;
        }

        public string Message { get; set; }
    }
}
