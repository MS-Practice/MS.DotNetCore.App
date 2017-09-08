using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.IO;
using System.Globalization;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace OwinSample.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
#if NETCORE_APP_DEFAULT_SETTING
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            }); 
#endif
            app.UseOwin(pipeline =>
            {
                pipeline(next => OwinHello);
            });

            app.Use(async (context, next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await EchoWebSocketAsync(webSocket);
                }
                else
                {
                    await next();
                }
            });

            app.Run(context =>
            {
                return context.Response.WriteAsync("Hello World");
            });
        }

        private async Task EchoWebSocketAsync(WebSocket webSocket)
        {
            byte[] buffer = new byte[1024];
            WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!webSocket.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, receiveResult.Count), receiveResult.MessageType, receiveResult.EndOfMessage, CancellationToken.None);
                receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),
                CancellationToken.None);
            }

            await webSocket.CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, CancellationToken.None);
        }

        public Task OwinHello(IDictionary<string,object> environment)
        {
            string responseText = "Hello World via OWIN";
            byte[] responseBytes = Encoding.UTF8.GetBytes(responseText);
            // OWIN Environment Keys: http://owin.org/spec/spec/owin-1.0.0.html
            var responseStream = (Stream)environment["owin.ResponseBody"];
            var responseHeaders = (IDictionary<string, string[]>)environment["owin.ResponseHeaders"];

            responseHeaders["Content-Type"] = new string[] { "text/plain" };
            responseHeaders["Content-Length"] = new string[] { responseBytes.Length.ToString(CultureInfo.InvariantCulture) };

            return responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
    }
}
