using DeepCopyCore.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DeepCopyCore
{
    public class DeepCopyByExpression
    {
        private void CompileExpression()
        {
            Expression<Func<Student, StudentDto>> ss = (x) => new StudentDto { Age = x.Age, Name = x.Name, Id = x.Id };
            var func = ss.Compile();
            var studentDto = func(new Student { Age = 25, Name = "MarsonShine", Id = 1 });
        }

        private static Dictionary<string, object> _dic = new Dictionary<string, object>();
        public static TOut TransExp<Tin,TOut>(Tin tin)
        {
            string key = string.Format("trans_exp_{0}_{1}", typeof(Tin).FullName, typeof(TOut).FullName);
            if (!_dic.ContainsKey(key))
            {
                ParameterExpression parameterExpression = Expression.Parameter(typeof(Tin), "p");
                List<MemberBinding> memberBindings = new List<MemberBinding>();
                foreach (var item in typeof(TOut).GetProperties())
                {
                    if (!item.CanWrite) continue;
                    MemberExpression propty = Expression.Property(parameterExpression, typeof(Tin).GetProperty(item.Name));
                    MemberBinding memberBinding = Expression.Bind(item, propty);
                    memberBindings.Add(memberBinding);
                }

                MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindings.ToArray());
                Expression<Func<Tin, TOut>> lambda = Expression.Lambda<Func<Tin, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });
                Func<Tin, TOut> func = lambda.Compile();
                _dic[key] = func;
            }
            return ((Func<Tin, TOut>)_dic[key])(tin);
        }
    }

    public static class TransExpByGeneric<TIn, TOut>
    {
        private static readonly Func<TIn, TOut> cache = GetFunc();
        private static Func<TIn, TOut> GetFunc()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();
            foreach (var item in typeof(TOut).GetProperties())
            {
                if (!item.CanWrite) continue;
                MemberExpression memberExpression = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, memberExpression);
                memberBindingList.Add(memberBinding);
            }

            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });
            return lambda.Compile();
        }

        public static TOut Trans(TIn @in)
        {
            return cache(@in);
        }
    }
}
