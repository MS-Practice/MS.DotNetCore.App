using System;

namespace DeepCopyCore
{
    public class DeepCopyByRelection
    {
        public static TOut TransRelection<Tin,TOut>(Tin tin)
        {
            TOut tout = Activator.CreateInstance<TOut>();
            var tinType = tin.GetType();
            foreach (var itemOut in tout.GetType().GetProperties())
            {
                var itemIn = tinType.GetProperty(itemOut.Name);
                if(itemIn != null)
                {
                    itemOut.SetValue(tout, itemIn.GetValue(tin));
                }
            }
            return tout;
        }
    }
}
