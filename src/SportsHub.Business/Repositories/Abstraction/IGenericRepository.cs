namespace SportsHub.Business.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class 
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(Guid id);

        Task AddAsync(TEntity entity);

        Task<int> SaveAsync();
    }
}
