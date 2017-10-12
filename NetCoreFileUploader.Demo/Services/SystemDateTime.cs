using NetCoreFileUploader.Demo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreFileUploader.Demo.Services
{
    public class SystemDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
