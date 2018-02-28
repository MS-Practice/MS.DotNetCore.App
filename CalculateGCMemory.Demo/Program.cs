using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateGCMemory.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentBytes = GC.GetTotalMemory(true);
            var obj = new object();
            GC.KeepAlive(obj);
            var objSize = GC.GetTotalMemory(true) - currentBytes;
            Console.WriteLine(objSize+ Environment.NewLine+ currentBytes.GetHashCode()+Environment.NewLine+obj.GetHashCode());

            //var array = new[] { 1, 0, 5, 11, 23, 5569, 523, 45, 7, 256, 14, 52, 35, 47, 102, 5, 485, 7, 165, 4654, 231, 54, 56, 156, 156, 42, 12, 159, 4789, 7, 894, 21, 321, 64, 54, 21, 54, 54, 231, 2314 };
            //new SortHelper().QuickSort(array);
            //var str = "";
            //Array.ForEach(array, item =>
            //{
            //    str += item + " ";
            //});
            //Console.WriteLine(str);
            Console.ReadLine();
        }
    }
}
