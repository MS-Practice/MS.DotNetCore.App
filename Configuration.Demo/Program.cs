
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;
using Configuration.Demo.EF;
using Microsoft.EntityFrameworkCore;

namespace Configuration.Demo
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        static void Main(string[] args)
        {
#if ConfigConsole
            var builder = new ConfigurationBuilder();
            Console.WriteLine("Initial Config Sources:" + builder.Sources.Count());

            builder.AddInMemoryCollection(new Dictionary<string, string>
            {
                {"username","Guest" }
            });
            Console.WriteLine("Added Memory Source. Sources: " + builder.Sources.Count());

            builder.AddCommandLine(args);
            Console.WriteLine("Added Command Line Source. Sources: " + builder.Sources.Count());

            var config = builder.Build();
            string username = config["username"];
            Console.WriteLine($"Hello, {username}!");
#endif

#if SimpleConfiguration
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            Console.WriteLine($"option1 = {Configuration["option1"]}");
            Console.WriteLine($"option2 = {Configuration["option2"]}");
            Console.WriteLine(
                $"suboption1 = {Configuration["subsection:suboption1"]}");
            Console.WriteLine();

            Console.WriteLine("Wizards:");
            Console.Write($"{Configuration["wizards:0:Name"]}, ");
            Console.WriteLine($"age {Configuration["wizards:0:Age"]}");
            Console.Write($"{Configuration["wizards:1:Name"]}, ");
            Console.WriteLine($"age {Configuration["wizards:1:Age"]}");
#endif


            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            var connectionStringConfig = builder.Build();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEntityFrameworkConfig(options => options.UseSqlServer(connectionStringConfig.GetConnectionString("DefaultConnection")))
                .Build();
            Console.WriteLine("key1={0}", config["key1"]);
            Console.WriteLine("key2={0}", config["key2"]);
            Console.WriteLine("key3={0}", config["key3"]);
        }
    }
}
