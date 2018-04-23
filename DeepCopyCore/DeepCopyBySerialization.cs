using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCopyCore
{
    public class DeepCopyBySerialization
    {
        public static TOut TransSerialization<Tin,TOut>(Tin tin)
        {
            var ss = JsonConvert.SerializeObject(tin);
            return JsonConvert.DeserializeObject<TOut>(ss);
        }
    }
}
