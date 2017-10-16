using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NetCoreControllers.Demo.Model
{
    public static class EntityTypes
    {
        public static IReadOnlyList<TypeInfo> Types => new List<TypeInfo>()
        {
            typeof(Sprockt).GetTypeInfo(),
            typeof(Widget).GetTypeInfo()
        };

        public class Sprockt { }
        public class Widget { }
    }
}
