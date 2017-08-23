using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Routing;

namespace DotNetCore.Fundamentals
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
            services.AddDirectoryBrowser();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var trackPackageRouteHandler = new RouteHandler(context =>
            {
                var routeValues = context.GetRouteData().Values;
                return context.Response.WriteAsync($"Hello! Route values: {string.Join(", ", routeValues)}");
            });
            var routeBuilder = new RouteBuilder(app, trackPackageRouteHandler);
            routeBuilder.MapRoute(
                "Track Package Route",
                "package/{operation:regex(^(track|create|detonate)$)}/{id:int}"
                );

            routeBuilder.MapGet("hello/{name}", context =>
            {
                var name = context.GetRouteValue("name");
                return context.Response.WriteAsync($"Hi,{name}");
            });

            var router = routeBuilder.Build();
            app.UseRouter(router);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //Serve my app-specific default file,if present
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("mydefault.html");
            app.UseDefaultFiles(options);

            //FileExtensionContentTypeProvider
            // Set up custom content types -associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".myapp"] = "application/x-msdownload";
            provider.Mappings[".htm3"] = "text/html";
            provider.Mappings[".image"] = "image/png";
            provider.Mappings[".rtf"] = "application/x-msdownload";
            provider.Mappings.Remove(".mp4");

            //app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
                RequestPath = new PathString("/StaticFiles"),
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                },
                ContentTypeProvider = provider
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot","images")),
                RequestPath = new PathString("/MyImages")
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "us_english_products",
                    template: "en-US/Products/{id}",
                    defaults: new { controller = "Products", action = "Details" },
                    constraints: new { id = new IntRouteConstraint() },
                    dataTokens: new { locale = "en-US" }
                    );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run(async context =>
            {
                var dictionary = new RouteValueDictionary
                {
                    { "operation","create"},
                    { "id",123}
                };
                var vpc = new VirtualPathContext(context, null, dictionary, "Track Package Route");
                var path = router.GetVirtualPath(vpc).VirtualPath;

                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("Menu<hr/>");
                await context.Response.WriteAsync($"<a href='{path}'>Create Package 123</a>");
            });
        }
    }
}
