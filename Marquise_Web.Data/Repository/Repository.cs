using Marquise_Web.Data.IRepository;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Marquise_Web.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext context;

        public Repository(DbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync() => await context.Set<T>().ToListAsync();

        public virtual async Task<T> GetByIdAsync(int id) => await context.Set<T>().FindAsync(id);

        public virtual async Task AddAsync(T entity) => context.Set<T>().Add(entity);

        public virtual async Task UpdateAsync(T entity) => context.Entry(entity).State = EntityState.Modified;

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            if (entity != null) context.Set<T>().Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync() => await context.SaveChangesAsync();
    }
}
