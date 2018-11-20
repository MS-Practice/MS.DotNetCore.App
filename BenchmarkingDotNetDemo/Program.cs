using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BenchmarkingDotNetDemo
{
    public class Program
    {
        public class SingleVsFirst
        {
            private readonly List<string> _haystack = new List<string>();
            private readonly int _haystackSize = 1000000;
            private readonly string _needle = "needle";
            public List<string> _needles => new List<string> { "StartNeedle", "MiddleNeedle", "EndNeedle" };

            public SingleVsFirst()
            {
                //Add a large amount of items to our list. 
                Enumerable.Range(1, _haystackSize).ToList().ForEach(x => _haystack.Add(x.ToString()));
                //Insert the needle right in the middle. 
                //_haystack.Insert(_haystackSize / 2, _needle);
                //One at the start. 
                _haystack.Insert(0, _needles[0]);
                //One right in the middle. 
                _haystack.Insert(_haystackSize / 2, _needles[1]);
                //One at the end. 
                _haystack.Insert(_haystack.Count - 1, _needles[2]);
            }
            [ParamsSource(nameof(_needles))]
            public string Needle { get; set; }

            [Benchmark]
            public string Single() => _haystack.SingleOrDefault(x => x == Needle);

            [Benchmark]
            public string First() => _haystack.FirstOrDefault(x => x == Needle);

        }

        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<MemoryAnalyzer>();
            Console.ReadLine();
        }
    }
}
