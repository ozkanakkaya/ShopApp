using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreGenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class//TEntity bir class olmalı
        //where TContext : DbContext, new()//TContext e DbContext ten türeyen bir class gönderilmeli, new ile bu generic yapının newlenebileceğini belirttik,yani instance ı oluşturulabilir.
    {
        protected readonly DbContext context;

        public EfCoreGenericRepository(DbContext ctx)
        {
            context = ctx;
        }


        public void Create(TEntity entity)
        {
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
                context.Set<TEntity>().Remove(entity);
                context.SaveChanges();
        }

        public List<TEntity> GetAll()
        {
                return context.Set<TEntity>().ToList();
        }

        public TEntity GetById(int id)
        {
                return context.Set<TEntity>().Find(id);
        }

        public virtual void Update(TEntity entity)
        {
                context.Entry(entity).State=EntityState.Modified;
                context.SaveChanges();
        }
    }
}