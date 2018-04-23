using DeepCopyCore.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AspNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //Startup startup = new Startup();
            //var methods = startup.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            //var methodInfo = methods.FirstOrDefault(m => m.Name == "Configure");
            ////获取参数
            //var parameterInfos = methodInfo.GetParameters();
            //var parameters = new object[parameterInfos.Length];
            //for (var index = 0; index < parameterInfos.Length; index++)
            //{
            //    var type = parameterInfos[index].ParameterType;
            //    switch (type.ToString())
            //    {
            //        case "System.String":
            //            parameters[index] = "methodName";
            //            break;
            //        case "System.Int32":
            //            parameters[index] = 100;
            //            break;
            //    }
            //}

            //methodInfo.Invoke(startup, parameters);
            //startup.Dispose();
            //GC.Collect();

            //var services = new ServiceCollection();


            //services.AddScoped<Startup>();
            //IServiceProvider serviceProvider = new DefaultServiceProviderFactory().CreateServiceProvider(services);
            ////注册
            //var scope = serviceProvider.CreateScope();
            //var startup = scope.ServiceProvider.GetService<Startup>();
            //startup.Configure("name", "value");
            //scope.Dispose();


            //IEnumerable<string> enumerable = new[] { "xxx", "eee", "rrr", "ttt" };
            //Random rd = new Random(10000);
            //var result = enumerable.RandomEnumerableValue(rd);
            //Console.Write(result);
            DeepCopyMethod_Test();
            Console.Read();
        }

        private static void DeepCopyMethod_Test()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000000; i++)
            {
                Student s = new Student
                {
                    Age = 25,
                    Id = 1,
                    Name = "MarsonShine"
                };
                var dto = DeepCopyCore.DeepCopyByRelection.TransRelection<Student, StudentDto>(s);
            }
            sw.Stop();
            Console.WriteLine("TransRelection :" + sw.ElapsedMilliseconds + " ms");

            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                Student s = new Student
                {
                    Age = 25,
                    Id = 1,
                    Name = "MarsonShine"
                };
                var dto = DeepCopyCore.DeepCopyBySerialization.TransSerialization<Student, StudentDto>(s);
            }
            sw.Stop();
            Console.WriteLine("TransSerialization :" + sw.ElapsedMilliseconds + " ms");

            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                Student s = new Student
                {
                    Age = 25,
                    Id = 1,
                    Name = "MarsonShine"
                };
                var dto = DeepCopyCore.DeepCopyByExpression.TransExp<Student, StudentDto>(s);
            }
            sw.Stop();
            Console.WriteLine("TransExpression :" + sw.ElapsedMilliseconds + " ms");

            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                Student s = new Student
                {
                    Age = 25,
                    Id = 1,
                    Name = "MarsonShine"
                };
                var dto = DeepCopyCore.TransExpByGeneric<Student, StudentDto>.Trans(s);
            }
            sw.Stop();
            Console.WriteLine("TransExpByGeneric :" + sw.ElapsedMilliseconds + " ms");
        }
    }
}
