using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDotNetCore.Demo.Services
{
    public class AuthMessageSender: ISmsSender, IEmailSender
    {
        public Task SendEmail(string mail, string subject, string message)
        {
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            return Task.FromResult(0);
        }
    }
}
