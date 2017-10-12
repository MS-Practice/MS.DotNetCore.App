using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreFileUploader.Demo.Models
{
    public class UploadedData
    {
        public string Name { get; internal set; }
        public int Age { get; internal set; }
        public string ZipCode { get; internal set; }
        public string FilePath { get; internal set; }
    }
}
