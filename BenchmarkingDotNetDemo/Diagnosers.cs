using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BenchmarkingDotNetDemo
{
    [HtmlExporter]
    [MemoryDiagnoser]
    public class MemoryAnalyzer
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                Add(MemoryDiagnoser.Default);
                Add(new HtmlExporter());
            }
        }

        [Benchmark]
        public IEnumerable<string> Boxes()
        {
            return Enumerable.Range(1, 100000).Where(d => d % 2 == 0).Cast<string>();
        }
        [Benchmark]
        public IEnumerable<int> UnBoxes()
        {
            return Enumerable.Range(1, 100000).Where(d => d % 2 == 0);
        }
    }
}
