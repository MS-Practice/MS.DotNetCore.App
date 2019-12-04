using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreControllers.Demo.Filters;
using NetCoreControllers.Demo.Formatters;
using NetCoreControllers.Demo.Model;
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
            //var assembly = typeof(Startup).GetTypeInfo().Assembly;
            //var part = new AssemblyPart(assembly);
            //services.AddMvc().ConfigureApplicationPartManager(ap =>
            //{
            //    ap.ApplicationParts.Add(part);
            //    ap.FeatureProviders.Add(new GenericControllerFeatureProvider());
            //});

            services.AddMvc(options =>
            {
#if USE_ROUTINGCONVENTION
                options.Conventions.Add(new NamespaceRoutingConvention()); 
#endif
                //options.Conventions.Add(new ApplicationDescription("My Application Description"));
                //options.Conventions.Add(new MustBeInRouteParameterModelConvention());
                //添加自定义formmattters
                options.InputFormatters.Add(new VcardInputFormatter());
                options.OutputFormatters.Add(new VcardOutputFormatter());
            })
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest)
            .ConfigureApplicationPartManager(ap =>
            {
                //ap.ApplicationParts.Add(part);
#if USE_CONTROLLERFEATURE_PROVIDER_GLOBEL_FILTER
                ap.FeatureProviders.Add(new GenericControllerFeatureProvider()); 
#endif
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
            //注册DI服务类
            services.AddSingleton<IContactRepository, ContactRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == Environments.Development)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors();

            //验证
            app.UseAuthentication();
            //授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints=> {
                endpoints.MapAreaControllerRoute("blog_route", "Blog", "Manage/{controller}/{action}/{id?}");
                endpoints.MapControllerRoute("blog_route", "Manage/{controller}/{action}/{id?}", defaults: new { area = "Blog" }, constraints: new { area = "Blog" });

                endpoints.MapDefaultControllerRoute();
                //...
            });

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
