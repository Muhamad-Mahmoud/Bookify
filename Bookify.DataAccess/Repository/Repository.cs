using Bookify.DL.Data;
using Bookify.DL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Linq.Expressions;

namespace Bookify.DL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BookifyDbContext _dbContext;
        internal DbSet<T> dbSet;
        public Repository(BookifyDbContext dbContext)
        {
            _dbContext = dbContext;
            this.dbSet = _dbContext.Set<T>();
        }

        public Task AddAsync(T entity) => dbSet.AddAsync(entity).AsTask();

        public async Task<T?> GetAsync(int id) => await dbSet.FindAsync(id);

        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string includeProperties = null)
        {
            IQueryable<T> query = dbSet.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in
                    includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(
                Expression<Func<T, bool>> filter = null, 
                    string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.ToListAsync();
        }


        public void Remove(T entity) => dbSet.Remove(entity);

        public void RemoveRange(IEnumerable<T> entity) => dbSet.RemoveRange(entity);

        
    }
}