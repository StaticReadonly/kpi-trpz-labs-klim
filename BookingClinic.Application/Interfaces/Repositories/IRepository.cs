namespace BookingClinic.Application.Interfaces.Repositories
{
    public interface IRepository<T, in TKey> where T : class
    {
        T? GetById(TKey key);
        IEnumerable<T> GetAll();
        void AddEntity(T entity);
        void AddRange(IEnumerable<T> entities);
        void UpdateEntity(T entity);
        void DeleteEntity(T entity);
        Task SaveChangesAsync();
    }
}
