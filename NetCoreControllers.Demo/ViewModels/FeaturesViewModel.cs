using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NetCoreControllers.Demo.ViewModels
{
    public class FeaturesViewModel
    {
        public List<System.Reflection.TypeInfo> Controllers { get; set; }

        public List<MetadataReference> MetadataReferences { get; set; }

        public List<System.Reflection.TypeInfo> TagHelpers { get; set; }

        public List<System.Reflection.TypeInfo> ViewComponents { get; set; }
    }
}
