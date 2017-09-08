using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using Microsoft.AspNetCore.Server.HttpSys;

namespace WebAppSessionState.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
#if USE_KESTREL_CONFIGURATION_OPTIONS
        .UseKestrel(Options) 
#endif
                .UseStartup<Startup>()
                .Build();

#if USE_KESTREL_CONFIGURATION_OPTIONS
        public static void Options(KestrelServerOptions options)
        {
            options.Limits.MaxConcurrentConnections = 100;
            options.Limits.MaxConcurrentUpgradedConnections = 100;
            options.Limits.MaxRequestBodySize = 10 * 1024;
            options.Limits.MinRequestBodyDataRate = new MinDataRate(100, TimeSpan.FromSeconds(10));
            options.Limits.MinResponseDataRate = new MinDataRate(100, TimeSpan.FromSeconds(10));
        #region Bind to TCP socket
		options.Listen(IPAddress.Loopback, 5000);
            options.Listen(IPAddress.Loopback, 5001, listenOptions =>
            {
                listenOptions.UseHttps("testCert.pfx", "testPassword");
            }); 
        #endregion
        } 
#endif

        public static void Options(HttpSysOptions options)
        {
            options.Authentication.Schemes = Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes.None;
            options.Authentication.AllowAnonymous = true;
            options.MaxConnections = 100;
            options.MaxRequestBodySize = 30000000;
            options.UrlPrefixes.Add("http://localhost:5000");
        }
    }
}
