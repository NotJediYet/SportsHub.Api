using Microsoft.EntityFrameworkCore;
using SportsHub.Web.AppData;
using SportsHub.Web.Interfaces;

namespace SportsHub.Web.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        readonly protected AppDbContext _context;
        readonly protected DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get()
        {
            return _dbSet.ToList();
        }

        public virtual TEntity? GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }
    }
}
