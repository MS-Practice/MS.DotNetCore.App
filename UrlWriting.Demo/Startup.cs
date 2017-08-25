using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.AspNetCore.Rewrite;

namespace UrlWriting.Demo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //UseReWriterMiddleware
            using (var apacheModRewriteStreamReader = File.OpenText("ApacheModRewrite.txt"))
            using (var iisUrlRewriteStreamReader = File.OpenText("IISUrlRewrite.xml"))
            {
                var options = new RewriteOptions()
                    .AddRedirect("redirect-rule/(.*)", "redirected/$1")
                    .AddRewrite(@"^rewrite-rule/(\d+)/(\d+)", "rewritten?val1=$1&val2=$2", skipRemainingRules: true)
                    .AddApacheModRewrite(apacheModRewriteStreamReader)
                    .AddIISUrlRewrite(iisUrlRewriteStreamReader)
                    .Add(new RedirectImageRequests(".png", "/png-images"))
                    .Add(new RedirectImageRequests(".jpg", "/png-images"));

                app.UseRewriter(options);
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Rewritten or Redirected Url: {context.Request.Path + context.Request.QueryString}");
            });
        }
    }
}
