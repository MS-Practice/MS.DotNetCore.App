using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NetCoreControllers.Demo.Filters;
using NetCoreControllers.Demo.Providers;
using System.Reflection;

namespace NetCoreControllers.Demo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
#if 新增ApplicationPart从指定的类的程序集_方法1

            var assembly = typeof(Startup).GetTypeInfo().Assembly;
            services.AddMvc()
                .AddApplicationPart(assembly); 
#endif
            var assembly = typeof(Startup).GetTypeInfo().Assembly;
            var part = new AssemblyPart(assembly);
            services.AddMvc().ConfigureApplicationPartManager(ap =>
            {
                ap.ApplicationParts.Add(part);
                ap.FeatureProviders.Add(new GenericControllerFeatureProvider());
            });

            services.AddMvc(options =>
            {
                options.Conventions.Add(new NamespaceRoutingConvention());
                options.Conventions.Add(new ApplicationDescription("My Application Description"));
                options.Conventions.Add(new MustBeInRouteParameterModelConvention());
            });

            //ServiceFilterAttribute
            services.AddScoped<AddHeaderFilterWithDI>();
            //更改mvc area文件搜索规则
#if TURE_CHANGE_ENGINE_SEARCH
            services.Configure<RazorViewEngineOptions>(options =>
    {
        options.AreaViewLocationFormats.Clear();
        options.AreaViewLocationFormats.Add("/Categories/{2}/Views/{1}/{0}.cshtml");
        options.AreaViewLocationFormats.Add("/Categories/{2}/Views/Shared/{0}.cshtml");
        options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
    }); 
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapAreaRoute("blog_route", "Blog", "Manage/{controller}/{action}/{id?}");
#if The_Same_MapAreaRoute
                routes.MapRoute("blog_route", "Manage/{controller}/{action}/{id?}", defaults: new { area = "Blog" }, constraints: new { area = "Blog" }); 
#endif
                routes.MapRoute(name: "Products", template: "{area:exists}/{controller=Home}/{action=Index}");
                //routes.MapRoute("products", "products",defaults:new { controller="MyDemo",action= "GetProducts" });
                //routes.MapRoute("getName", "{controller=Home}/{action=Hi}/{name?}", defaults: new { name = "" });
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}", new { country = "US" });
            });

            //app.UseMvcWithDefaultRoute();
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
