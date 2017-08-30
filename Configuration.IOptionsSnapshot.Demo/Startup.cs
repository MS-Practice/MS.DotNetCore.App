using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Configuration.IOptionsSnapshot.Demo
{
    public class TimeOptions
    {
        // Records the time when the options are created.
        public DateTime CreationTime { get; set; } = DateTime.Now;

        // Bound to config. Changes to the value of "Message"
        // in config.json will be reflected in this property.
        public string Message { get; set; }
    }

    public class Controller
    {
        public readonly TimeOptions _options;

        public Controller(IOptionsSnapshot<TimeOptions> options)
        {
            _options = options.Value;
        }

        public Task DisplayTimeAsync(HttpContext context)
        {
            return context.Response.WriteAsync(_options.Message + _options.CreationTime);
        }
    }

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Controller>();
            services.Configure<TimeOptions>(Configuration.GetSection("Time"));
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
#if OriginalCode
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            }); 
#endif
            app.Run(DisplayTimeAsync);
        }

        private Task DisplayTimeAsync(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            return context.RequestServices.GetRequiredService<Controller>().DisplayTimeAsync(context);
        }
    }
}
