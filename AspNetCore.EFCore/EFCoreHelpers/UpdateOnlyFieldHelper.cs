using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.EFCore.EFCoreHelpers
{
    /// <summary>
    /// <see cref="TEntity"/> 应该约束为实体接口标记
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class UpdateOnlyFieldHelper<TEntity>
        where TEntity : class
    {
        public async Task<int> UpdateAsync(TEntity entity, List<string> fieldNames,CancellationToken cancellationToken = default)
        {
            using (var context = new MyContext())
            {
                if (fieldNames?.Count > 0)
                {
                    context.Set<TEntity>().Attach(entity);
                    foreach (var item in fieldNames)
                    {
                        context.Entry<TEntity>(entity)
                            .Property(item)
                            .IsModified = true;
                    }
                }
                else
                    context.Entry(entity)
                        .State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                return await context.SaveChangesAsync(cancellationToken: cancellationToken);

            }
        }
        public async Task UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            using (var context = new MyContext())
            {
                context.Attach(entity);
                EntityEntry<TEntity> entry = context.Entry(entity);
                foreach (var selector in properties)
                {
                    entry.Property(selector).IsModified = true;
                }
                await context.SaveChangesAsync();
            }
        }
        //public async Task<int> UpdateAsync(TEntity entity,Expression<Func<TEntity,object>>[] properties)
        //{
        //    using (var context = new MyContext())
        //    {
        //        context.Set<TEntity>()
        //            .Attach(entity).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
        //        foreach (var property in properties)
        //        {
        //            var propertyName = ExpresstionHelper.GetExpressionText(property);
        //        }
        //    }
        //}
    }
}
