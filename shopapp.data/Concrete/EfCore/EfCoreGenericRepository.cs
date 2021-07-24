using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        }

        public void Delete(TEntity entity)
        {
                context.Set<TEntity>().Remove(entity);
        }

        public async Task<List<TEntity>> GetAll()
        {
                return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
                return await context.Set<TEntity>().FindAsync(id);
        }

        public virtual void Update(TEntity entity)
        {
                context.Entry(entity).State=EntityState.Modified;
        }
    }
}