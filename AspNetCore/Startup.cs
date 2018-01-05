using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore
{
    class Startup : IDisposable
    {
        public Startup()
        {
            
        }
        public void Configure(string name, string value)
        {
            Console.WriteLine($"name:{name} value:{value}");
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose was Called");
        }

        ~Startup()
        {
            Console.WriteLine("~Startup");
        }
    }
}
