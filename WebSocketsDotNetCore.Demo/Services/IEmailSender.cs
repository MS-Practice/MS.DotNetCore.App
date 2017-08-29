using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDotNetCore.Demo.Services
{
    public interface IEmailSender
    {
        Task SendEmail(string mail, string subject, string message);
    }
}
