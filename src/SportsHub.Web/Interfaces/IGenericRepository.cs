namespace SportsHub.Web.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get();
        TEntity? GetByID(object id);
        void Add(TEntity entity);
    }
}
