using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore
{
    public static class EnumerableExtension
    {
        public static T RandomEnumerableValue<T>(this IEnumerable<T> source,Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));
            //if(source is ICollection)
            //{
            //    ICollection collection = source as ICollection;
            //    int count = collection.Count;
            //    if(count == 0)
            //    {
            //        throw new Exception("empty data");
            //    }
            //    int index = random.Next(count);
            //    return source.ElementAt(index);
            //}
            using (IEnumerator<T> iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                    throw new Exception("inumerable empty data");
                int count = 1;
                T current = iterator.Current;
                while (iterator.MoveNext())
                {
                    count++;
                    if (random.Next(count) == 0)
                        current = iterator.Current;
                }
                return current;
            }
        }
    }
}
