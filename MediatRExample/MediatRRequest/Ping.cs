using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatRExample.MediatRRequest
{
    public class Ping: IRequest<string>
    {
        public Ping(string message)
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
