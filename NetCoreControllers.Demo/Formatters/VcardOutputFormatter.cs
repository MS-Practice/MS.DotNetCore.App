using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Net.Http.Headers;
using NetCoreControllers.Demo.Model;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace NetCoreControllers.Demo.Formatters
{
    //在Starpup.ConfigureServices注入
    public class VcardOutputFormatter : TextOutputFormatter
    {
        public VcardOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/vcard"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            //获取DI容器
            //注意，不能再构造函数中注入服务，只能通过上下文传递的数据获取DI容器
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var logger = serviceProvider.GetService(typeof(ILogger<VcardOutputFormatter>)) as ILogger;

            var response = context.HttpContext.Response;

            var buffer = new StringBuilder();
            if(context.Object is IEnumerable<Contact>)
            {
                foreach (var contact in context.Object as IEnumerable<Contact>)
                {
                    FormatVcard(buffer, contact, logger);
                }
            }
            else
            {
                var contact = context.Object as Contact;
                FormatVcard(buffer, contact, logger);
            }
            return response.WriteAsync(buffer.ToString());
        }

        private void FormatVcard(StringBuilder buffer, Contact contact, ILogger logger)
        {
            buffer.AppendLine("BEGIN:VCARD");
            buffer.AppendLine("VERSION:2.1");
            buffer.AppendFormat($"N:{contact.LastName};{contact.FirstName}\r\n");
            buffer.AppendFormat($"FN:{contact.FirstName} {contact.LastName}\r\n");
            buffer.AppendFormat($"UID:{contact.ID}\r\n");
            buffer.AppendLine("END:VCARD");
            logger.LogInformation($"Writing {contact.FirstName} {contact.LastName}");
        }

        protected override bool CanWriteType(Type type)
        {
            if(typeof(Contact).IsAssignableFrom(type)
                || typeof(IEnumerable<Contact>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }
        

    }
}
