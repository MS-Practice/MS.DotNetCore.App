using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace NetCoreControllers.Demo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMvc(options =>
            {
                options.Conventions.Add(new NamespaceRoutingConvention());
                options.Conventions.Add(new ApplicationDescription("My Application Description"));
            });
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
                routes.MapRoute("products", "products",defaults:new { controller="MyDemo",action= "GetProducts" });
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
